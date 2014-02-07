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
            target.OnVerbindung += AssertTest1;

            Strecke s = new Strecke();
            s.Linienname = "U1";
            s.Zielhaltestellenname = "H2";

            target.Verbindugen_zu_Pfad_bilden(null, Time(8, 0));
            Assert.AreEqual(1, this.results.Count);
            Assert.IsNull(this.results[0]);
        }

        [TestMethod]
        public void Test1()
        {
            var target = new VerbindungsErzeugung(new FahrplanProviderMock());
            this.results = new List<Verbindung>();

            target.OnVerbindung += AssertTest1;

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
            Assert.AreEqual(Time(9,0) + TimeSpan.FromMinutes(1), res.Fahrtzeiten[0].Ankunftszeit);
        }

        private void AssertTest1(Verbindung v)
        {
            this.results.Add(v);
        }


        public static DateTime Time(int hour, int minute)
        {
            return new DateTime(2014, 1, 1, hour, minute, 0);
        }

        private class FahrplanProviderMock : IFahrplanProvider
        {
            public DateTime[] Abfahrtszeiten_bei_Haltestelle(string linienname, string haltestellenname)
            {
                return new[] 
                {
                    Time(8, 00),
                    Time(8, 10),
                    Time(8, 20),
                    Time(8, 30),
                    Time(8, 40),
                    Time(8, 50),
                    Time(9, 00),
                };
            }

            public TimeSpan Fahrtdauer_für_Strecke(string linienname, string haltestellenname)
            {
                switch (haltestellenname)
                {
                    case "H1": return TimeSpan.FromMinutes(1);
                    case "H2": return TimeSpan.FromMinutes(2);
                    case "H3": return TimeSpan.FromMinutes(3);
                    case "H4": return TimeSpan.FromMinutes(4);
                    default: return TimeSpan.FromMinutes(10);
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
