﻿using System;
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
            // Hallo3
            return new Netzplan();
        }
    }
}
