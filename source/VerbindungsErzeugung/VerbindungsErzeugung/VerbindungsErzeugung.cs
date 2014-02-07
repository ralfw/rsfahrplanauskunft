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
        private IFahrplanProvider fahrplanProvider;

        public VerbindungsErzeugung(IFahrplanProvider fahrplanProvider)
        {
            this.fahrplanProvider = fahrplanProvider;
        }

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
            var linienName = pfad.Strecken.First().Linienname;
            var startHalteStelle = pfad.Starthaltestellenname;

            var abfahrtszeiten = this.fahrplanProvider.Abfahrtszeiten_bei_Haltestelle(linienName, startHalteStelle);

            for (int i = 0; i < abfahrtszeiten.Length; i++)
            {
                var verbindung = new Verbindung { Pfad = pfad, Fahrtzeiten = new Fahrtzeit[pfad.Strecken.Length] };

                verbindung.Fahrtzeiten[0].Abfahrtszeit = abfahrtszeiten[i];
                verbindungen.Add(verbindung);
            }

            return verbindungen.ToArray();
        }

        private Verbindung[] FahrzeitenZuordnen(Verbindung[] verbindungen)
        {
            foreach (var verbindung in verbindungen)
            {
                var startHalteStelle = verbindung.Pfad.Starthaltestellenname;
                var startZeitAtHalteStelle = verbindung.Fahrtzeiten[0].Abfahrtszeit;

                for (var i = 0; i < verbindung.Pfad.Strecken.Length; i++)
                {
                    verbindung.Fahrtzeiten[i].Ankunftszeit = startZeitAtHalteStelle;

                    var dauer = this.fahrplanProvider.Fahrtdauer_für_Strecke(
                        verbindung.Pfad.Strecken[i].Linienname,
                        startHalteStelle);

                    verbindung.Fahrtzeiten[i].Ankunftszeit = startZeitAtHalteStelle + dauer;

                    startHalteStelle = verbindung.Pfad.Strecken[i].Zielhaltestellenname;
                }
            }            

            return verbindungen.ToArray();
        }

        private Verbindung[] EinschraenkenNachFahrzeit(IEnumerable<Verbindung> verbindungen, DateTime startZeit)
        {
            return verbindungen.ToArray(); ;
        }
    }
}
