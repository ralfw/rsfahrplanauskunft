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
      private List<Strecke> strecken; 

      public PfadKandidat(Haltestelle starthaltestelle)
      {
         this.Starthaltestelle = starthaltestelle;
         this.strecken = new List<Strecke>();
      }

      public Haltestelle Starthaltestelle { get; private set; }

      public IEnumerable<Strecke> Strecken { get { return this.strecken; } }

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
         klon.strecken = new List<Strecke>(this.Strecken);
         return klon;
      }

      internal void AddStrecke(Strecke strecke)
      {
          this.strecken.Add(strecke);
      }

      public int StreckenCount { get { return this.strecken.Count; } }
   }
}
