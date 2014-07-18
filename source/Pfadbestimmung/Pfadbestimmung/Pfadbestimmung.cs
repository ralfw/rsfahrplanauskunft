using rsfa.contracts;
using rsfa.contracts.daten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsfa.pfadbestimmung
{
    using System.Collections.Concurrent;
    using System.Threading;

    public class Pfadbestimmung : IPfadbestimmung
   {

      public event Action<Pfad> OnPfad;

      private Netzplan netzplan;

      private Haltestelle starthaltestelle;

      private Haltestelle zielhaltestelle;
 
      private ConcurrentBag<PfadKandidat> kandidaten;

        private ConcurrentBag<PfadKandidat> akzeptiertePfadKandidaten;

        private Int32 numberOfRunningWorkers = 0;
 
      public void Alle_Pfade_bestimmen(Netzplan netzplan, string starthaltestellenname, string zielhaltestellenname)
      {
         this.netzplan = netzplan;
         if (this.netzplan == null)
         {
            throw new ArgumentNullException("netzplan");
         }

         this.starthaltestelle = this.FindHaltestelle(starthaltestellenname);
         if (this.starthaltestelle == null)
         {
            throw new InvalidOperationException("Starthaltestelle nicht gefunden");
         }

         this.zielhaltestelle = FindHaltestelle(zielhaltestellenname);
         if (this.zielhaltestelle == null)
         {
            throw new InvalidOperationException("Zielhaltestelle nicht gefunden");
         }

         var initialKandidat = new PfadKandidat(starthaltestelle);

         this.StarteSuche(initialKandidat, numberOfWorkers: 7);
         this.StarteAusgabe();
         
         this.OutputEndOfSteam();
      }

      private void StarteAusgabe()
      {
          do
          {
              PfadKandidat pfadKandidat;
              while (this.akzeptiertePfadKandidaten.TryTake(out pfadKandidat))
              {
                  this.Output(pfadKandidat);
              }
              Thread.Yield();
          } while (this.numberOfRunningWorkers > 0);
      }

      private void StarteSuche(PfadKandidat initial_kandidat, int numberOfWorkers)
      {
          this.kandidaten = new ConcurrentBag<PfadKandidat>(new[] { initial_kandidat });
          this.akzeptiertePfadKandidaten = new ConcurrentBag<PfadKandidat>();

          while (numberOfWorkers > 0)
          {
              var handle = Task.Run(() => this.ProduziereKandidat());
                  
              numberOfWorkers--;
          }

          while (this.numberOfRunningWorkers == 0)
          {
              Thread.Yield();
          }
      }

        private void ProduziereKandidat()
        {
            do
            {
                Interlocked.Increment(ref this.numberOfRunningWorkers);

                PfadKandidat kandidat;
                while (this.kandidaten.TryTake(out kandidat))
                {
                    this.BackTrack(kandidat);
                }

                Thread.Yield();
                Interlocked.Decrement(ref this.numberOfRunningWorkers);
            }
            while (this.numberOfRunningWorkers > 0);
        }

        private void BackTrack(PfadKandidat kandidat)
        {
            if (this.Reject(kandidat))
            {
                return;
            }

            if (this.Accept(kandidat))
            {
                this.akzeptiertePfadKandidaten.Add(kandidat);
                return;
            }

            Haltestelle zielhaltestelle;
            if (kandidat.StreckenCount == 0)
            {
                zielhaltestelle = kandidat.Starthaltestelle;
            }
            else
            {
                var zielhaltestellenname = kandidat.Zielhaltestellenname;
                zielhaltestelle = this.FindHaltestelle(zielhaltestellenname);
            }

            foreach (var strecke in zielhaltestelle.Strecken)
            {
                var nextKandidat = kandidat.Clone();
                nextKandidat.AddStrecke(strecke);
                this.kandidaten.Add(nextKandidat);
            }
        }

        private Haltestelle FindHaltestelle(String haltestellenname)
      {
         return this.netzplan.Haltestellen.FirstOrDefault(h => h.Name == haltestellenname);
      }

      private Boolean Reject(PfadKandidat kandidat)
      {
         if (kandidat.StreckenCount == 0)
         {
            return false;
         }

         var lastHaltestellenname = kandidat.Zielhaltestellenname;
         if (lastHaltestellenname == this.starthaltestelle.Name)
         {
            return true;
         }

         if (kandidat.Strecken.Skip(1).Any(s => s.Zielhaltestellenname == lastHaltestellenname))
         {
            return true;
         }

         return false;
      }

      private Boolean Accept(PfadKandidat kandidat)
      {
         if (kandidat.StreckenCount == 0)
         {
            return false;
         }

         if (kandidat.Zielhaltestellenname == this.zielhaltestelle.Name)
         {
            return true;
         }

         return false;
      }

      private void Output(PfadKandidat kandidat)
      {
         if (this.OnPfad != null)
         {
            this.OnPfad(kandidat.ErstellePfad());
         }
      }

      private void OutputEndOfSteam()
      {
          if (this.OnPfad != null)
          {
              this.OnPfad(null);
          }
      }
   }
}
