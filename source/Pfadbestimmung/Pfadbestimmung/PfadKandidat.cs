using rsfa.contracts.daten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsfa.pfadbestimmung
{
   class PfadKandidat
   {
      public PfadKandidat(Haltestelle starthaltestelle)
      {
         this.Starthaltestelle = starthaltestelle;
         this.Strecken = new List<Strecke>();
      }

      public Haltestelle Starthaltestelle { get; private set; }

      public List<Strecke> Strecken { get; private set; }

      public Pfad ErstellePfad()
      {
         var pfad = new Pfad()
         {
            Starthaltestellenname = this.Starthaltestelle.Name,
            Strecken = this.Strecken.ToArray()
         };

         return pfad;
      }

      public PfadKandidat Clone()
      {
         var klon = new PfadKandidat(this.Starthaltestelle);
         klon.Strecken = new List<Strecke>(this.Strecken);
         return klon;
      }
   }
}
