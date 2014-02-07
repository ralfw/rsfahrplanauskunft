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
      public void Alle_Pfade_bestimmen(Netzplan netzplan, string starthaltestellenname, string zielhaltestellenname)
      {
         if (this.OnPfad != null)
         {
            this.OnPfad(new Pfad());
         }
      }

      public event Action<Pfad> OnPfad;
   }
}
