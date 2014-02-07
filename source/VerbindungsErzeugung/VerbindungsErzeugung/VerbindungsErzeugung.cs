namespace VerbindungsErzeugung
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using rsfa.contracts;
    using rsfa.contracts.daten;

    public class VerbindungsErzeugung : IVerbindungserzeugung
    {
        /// <summary>
        /// The fahrplan provider.
        /// </summary>
        private IFahrplanProvider fahrplanProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="VerbindungsErzeugung"/> class.
        /// </summary>
        /// <param name="fahrplanProvider">
        /// The fahrplan provider.
        /// </param>
        public VerbindungsErzeugung(IFahrplanProvider fahrplanProvider)
        {
            this.fahrplanProvider = fahrplanProvider;
        }

        /// <summary>
        /// The on verbindung.
        /// </summary>
        public event Action<Verbindung> OnVerbindung;

        /// <summary>
        /// The verbindugen_zu_ pfad_bilden.
        /// </summary>
        /// <param name="pfad">
        /// The pfad.
        /// </param>
        /// <param name="startzeit">
        /// The startzeit.
        /// </param>
        public void Verbindugen_zu_Pfad_bilden(Pfad pfad, DateTime startzeit)
        {
            if (this.OnVerbindung == null)
            {
                return;
            }

            if (pfad == null)
            {
                this.OnVerbindung(null);
                return;
            }

            var verbOhneZeit = this.Verbindungenerzeugen(pfad);
            var verbMitZeit = this.FahrzeitenZuordnen(verbOhneZeit);
            var verbEingeschraenkt = this.EinschraenkenNachFahrzeit(verbMitZeit, startzeit);

            foreach (var item in verbEingeschraenkt)
            {
                this.OnVerbindung(item);
            }
        }

        /// <summary>
        /// The verbindungenerzeugen.
        /// </summary>
        /// <param name="pfad">
        /// The pfad.
        /// </param>
        /// <returns>
        /// The <see cref="Verbindung[]"/>.
        /// </returns>
        private Verbindung[] Verbindungenerzeugen(Pfad pfad)
        {
            var linienName = pfad.Strecken.First().Linienname;
            var startHalteStelle = pfad.Starthaltestellenname;

            var abfahrtszeiten = this.fahrplanProvider.Abfahrtszeiten_bei_Haltestelle(linienName, startHalteStelle);
            var verbindungen = new Verbindung[abfahrtszeiten.Length];

            for (int index = 0; index < abfahrtszeiten.Length; index++)
            {
                var zeit = abfahrtszeiten[index];
                var verbindung = new Verbindung { Pfad = pfad, Fahrtzeiten = new Fahrtzeit[pfad.Strecken.Length] };

                // Startzeit der Verbindung ist nur hier bekannt
                verbindung.Fahrtzeiten[0].Abfahrtszeit = zeit;
                verbindungen[index] = verbindung;
            }

            return verbindungen;
        }

        /// <summary>
        /// The fahrzeiten zuordnen.
        /// </summary>
        /// <param name="verbindungen">
        /// The verbindungen.
        /// </param>
        /// <returns>
        /// The <see cref="Verbindung[]"/>.
        /// </returns>
        private Verbindung[] FahrzeitenZuordnen(Verbindung[] verbindungen)
        {
            foreach (var verbindung in verbindungen)
            {
                // Init
                var startHalteStelle = verbindung.Pfad.Starthaltestellenname;
                var ankunftzeitLetzteStrecke = verbindung.Fahrtzeiten[0].Abfahrtszeit;

                for (var i = 0; i < verbindung.Pfad.Strecken.Length; i++)
                {
                    var linienName = verbindung.Pfad.Strecken[i].Linienname;

                    // Alle Zeiten an dieser Haltestelle
                    var startZeitenHalteStelle = this.fahrplanProvider.Abfahrtszeiten_bei_Haltestelle(
                        linienName, startHalteStelle);

                    // Nehme die nächste verfügbare Abfahrtszeit
                    // Rollator use case wird nicht berücksichtigt
                    verbindung.Fahrtzeiten[i].Abfahrtszeit = startZeitenHalteStelle.First(zeit => zeit >= ankunftzeitLetzteStrecke);

                    // Frage the Fahrplan über die Dauer der Strecke
                    var dauer = this.fahrplanProvider.Fahrtdauer_für_Strecke(linienName, startHalteStelle);

                    // Ankunftszeit
                    verbindung.Fahrtzeiten[i].Ankunftszeit = verbindung.Fahrtzeiten[i].Abfahrtszeit + dauer;

                    // Merke für nächste Runde
                    startHalteStelle = verbindung.Pfad.Strecken[i].Zielhaltestellenname;
                    ankunftzeitLetzteStrecke = verbindung.Fahrtzeiten[i].Ankunftszeit;
                }
            }

            return verbindungen;
        }

        /// <summary>
        /// The einschraenken nach fahrzeit.
        /// </summary>
        /// <param name="verbindungen">
        /// The verbindungen.
        /// </param>
        /// <param name="startZeit">
        /// The start zeit.
        /// </param>
        /// <returns>
        /// The <see cref="Verbindung[]"/>.
        /// </returns>
        private Verbindung[] EinschraenkenNachFahrzeit(IEnumerable<Verbindung> verbindungen, DateTime startZeit)
        {
            return verbindungen.Where(verb => verb.Fahrtzeiten[0].Abfahrtszeit >= startZeit).ToArray();
        }
    }
}
