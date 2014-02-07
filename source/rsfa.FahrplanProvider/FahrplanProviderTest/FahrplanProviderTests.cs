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
            var zeiten = fahrplanProvider.Abfahrtszeiten_bei_Haltestelle("Transrapid Linie Stoiber", "Lichtgestalthausen");
            Assert.IsNotNull(zeiten);
            Assert.IsTrue(zeiten.Length == 3, "Wrong length");
        }

        [TestMethod]
        public void Fahrtdauer_für_StreckeTest()
        {
            var fahrplanProvider = new FahrplanProvider();
            TimeSpan fahrtdauer = fahrplanProvider.Fahrtdauer_für_Strecke("Transrapid Linie Stoiber", "Lichtgestalthausen");
            Assert.IsNotNull(fahrtdauer);
        }

        [TestMethod]
        public void Haltestellen_für_LinieTest()
        {
            var fahrplanProvider = new FahrplanProvider();
            string[] haltestellen = fahrplanProvider.Haltestellen_für_Linie("Transrapid Linie Stoiber");
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
