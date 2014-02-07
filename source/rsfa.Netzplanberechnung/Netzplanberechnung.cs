using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rsfa.contracts;
using rsfa.contracts.daten;

namespace Netzplanberechnung
{
    public class Netzplanberechnung : INetzplanberechnung
    {
        public Netzplan Netzplan_berechnen()
        {
            return new Netzplan();
        }
    }
}
