using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetzplanberechnungTests
{
    using rsfa.contracts;

    public class TestFahrplanprovider : IFahrplanProvider
    {
        private Dictionary<string, string[]> linienHaltestellen = new Dictionary<string, string[]>(); 
        
        public TestFahrplanprovider()
        {
            this.InitHaltestellen();
        }

        public DateTime[] Abfahrtszeiten_bei_Haltestelle(string linienname, string haltestellenname)
        {
            throw new NotImplementedException();
        }

        public TimeSpan[] Fahrtdauern_für_Linie(string linienname)
        {
            throw new NotImplementedException();
        }

        public TimeSpan Fahrtdauer_für_Strecke(string linienname, string haltestellenname)
        {
            throw new NotImplementedException();
        }

        public string[] Haltestellen_für_Linie(string linienname)
        {
            return this.linienHaltestellen[linienname];
        }

        public string[] Liniennamen
        {
            get
            {
                return this.linienHaltestellen.Keys.ToArray();
            }
        }

        private void InitHaltestellen()
        {
            this.linienHaltestellen.Add("S1OOst", new string[] { "Stachus", "Marienplatz", "Isartor", "Rosenheimer" });
            this.linienHaltestellen.Add("S1OWest", new string[] { "Rosenheimer", "Isartor", "Marienplatz", "Stachus" });
            this.linienHaltestellen.Add("S2", new string[] { "Stachus", "Marienplatz", "Isartor", "Rosenheimer" });
            this.linienHaltestellen.Add("U3", new string[] { "Sendlinger", "Marienplatz", "Odeons" });

        }
    }
}
