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
            return new  DateTime[3];
        }

        public TimeSpan[] Fahrtdauern_für_Linie(string linienname)
        {
            return new TimeSpan[3];
        }

        public string[] Haltestellen_für_Linie(string linienname)
        {
            return new string[3];
        }

        public string[] Liniennamen
        {
            get
            {
                return new string[3];
            }
        }
    }
}
