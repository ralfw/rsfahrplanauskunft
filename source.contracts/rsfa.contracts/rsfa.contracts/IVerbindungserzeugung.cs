using System;
using System.Collections.Generic;
using rsfa.contracts.daten;

namespace rsfa.contracts
{
    public interface IVerbindungserzeugung
    {
        Verbindung[] Verbindugen_zu_Pfaden_bilden(IEnumerable<Pfad> pfade, DateTime startzeit);
    }
}
