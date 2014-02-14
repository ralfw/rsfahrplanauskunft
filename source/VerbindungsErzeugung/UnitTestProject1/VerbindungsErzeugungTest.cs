using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using rsfa.contracts;
using rsfa.contracts.daten;

namespace VerbindungsErzeugung
{
    [TestClass]
    public class VerbindungsErzeugungTest
    {
        private List<Verbindung> results;

        [TestMethod]
        public void ConstructorTest()
        {
            var target = new VerbindungsErzeugung(new FahrplanProviderMock());
            Assert.IsNotNull(target);
        }

        [TestMethod]
        public void TestWithNull()
        {
            var target = new VerbindungsErzeugung(new FahrplanProviderMock());
            this.results = new List<Verbindung>();
            target.OnVerbindung += MerkeVerbindungInListe;

            Strecke s = new Strecke();
            s.Linienname = "U1";
            s.Zielhaltestellenname = "H2";

            target.Verbindugen_zu_Pfad_bilden(null, Time(8, 0));
            Assert.AreEqual(1, this.results.Count);
            Assert.IsNull(this.results[0]);
        }

        [TestMethod]
        public void TestEinzelstrecke()
        {
            var target = new VerbindungsErzeugung(new FahrplanProviderMock());
            this.results = new List<Verbindung>();

            target.OnVerbindung += MerkeVerbindungInListe;

            Strecke s = new Strecke();
            s.Linienname = "U1";
            s.Zielhaltestellenname = "H2";

            Pfad pfad = new Pfad();
            pfad.Starthaltestellenname = "H1";
            pfad.Strecken = new Strecke[1];
            pfad.Strecken[0] = s;

            target.Verbindugen_zu_Pfad_bilden(pfad, Time(9, 0));

            Assert.AreEqual(1, this.results.Count);
            var res = this.results[0];
            Assert.AreEqual("H1", res.Pfad.Starthaltestellenname);
            Assert.AreEqual(1, res.Pfad.Strecken.Length);
            Assert.AreEqual("H2", res.Pfad.Strecken[0].Zielhaltestellenname);
            Assert.AreEqual(1, res.Fahrtzeiten.Length);
            Assert.AreEqual(Time(9, 0), res.Fahrtzeiten[0].Abfahrtszeit);
            Assert.AreEqual(Time(9, 2), res.Fahrtzeiten[0].Ankunftszeit);
        }

        [TestMethod]
        public void TestZweiStrecken()
        {
            var target = new VerbindungsErzeugung(new FahrplanProviderMock());
            this.results = new List<Verbindung>();

            target.OnVerbindung += MerkeVerbindungInListe;

            Pfad pfad = new Pfad();
            pfad.Starthaltestellenname = "H1";
            pfad.Strecken = new Strecke[2];

            Strecke s = new Strecke();
            s.Linienname = "U1";
            s.Zielhaltestellenname = "H2";
            pfad.Strecken[0] = s;

            s = new Strecke();
            s.Linienname = "U1";
            s.Zielhaltestellenname = "H3";
            pfad.Strecken[1] = s;

            target.Verbindugen_zu_Pfad_bilden(pfad, Time(9, 0));

            Assert.AreEqual(1, this.results.Count);
            var res = this.results[0];
            Assert.AreEqual("H1", res.Pfad.Starthaltestellenname);
            Assert.AreEqual(2, res.Pfad.Strecken.Length);
            Assert.AreEqual("H2", res.Pfad.Strecken[0].Zielhaltestellenname);
            Assert.AreEqual("H3", res.Pfad.Strecken[1].Zielhaltestellenname);
            Assert.AreEqual(2, res.Fahrtzeiten.Length);
            Assert.AreEqual(Time(9, 0), res.Fahrtzeiten[0].Abfahrtszeit);
            Assert.AreEqual(Time(9, 2), res.Fahrtzeiten[0].Ankunftszeit);
            Assert.AreEqual(Time(9, 2), res.Fahrtzeiten[1].Abfahrtszeit);
            Assert.AreEqual(Time(9, 3), res.Fahrtzeiten[1].Ankunftszeit);
        }

        [TestMethod]
        public void TestZweiStreckenMehrereVerbindungen()
        {
            var target = new VerbindungsErzeugung(new FahrplanProviderMock());
            this.results = new List<Verbindung>();

            target.OnVerbindung += MerkeVerbindungInListe;

            Pfad pfad = new Pfad();
            pfad.Starthaltestellenname = "H1";
            pfad.Strecken = new Strecke[2];

            Strecke s = new Strecke();
            s.Linienname = "U1";
            s.Zielhaltestellenname = "H2";
            pfad.Strecken[0] = s;

            s = new Strecke();
            s.Linienname = "U3";
            s.Zielhaltestellenname = "H3";
            pfad.Strecken[1] = s;

            target.Verbindugen_zu_Pfad_bilden(pfad, Time(8, 40));

            Assert.AreEqual(3, this.results.Count);
            Assert.AreEqual(Time(8, 48), this.results[0].Fahrtzeiten[1].Ankunftszeit);
            Assert.AreEqual(Time(8, 58), this.results[1].Fahrtzeiten[1].Ankunftszeit);
            Assert.AreEqual(Time(9, 08), this.results[2].Fahrtzeiten[1].Ankunftszeit);
        }

        [TestMethod]
        public void TestU1U2U3()
        {
            var target = new VerbindungsErzeugung(new FahrplanProviderMock());
            this.results = new List<Verbindung>();

            target.OnVerbindung += MerkeVerbindungInListe;

            Pfad pfad = new Pfad();
            pfad.Starthaltestellenname = "H1";
            pfad.Strecken = new Strecke[3];

            Strecke s = new Strecke();
            s.Linienname = "U1";
            s.Zielhaltestellenname = "H2";
            pfad.Strecken[0] = s;

            s = new Strecke();
            s.Linienname = "U2";
            s.Zielhaltestellenname = "H3";
            pfad.Strecken[1] = s;

            s = new Strecke();
            s.Linienname = "U3";
            s.Zielhaltestellenname = "H4";
            pfad.Strecken[2] = s;

            target.Verbindugen_zu_Pfad_bilden(pfad, Time(9, 0));

            Assert.AreEqual(1, this.results.Count);
            var res = this.results[0];
            Assert.AreEqual("H1", res.Pfad.Starthaltestellenname);
            Assert.AreEqual(3, res.Pfad.Strecken.Length);
            Assert.AreEqual("H2", res.Pfad.Strecken[0].Zielhaltestellenname);
            Assert.AreEqual("H3", res.Pfad.Strecken[1].Zielhaltestellenname);
            Assert.AreEqual("H4", res.Pfad.Strecken[2].Zielhaltestellenname);
            Assert.AreEqual(3, res.Fahrtzeiten.Length);
            Assert.AreEqual(Time(9, 0), res.Fahrtzeiten[0].Abfahrtszeit);
            Assert.AreEqual(Time(9, 2), res.Fahrtzeiten[0].Ankunftszeit);
            Assert.AreEqual(Time(9, 4), res.Fahrtzeiten[1].Abfahrtszeit);
            Assert.AreEqual(Time(9, 5), res.Fahrtzeiten[1].Ankunftszeit);
            Assert.AreEqual(Time(9, 8), res.Fahrtzeiten[2].Abfahrtszeit);
            Assert.AreEqual(Time(9, 12), res.Fahrtzeiten[2].Ankunftszeit);
        }

        [TestMethod]
        public void TestStartzeitZuSpät()
        {
            var target = new VerbindungsErzeugung(new FahrplanProviderMock());
            this.results = new List<Verbindung>();

            target.OnVerbindung += MerkeVerbindungInListe;

            Strecke s = new Strecke();
            s.Linienname = "U1";
            s.Zielhaltestellenname = "H2";

            Pfad pfad = new Pfad();
            pfad.Starthaltestellenname = "H1";
            pfad.Strecken = new Strecke[1];
            pfad.Strecken[0] = s;

            target.Verbindugen_zu_Pfad_bilden(pfad, Time(10, 0));

            Assert.AreEqual(0, this.results.Count);
        }

        [TestMethod]
        public void TestUmsteigezeitZuSpät()
        {
            var target = new VerbindungsErzeugung(new FahrplanProviderMock());
            this.results = new List<Verbindung>();

            target.OnVerbindung += MerkeVerbindungInListe;

            Pfad pfad = new Pfad();
            pfad.Starthaltestellenname = "H1";
            pfad.Strecken = new Strecke[2];

            Strecke s = new Strecke();
            s.Linienname = "U2";
            s.Zielhaltestellenname = "H2";
            pfad.Strecken[0] = s;

            s = new Strecke();
            s.Linienname = "U1";
            s.Zielhaltestellenname = "H3";
            pfad.Strecken[1] = s;

            target.Verbindugen_zu_Pfad_bilden(pfad, Time(9, 07));

            Assert.AreEqual(0, this.results.Count);
        }

        /// <summary>
        /// Action für die Resultate. Die Verbindung wird in die Resultatliste eingetragen
        /// </summary>
        /// <param name="v">Die Verbindung</param>
        private void MerkeVerbindungInListe(Verbindung v)
        {
            this.results.Add(v);
        }

        /// <summary>
        /// Hilfsfunktion für die einfache Erzeugung von DateTime
        /// </summary>
        /// <param name="hour">Die Stunde</param>
        /// <param name="minute">Die Minute</param>
        /// <returns>DateTime, Stunde und Minute für das aktuelle Datum</returns>
        public static DateTime Time(int hour, int minute)
        {
            DateTime datum = new DateTime(2014, 1, 1, 0, 0, 0);
            datum += TimeSpan.FromHours(hour);
            datum += TimeSpan.FromMinutes(minute);
            return datum;
        }

        /// <summary>
        /// Mock für den Fahrplanprovider
        /// </summary>
        private class FahrplanProviderMock : IFahrplanProvider
        {
            public DateTime[] Abfahrtszeiten_bei_Haltestelle(string linienname, string haltestellenname)
            {
                // Abfahrtszeiten (Minuten) an den Haltestellen
                // Linie        H1  H2  H3  H4
                // U1           00  02  03  07
                // U2           02  04  05  09
                // U3           05  07  08  12
                // Fahrtdauer     02  01  04
                // Die Abfahrtszeiten beginnen bei 08:00 und wiederholen sich alle 10 Minuten bis 09:00

                int minute = 0;
                switch (linienname)
                {
                    case "U1": minute += 0; break;
                    case "U2": minute += 2; break;
                    case "U3": minute += 5; break;
                }

                switch (haltestellenname)
                {
                    case "H1": minute += 0; break;
                    case "H2": minute += 2; break;
                    case "H3": minute += 3; break;
                    case "H4": minute += 7; break;
                }

                return new[] 
                {
                    Time(8, minute),
                    Time(8, minute + 10),
                    Time(8, minute + 20),
                    Time(8, minute + 30),
                    Time(8, minute + 40),
                    Time(8, minute + 50),
                    Time(8, minute + 60),
                };
            }

            public TimeSpan Fahrtdauer_für_Strecke(string linienname, string haltestellenname)
            {
                switch (haltestellenname)
                {
                    case "H1": return TimeSpan.FromMinutes(2);
                    case "H2": return TimeSpan.FromMinutes(1);
                    case "H3": return TimeSpan.FromMinutes(4);
                    case "H4": return TimeSpan.FromMinutes(0);
                    default: return TimeSpan.FromMinutes(0);
                }
            }

            public string[] Haltestellen_für_Linie(string linienname)
            {
                return new[] { "H1", "H2", "H3", "H4" };
            }

            public string[] Liniennamen
            {
                get { return new[] { "U1", "U2", "U3" }; }
            }
        }
    }
}
