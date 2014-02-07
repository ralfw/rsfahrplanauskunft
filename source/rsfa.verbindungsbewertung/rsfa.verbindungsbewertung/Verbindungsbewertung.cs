using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rsfa.contracts;
using rsfa.contracts.daten;

namespace rsfa.verbindungsbewertung
{
    public class Verbindungsbewertung : IVerbindungsbewertung
    {
        TraceSource _ts = new TraceSource("Verbindungsbewertung");


        public void Verbindungen_bewerten(Verbindung verbindung)
        {
            _ts.TraceInformation("Verbindungen_bewerten");
            var v = new Verbindung();
            OnVerbindungenKomplett(new[] {v});
        }

        public event Action<Verbindung[]> OnVerbindungenKomplett;
    }
}
