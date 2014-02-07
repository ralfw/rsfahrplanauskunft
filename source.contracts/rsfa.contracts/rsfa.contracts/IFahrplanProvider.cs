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

        TimeSpan[] Fahrtdauern_für_Linie(string linienname);
        DateTime[] Abfahrtszeiten_bei_Haltestelle(string linienname, string haltestellenname);
    }
}
