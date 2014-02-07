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
        readonly TraceSource _ts = new TraceSource("Verbindungsbewertung");


        public void Verbindungen_bewerten(Verbindung verbindung)
        {
            Verbindung_registrieren(verbindung, 
                Auswerten);
        }


        private List<Verbindung> _verbindungen = new List<Verbindung>(); 
        void Verbindung_registrieren(Verbindung verbindung, Action<Verbindung[]> onEndOfStream)
        {
            if (verbindung != null)
                _verbindungen.Add(verbindung);
            else
            {
                var tmp = _verbindungen;
                _verbindungen = null;
                onEndOfStream(tmp.ToArray());
            }
        }


        void Auswerten(IEnumerable<Verbindung> verbindungen)
        {
            _ts.TraceInformation("Verbindungen auswerten");

            var scored = verbindungen.Select(v => new { Score = v.Pfad.Strecken.Length, Verbindung = v });
            var sorted = scored.OrderBy(s => s.Score).Select(s => s.Verbindung).Take(5);
            OnVerbindungenKomplett(sorted.ToArray());
        }


        public event Action<Verbindung[]> OnVerbindungenKomplett;
    }
}
