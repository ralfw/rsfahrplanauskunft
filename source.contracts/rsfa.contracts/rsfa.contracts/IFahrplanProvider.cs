using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsfa.contracts
{
    public interface IFahrplanProvider
    {
        string[] Liniennamen { get; }
        string[] Haltestellen_für_Linie(string linienname);

        TimeSpan Fahrtdauer_für_Strecke(string linienname, string haltestellenname);
        DateTime[] Abfahrtszeiten_bei_Haltestelle(string linienname, string haltestellenname);
    }
}
