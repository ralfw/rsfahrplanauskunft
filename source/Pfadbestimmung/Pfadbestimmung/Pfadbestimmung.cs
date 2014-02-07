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
         if (netzplan == null)
         {
            throw new ArgumentNullException("netzplan");
         }

         var starthaltestelle = this.FindHaltestelle(starthaltestellenname);
         if (starthaltestelle == null)
         {
            throw new InvalidOperationException("Starthaltestelle nicht gefunden");
         }

         var stophaltestelle = FindHaltestelle(zielhaltestellenname);
         if (stophaltestelle == null)
         {
            throw new InvalidOperationException("Zielhaltestelle nicht gefunden");
         }

         this.netzplan = netzplan;
         this.starthaltestelle = starthaltestelle;
         this.zielhaltestelle = stophaltestelle;

         var initialKandidat = new PfadKandidat(starthaltestelle);

         this.BackTrack(initialKandidat);
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

         var zielhaltestellenname = kandidat.Strecken.Last().Zielhaltestellenname;
         var zielhaltestelle = this.FindHaltestelle(zielhaltestellenname);
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
         if (kandidat.Strecken.Last().Zielhaltestellenname == this.starthaltestelle.Name)
         {
            return true;
         }

         if (kandidat.Strecken.Count(s => s.Zielhaltestellenname == this.zielhaltestelle.Name) > 1)
         {
            return true;
         }

         return false;
      }

      private Boolean Accept(PfadKandidat kandidat)
      {
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
   }
}
