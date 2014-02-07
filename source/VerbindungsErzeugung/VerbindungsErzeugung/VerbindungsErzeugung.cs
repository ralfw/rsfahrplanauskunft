using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rsfa.contracts;
using rsfa.contracts.daten;

namespace VerbindungsErzeugung
{
    public class VerbindungsErzeugung : IVerbindungserzeugung
    {
        public event Action<Verbindung> OnVerbindung;

        public void Verbindugen_zu_Pfad_bilden(Pfad pfad, DateTime startzeit)
        {
            if (pfad == null && this.OnVerbindung != null)
            {
                this.OnVerbindung(null);
                return;
            }

            var verbOhneZeit = this.Verbindungenerzeugen(pfad);
            var verbMitZeit = this.FahrzeitenZuordnen(verbOhneZeit);
            var verbEingeschraenkt = this.EinschraenkenNachFahrzeit(verbMitZeit, startzeit);
            
            if (this.OnVerbindung != null)
            {
                foreach (var item in verbEingeschraenkt)
                {
                    OnVerbindung(item);                
                }
            }
        }

        private Verbindung[] Verbindungenerzeugen(Pfad pfad)
        {
            var verbindungen = new List<Verbindung>();

            return verbindungen.ToArray();
        }

        private Verbindung[] FahrzeitenZuordnen(IEnumerable<Verbindung> verbindungen)
        {
            return verbindungen.ToArray(); ;
        }

        private Verbindung[] EinschraenkenNachFahrzeit(IEnumerable<Verbindung> verbindungen, DateTime startZeit)
        {
            return verbindungen.ToArray(); ;
        }
    }
}
