using System;
using System.Collections.Generic;
using rsfa.contracts.daten;

namespace rsfa.contracts
{
    public interface IVerbindungsbewertung
    {
        void Verbindungen_bewerten(Verbindung verbindung);
        event Action<Verbindung[]> OnVerbindungenKomplett;
    }
}
