using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace rsfa.FahrplanProvider
{
    [TestClass]
    public class FahrplanProviderTests
    {
        [TestMethod]
        public void Abfahrtszeiten_bei_HaltestelleTest()
        {
            var fahrplanProvider = new FahrplanProvider();
            var zeiten = fahrplanProvider.Abfahrtszeiten_bei_Haltestelle("Transrapid Linie Stoiber Nord", "Lichtgestalthausen");
            Assert.IsNotNull(zeiten);
            Assert.IsTrue(zeiten.Length == 3, "Wrong length");
        }

        [TestMethod]
        public void Abfahrtszeiten_bei_HaltestelleConsistencyTest()
        {
            var fahrplanProvider = new FahrplanProvider();
            foreach (var linie in fahrplanProvider.Liniennamen)
            {
                foreach (var haltestelle in fahrplanProvider.Haltestellen_für_Linie(linie))
                {
                    var abfahrtszeiten = fahrplanProvider.Abfahrtszeiten_bei_Haltestelle(linie, haltestelle);
                    Assert.IsTrue(abfahrtszeiten.Length > 0,
                        String.Format("Keine Abfahrszeit für '{0}' an Haltestelle '{1}'", linie, haltestelle));
                }
            }
        }

        [TestMethod]
        public void Fahrtdauer_für_StreckeTest()
        {
            var fahrplanProvider = new FahrplanProvider();
            TimeSpan fahrtdauer = fahrplanProvider.Fahrtdauer_für_Strecke("Transrapid Linie Stoiber Nord", "Lichtgestalthausen");
            Assert.IsNotNull(fahrtdauer);
        }

        [TestMethod]
        public void Haltestellen_für_LinieTest()
        {
            var fahrplanProvider = new FahrplanProvider();
            string[] haltestellen = fahrplanProvider.Haltestellen_für_Linie("Transrapid Linie Stoiber Nord");
            Assert.IsNotNull(haltestellen);
        }

        [TestMethod]
        public void LiniennamenTest()
        {
            var fahrplanProvider = new FahrplanProvider();
            string[] linienNamen = fahrplanProvider.Liniennamen;
            Assert.IsNotNull(linienNamen);
        }
    }
}
