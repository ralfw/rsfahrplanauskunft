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
    }
}
