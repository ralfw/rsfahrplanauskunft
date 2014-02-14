using rsfa.contracts;
using rsfa.contracts.daten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsfa.pfadbestimmung
{
   public class Pfadbestimmung : IPfadbestimmung
   {

      public event Action<Pfad> OnPfad;

      private Netzplan netzplan;

      private Haltestelle starthaltestelle;

      private Haltestelle zielhaltestelle;

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

         this.BackTrack(initialKandidat);

         this.OutputEndOfSteam();
      }

      private void BackTrack(PfadKandidat kandidat)
      {
         if (this.Reject(kandidat))
         {
            return;
         }

         if (this.Accept(kandidat))
         {
            this.Output(kandidat);
            return; // we can also stop here, as everything else will cause an reject anyway
         }

         Haltestelle zielhaltestelle;
         if (kandidat.Strecken.Count == 0)
         {
            zielhaltestelle = kandidat.Starthaltestelle;
         }
         else
         {
            var zielhaltestellenname = kandidat.Strecken.Last().Zielhaltestellenname;
            zielhaltestelle = this.FindHaltestelle(zielhaltestellenname);
         }

         foreach (var strecke in zielhaltestelle.Strecken)
         {
            var nextKandidat = kandidat.Clone();
            nextKandidat.Strecken.Add(strecke);
            this.BackTrack(nextKandidat);
         }
      }

      private Haltestelle FindHaltestelle(String haltestellenname)
      {
         return this.netzplan.Haltestellen.FirstOrDefault(h => h.Name == haltestellenname);
      }

      private Boolean Reject(PfadKandidat kandidat)
      {
         if (kandidat.Strecken.Count == 0)
         {
            return false;
         }

         var lastHaltestellenname = kandidat.Strecken.Last().Zielhaltestellenname;
         if (lastHaltestellenname == this.starthaltestelle.Name)
         {
            return true;
         }

         if (kandidat.Strecken.Count(s => s.Zielhaltestellenname == lastHaltestellenname) > 1)
         {
            return true;
         }

         return false;
      }

      private Boolean Accept(PfadKandidat kandidat)
      {
         if (kandidat.Strecken.Count == 0)
         {
            return false;
         }

         if (kandidat.Strecken.Last().Zielhaltestellenname == this.zielhaltestelle.Name)
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
