using System;
using rsfa.contracts.daten;

namespace rsfa.contracts
{
    public interface IPfadbestimmung
    {
        void Alle_Pfade_bestimmen(Netzplan netzplan, 
                                  string starthaltestellenname, 
                                  string zielhaltestellenname);
        event Action<Pfad> OnPfad;
    }
}
