using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using rsfa.contracts;

namespace VerbindungsErzeugung
{
    [TestClass]
    public class VerbindungsErzeugungTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var target = new VerbindungsErzeugung(new FahrplanProviderStub());
            Assert.IsNotNull(target);
        }

        private class FahrplanProviderStub : IFahrplanProvider
        {
            public DateTime[] Abfahrtszeiten_bei_Haltestelle(string linienname, string haltestellenname)
            {
                throw new NotImplementedException();
            }

            public TimeSpan Fahrtdauer_für_Strecke(string linienname, string haltestellenname)
            {
                throw new NotImplementedException();
            }

            public string[] Haltestellen_für_Linie(string linienname)
            {
                throw new NotImplementedException();
            }

            public string[] Liniennamen
            {
                get { return new[] { "U1", "U2", "U3" }; }
            }
        }
    }
}
