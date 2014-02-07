using System.Collections.Generic;
using rsfa.contracts.daten;

namespace rsfa.contracts
{
    public interface IVerbindungsbewertung
    {
        Verbindung[] Verbindungen_bewerten(IEnumerable<Verbindung> verbindungen);
    }
}
