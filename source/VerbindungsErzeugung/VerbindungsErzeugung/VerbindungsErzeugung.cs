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
                throw new InvalidOperationException("Event OnVerbindung is not specified.");
            }

            if (pfad == null)
            {
                this.OnVerbindung(null);
                return;
            }

            foreach (var verbindungOhneZeit in this.Verbindungenerzeugen(pfad))
            {
                var verbindungMitZeit = this.FahrzeitenZuordnen(verbindungOhneZeit);
                
                if (verbindungMitZeit == null)
                {
                    continue;
                }

                var verbindungEingeschraenkt = this.EinschraenkenNachFahrzeit(verbindungMitZeit, startzeit);

                if (verbindungEingeschraenkt == null)
                {
                    continue;
                }

                this.OnVerbindung(verbindungEingeschraenkt);
            }
        }

        /// <summary>
        /// Erzeugt die Verbindungen aus den gegebenen Pfaden.
        /// </summary>
        /// <param name="pfad">
        /// The pfad.
        /// </param>
        /// <returns>
        /// The <see cref="Verbindung"/>.
        /// </returns>
        internal IEnumerable<Verbindung> Verbindungenerzeugen(Pfad pfad)
        {
            var linienName = pfad.Strecken.First().Linienname;
            var startHalteStelle = pfad.Starthaltestellenname;

            var abfahrtszeiten = this.fahrplanProvider.Abfahrtszeiten_bei_Haltestelle(linienName, startHalteStelle);

            foreach (var zeit in abfahrtszeiten)
            {
                var verbindung = new Verbindung { Pfad = pfad, Fahrtzeiten = new Fahrtzeit[pfad.Strecken.Length] };
                
                for (int i = 0; i < pfad.Strecken.Length; i++)
                {
                    verbindung.Fahrtzeiten[i] = new Fahrtzeit();
                }

                // Startzeit der Verbindung ist nur hier bekannt
                verbindung.Fahrtzeiten[0].Abfahrtszeit = zeit;
                yield return verbindung;
            }
        }

        /// <summary>
        /// Ordnet den Verbindungen die Fahrzeiten zu.
        /// </summary>
        /// <param name="verbindung">
        /// The verbindungen.
        /// </param>
        /// <returns>
        /// The <see cref="Verbindung"/>.
        /// </returns>
        internal Verbindung FahrzeitenZuordnen(Verbindung verbindung)
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

                if (!gueltigeVerbindung)
                {
                    return null;
                }
                
                // Frage den Fahrplan über die Dauer der Strecke
                var dauer = this.fahrplanProvider.Fahrtdauer_für_Strecke(linienName, startHalteStelle);

                // Ankunftszeit
                verbindung.Fahrtzeiten[i].Ankunftszeit = verbindung.Fahrtzeiten[i].Abfahrtszeit + dauer;

                // Merke für nächste Runde
                startHalteStelle = verbindung.Pfad.Strecken[i].Zielhaltestellenname;
                ankunftzeitLetzteStrecke = verbindung.Fahrtzeiten[i].Ankunftszeit;
            }

            return verbindung;
        }

        /// <summary>
        /// Schraenkt die gegebenen Verbindungen nach fahrzeit ein.
        /// </summary>
        /// <param name="verbindung">
        /// Die verbindungen.
        /// </param>
        /// <param name="startZeit">
        /// Die start zeit.
        /// </param>
        /// <returns>
        /// The <see cref="Verbindung"/>.
        /// </returns>
        internal Verbindung EinschraenkenNachFahrzeit(Verbindung verbindung, DateTime startZeit)
        {
            return verbindung.Fahrtzeiten[0].Abfahrtszeit >= startZeit ? verbindung : null;
        }
    }
}
