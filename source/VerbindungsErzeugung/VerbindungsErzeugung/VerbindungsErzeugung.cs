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
        /// Aus Pfaden Verbindungen bilden.
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
        /// Erzeugt die Verbindungen aus den gegebenen Pfaden.
        /// </summary>
        /// <param name="pfad">
        /// The pfad.
        /// </param>
        /// <returns>
        /// The <see cref="Verbindung[]"/>.
        /// </returns>
        internal Verbindung[] Verbindungenerzeugen(Pfad pfad)
        {
            var linienName = pfad.Strecken.First().Linienname;
            var startHalteStelle = pfad.Starthaltestellenname;

            var abfahrtszeiten = this.fahrplanProvider.Abfahrtszeiten_bei_Haltestelle(linienName, startHalteStelle);
            var verbindungen = new Verbindung[abfahrtszeiten.Length];

            for (int index = 0; index < abfahrtszeiten.Length; index++)
            {
                var zeit = abfahrtszeiten[index];
                var verbindung = new Verbindung { Pfad = pfad, Fahrtzeiten = new Fahrtzeit[pfad.Strecken.Length] };
                for (int i = 0; i < pfad.Strecken.Length; i++)
                {
                    verbindung.Fahrtzeiten[i] = new Fahrtzeit();
                }

                // Startzeit der Verbindung ist nur hier bekannt
                verbindung.Fahrtzeiten[0].Abfahrtszeit = zeit;
                verbindungen[index] = verbindung;
            }

            return verbindungen;
        }

        /// <summary>
        /// Ordnet den Verbindungen die Fahrzeiten zu.
        /// </summary>
        /// <param name="verbindungen">
        /// The verbindungen.
        /// </param>
        /// <returns>
        /// The <see cref="Verbindung[]"/>.
        /// </returns>
        internal Verbindung[] FahrzeitenZuordnen(Verbindung[] verbindungen)
        {
            var tempVerbindungen = new List<Verbindung>(verbindungen.Length);

            foreach (var verbindung in verbindungen)
            {
                // Init
                var startHalteStelle = verbindung.Pfad.Starthaltestellenname;
                var ankunftzeitLetzteStrecke = verbindung.Fahrtzeiten[0].Abfahrtszeit;
                var gueltigeVerbindung = false;

                for (var i = 0; i < verbindung.Pfad.Strecken.Length; i++)
                {
                    var linienName = verbindung.Pfad.Strecken[i].Linienname;

                    // Alle Zeiten an dieser Haltestelle
                    var startZeitenHalteStelle = this.fahrplanProvider.Abfahrtszeiten_bei_Haltestelle(
                        linienName, startHalteStelle);

                    // Nehme die nächste verfügbare Abfahrtszeit
                    // Rollator use case wird nicht berücksichtigt
                    foreach (var dateTime in startZeitenHalteStelle)
                    {
                        if (dateTime < ankunftzeitLetzteStrecke)
                        {
                            continue;
                        }
                        
                        verbindung.Fahrtzeiten[i].Abfahrtszeit = dateTime;
                        gueltigeVerbindung = true;
                        break;
                    }

                    // Frage den Fahrplan über die Dauer der Strecke
                    var dauer = this.fahrplanProvider.Fahrtdauer_für_Strecke(linienName, startHalteStelle);

                    // Ankunftszeit
                    verbindung.Fahrtzeiten[i].Ankunftszeit = verbindung.Fahrtzeiten[i].Abfahrtszeit + dauer;

                    // Merke für nächste Runde
                    startHalteStelle = verbindung.Pfad.Strecken[i].Zielhaltestellenname;
                    ankunftzeitLetzteStrecke = verbindung.Fahrtzeiten[i].Ankunftszeit;
                }

                if (gueltigeVerbindung)
                {
                    tempVerbindungen.Add(verbindung);
                }
            }

            return tempVerbindungen.ToArray();
        }

        /// <summary>
        /// Schraenkt die gegebenen Verbindungen nach fahrzeit ein.
        /// </summary>
        /// <param name="verbindungen">
        /// Die verbindungen.
        /// </param>
        /// <param name="startZeit">
        /// Die start zeit.
        /// </param>
        /// <returns>
        /// The <see cref="Verbindung[]"/>.
        /// </returns>
        internal Verbindung[] EinschraenkenNachFahrzeit(IEnumerable<Verbindung> verbindungen, DateTime startZeit)
        {
            return verbindungen.Where(verb => verb.Fahrtzeiten[0].Abfahrtszeit >= startZeit).ToArray();
        }
    }
}
