using System;
using System.Collections.Generic;
using rsfa.contracts.daten;

namespace rsfa.contracts
{
    public interface IVerbindungserzeugung
    {
        void Verbindugen_zu_Pfad_bilden(Pfad pfad, DateTime startzeit);
        event Action<Verbindung> OnVerbindung;
    }
}
