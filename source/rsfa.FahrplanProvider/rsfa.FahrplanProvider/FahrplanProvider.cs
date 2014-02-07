using rsfa.contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsfa.FahrplanProvider
{
    public class FahrplanProvider : IFahrplanProvider
    {

        public DateTime[] Abfahrtszeiten_bei_Haltestelle(string linienname, string haltestellenname)
        {
            return new DateTime[3] { DateTime.Now.AddMinutes(5), DateTime.Now.AddMinutes(10), DateTime.Now.AddMinutes(15), };
        }

        public TimeSpan[] Fahrtdauern_für_Linie(string linienname)
        {
            return new TimeSpan[3] { TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(4), TimeSpan.FromMinutes(3), };
        }

        public string[] Haltestellen_für_Linie(string linienname)
        {
            return new string[3] { "Hintertupfing", "Vordertupfing", "Obertupfing" };
        }
        /// <summary>
        /// /sdfsad
        /// </summary>
        public string[] Liniennamen
        {
            get
            {
                return new string[4] { "Kaiser-Franz Linie 8", "Transrapid Linie Stoiber", "Franz-Josef Linie 15", "Hans-Dampf-Seehofer Linie"};
            }
        }
    }
}
