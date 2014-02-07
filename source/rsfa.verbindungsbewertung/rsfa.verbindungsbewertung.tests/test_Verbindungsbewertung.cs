using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using rsfa.contracts.daten;

namespace rsfa.verbindungsbewertung.tests
{
    [TestFixture]
    public class test_Verbindungsbewertung
    {
        [Test]
        public void Test()
        {
            var sut = new Verbindungsbewertung();

            Verbindung[] results = null;
            sut.OnVerbindungenKomplett += verbindungen => results = verbindungen.ToArray();

            var testdaten = this.Testdaten;
            sut.Verbindungen_bewerten(testdaten[0]);
            Assert.IsNull(results);
            sut.Verbindungen_bewerten(testdaten[1]);
            Assert.IsNull(results);
            sut.Verbindungen_bewerten(null);
            Assert.IsNotNull(results);

            Assert.AreEqual(1, results[0].Pfad.Strecken.Length);
            Assert.AreEqual(2, results[1].Pfad.Strecken.Length);
        }


        Verbindung[] Testdaten
        {
            get
            {
                var verbindungen = new List<Verbindung>();

                var v = new Verbindung();
                var p = new Pfad
                {
                    Starthaltestellenname = "A",
                    Strecken = new[]
                            {
                                new Strecke {Linienname = "L2", Zielhaltestellenname = "X"},
                                new Strecke {Linienname = "L3", Zielhaltestellenname = "C"}
                            }
                };
                v.Pfad = p;
                v.Fahrtzeiten = new[]
                {
                    new Fahrtzeit
                        {
                            Abfahrtszeit = new DateTime(2000, 1, 1, 8, 1, 0),
                            Ankunftszeit = new DateTime(2000, 1, 1, 8, 3, 0)
                        },
                    new Fahrtzeit
                        {
                            Abfahrtszeit = new DateTime(2000, 1, 1, 8, 4, 0),
                            Ankunftszeit = new DateTime(2000, 1, 1, 8, 6, 0)
                        }
                };
                verbindungen.Add(v);

                v = new Verbindung();
                p = new Pfad
                    {
                        Starthaltestellenname = "A",
                        Strecken = new[]
                            {
                                new Strecke {Linienname = "L1", Zielhaltestellenname = "B"}
                            }
                    };
                v.Pfad = p;
                v.Fahrtzeiten = new[]
                {
                    new Fahrtzeit
                        {
                            Abfahrtszeit = new DateTime(2000, 1, 1, 8, 0, 0),
                            Ankunftszeit = new DateTime(2000, 1, 1, 8, 7, 0)
                        }
                };
                verbindungen.Add(v);

                return verbindungen.ToArray();
            }
        }
    }
}
