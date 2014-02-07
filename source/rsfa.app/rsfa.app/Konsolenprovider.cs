using System;
using System.Collections.Generic;
using rsfa.contracts.daten;

namespace rsfa.app
{
    internal class Konsolenprovider
    {
        public void Verbindungen_anzeigen(IEnumerable<Verbindung> verbindungen)
        {
            foreach (var v in verbindungen)
            {
                Console.WriteLine("Verb: {0}", v.Pfad.Starthaltestellenname);
            }
        }
    }
}