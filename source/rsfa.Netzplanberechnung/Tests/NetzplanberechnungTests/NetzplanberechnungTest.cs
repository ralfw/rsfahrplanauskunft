using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetzplanberechnungTests
{
    using rsfa.Netzplanberechnung;
    using rsfa.contracts.daten;

    [TestClass]
    public class NetzplanberechnungTest
    {
        [TestMethod]
        public void Netzplan_berechnenCtorTest()
        {
            TestFahrplanprovider testProvider = new TestFahrplanprovider();
            Assert.IsNotNull(testProvider);
            Netzplanberechnung berechnung = new Netzplanberechnung(testProvider);
            Assert.IsNotNull(berechnung);
        }

        [TestMethod]
        public void Netzplan_berechnenTest()
        {
            var plan = instantiateNetzplanBerechner();
            Assert.IsNotNull(plan);

            var halteMarien = plan.Haltestellen.Single(hs => hs.Name.Contains("Marienplatz"));
            Assert.IsTrue(halteMarien.Strecken.Length == 4);
            Assert.IsTrue(halteMarien.Strecken.Count(s => s.Linienname == "S2" && s.Zielhaltestellenname == "Isartor") == 1);
            Assert.IsTrue(halteMarien.Strecken.Count(s => s.Linienname == "S1OOst" && s.Zielhaltestellenname == "Isartor") == 1);
            Assert.IsTrue(halteMarien.Strecken.Count(s => s.Linienname == "S1OWest" && s.Zielhaltestellenname == "Stachus") == 1);
            Assert.IsTrue(halteMarien.Strecken.Count(s => s.Linienname == "S1OWest" && s.Zielhaltestellenname == "Isartor") == 0);
            Assert.IsTrue(halteMarien.Strecken.Count(s => s.Linienname == "U3" && s.Zielhaltestellenname == "Sendlinger") == 0);
            Assert.IsTrue(halteMarien.Strecken.Count(s => s.Linienname == "U3" && s.Zielhaltestellenname == "Odeons") == 1);
        }

        [TestMethod]
        public void Netzplan_berechnenSendlingerTest()
        {
            var plan = instantiateNetzplanBerechner();
            Assert.IsNotNull(plan);

            var halteMarien = plan.Haltestellen.Single(hs => hs.Name.Contains("Sendlinger"));
            Assert.IsTrue(halteMarien.Strecken.Length == 1);
            Assert.IsTrue(halteMarien.Strecken.Count(s => s.Linienname == "U3" && s.Zielhaltestellenname == "Marienplatz") == 1);
        }

        [TestMethod]
        public void Netzplan_berechnenUnbekannteHalteStelleTest()
        {
            var plan = instantiateNetzplanBerechner();
            Assert.IsFalse(plan.Haltestellen.Any(hs => hs.Name.Contains("jdsfk")));
        }


        private static Netzplan instantiateNetzplanBerechner()
        {
            TestFahrplanprovider testProvider = new TestFahrplanprovider();

            Netzplanberechnung berechnung = new Netzplanberechnung(testProvider);
            var plan = berechnung.Netzplan_berechnen();
            return plan;
        }
    }
}
