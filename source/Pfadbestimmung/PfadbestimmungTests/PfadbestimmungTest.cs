using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using rsfa.pfadbestimmung;
using rsfa.contracts;
using rsfa.contracts.daten;
using System.Text;


namespace PfadbestimmungTests
{
    using System.Diagnostics;

    [TestClass]
    public class PfadbestimmungTest
    {
        private const String StartHaltestellenname = "Start";
        private const String ZielHaltestellenname = "Ziel";

        private Netzplan EinfachsterNetzplan = new Netzplan()
        {
            Haltestellen = new[]
         {
            new Haltestelle
            {
               Name = StartHaltestellenname,
               Strecken = new[]
               {
                  new Strecke { Linienname = "U1 Ost", Zielhaltestellenname = ZielHaltestellenname },
               },
            },
            new Haltestelle
            {
               Name = ZielHaltestellenname,
               Strecken = new[]
               {
                  new Strecke { Linienname = "U1 West", Zielhaltestellenname = StartHaltestellenname },
               },
            },
         }
        };

        private Netzplan KomplexerNetzplan = new Netzplan()
        {
            Haltestellen = new[]
         {
            new Haltestelle
            {
               Name = StartHaltestellenname,
               Strecken = new[]
               {
                  new Strecke { Linienname = "U1 Ost", Zielhaltestellenname = "Mitte" },
               },
            },
            new Haltestelle
            {
               Name = "Mitte",
               Strecken = new[]
               {
                  new Strecke { Linienname = "U1 Ost", Zielhaltestellenname = ZielHaltestellenname },
               },
            },
            new Haltestelle
            {
               Name = ZielHaltestellenname,
               Strecken = new[]
               {
                  new Strecke { Linienname = "U1 West", Zielhaltestellenname = StartHaltestellenname },
               },
            },
         }
        };

        private Netzplan KomplexerNetzplan2 = new Netzplan()
        {
            Haltestellen = new[]
         {
            new Haltestelle
            {
               Name = StartHaltestellenname,
               Strecken = new[]
               {
                  new Strecke { Linienname = "U1 Ost", Zielhaltestellenname = "Mitte" },
               },
            },
            new Haltestelle
            {
               Name = "Mitte",
               Strecken = new[]
               {
                  new Strecke { Linienname = "U1 Ost", Zielhaltestellenname = ZielHaltestellenname },
                  new Strecke { Linienname = "U2", Zielhaltestellenname = ZielHaltestellenname },
               },
            },
            new Haltestelle
            {
               Name = ZielHaltestellenname,
               Strecken = new[]
               {
                  new Strecke { Linienname = "U1 West", Zielhaltestellenname = StartHaltestellenname },
               },
            },
         }
        };

        private Netzplan KomplexerNetzplan3 = new Netzplan()
        {
            Haltestellen = new[]
         {
            new Haltestelle
            {
               Name = StartHaltestellenname,
               Strecken = new[]
               {
                  new Strecke { Linienname = "U1 Ost", Zielhaltestellenname = "Mitte" },
               },
            },
            new Haltestelle
            {
               Name = "Mitte",
               Strecken = new[]
               {
                  new Strecke { Linienname = "U1 Ost", Zielhaltestellenname = ZielHaltestellenname },
                  new Strecke { Linienname = "Bus", Zielhaltestellenname = ZielHaltestellenname },
                  new Strecke { Linienname = "U1 West", Zielhaltestellenname = StartHaltestellenname },
               },
            },
            new Haltestelle
            {
               Name = ZielHaltestellenname,
               Strecken = new[]
               {
                  new Strecke { Linienname = "U1 West", Zielhaltestellenname = "Mitte" },
               },
            },
         }
        };

        private Netzplan KomplexerNetzplanMitSchleifeVorZiel = new Netzplan()
        {
            Haltestellen = new[]
         {
            new Haltestelle
            {
               Name = StartHaltestellenname,
               Strecken = new[]
               {
                  new Strecke { Linienname = "U1 Ost", Zielhaltestellenname = "Mitte1" },
               },
            },
            new Haltestelle
            {
               Name = "Mitte1",
               Strecken = new[]
               {
                  new Strecke { Linienname = "U1 Ost", Zielhaltestellenname = "Mitte2" },
               },
            },
            new Haltestelle
            {
               Name = "Mitte2",
               Strecken = new[]
               {
                  new Strecke { Linienname = "U1 Ost", Zielhaltestellenname = ZielHaltestellenname },
                  new Strecke { Linienname = "U1 West", Zielhaltestellenname = "Mitte1" },
               },
            },
            new Haltestelle
            {
               Name = ZielHaltestellenname,
               Strecken = new[]
               {
                  new Strecke { Linienname = "U1 West", Zielhaltestellenname = "Mitte2" },
               },
            },
         }
        };

        [TestMethod]
        public void EinfacherNetzplanMitEinemPfad()
        {
            var netzplan = this.EinfachsterNetzplan;
            var expectedNumberOfPaths = 1;

            this.AssertPathSearchOutcome(netzplan, expectedNumberOfPaths);
        }

        [TestMethod]
        public void KomplexerNetzplanMitEinemPfad()
        {
            var netzplan = this.KomplexerNetzplan;
            var expectedNumberOfPaths = 1;

            this.AssertPathSearchOutcome(netzplan, expectedNumberOfPaths);
        }

        [TestMethod]
        public void KomplexerNetzplanMitZwoaPfaden()
        {
            var netzplan = this.KomplexerNetzplan2;
            var expectedNumberOfPaths = 2;

            this.AssertPathSearchOutcome(netzplan, expectedNumberOfPaths);
        }

        [TestMethod]
        public void KomplexerNetzplanMitZwoaPfaden3()
        {
            var netzplan = this.KomplexerNetzplan3;
            var expectedNumberOfPaths = 2;

            this.AssertPathSearchOutcome(netzplan, expectedNumberOfPaths);
        }

        [TestMethod]
        public void KomplexerNetzplanMitSchleifeVorZielUndEinemPfad()
        {
            var netzplan = this.KomplexerNetzplanMitSchleifeVorZiel;
            var expectedNumberOfPaths = 1;

            this.AssertPathSearchOutcome(netzplan, expectedNumberOfPaths);
        }

        [TestMethod]
        public void NetzplanSehrLangemPfad()
        {
            var netzplan = this.GenerateLinearNetzplan(1000);
            var expectedNumberOfPaths = 1;

            this.AssertPathSearchOutcome(netzplan, expectedNumberOfPaths);
        }

        // base: 12 Pfade gefunden in 00:00:00.0000775 (154.838709677419 pfade/ms)
        // immutable: 12 Pfade gefunden in 00:00:00.0000652 (184.049079754601 pfade/ms)
        // manual reverse implementation: 12 Pfade gefunden in 00:00:00.0000585 (205.128205128205 pfade/ms)
        // nicht-rekursiv mit ConcurrentStack: 12 Pfade gefunden in 00:00:00.0000612 (196.078431372549 pfade/ms)
        [TestMethod]
        public void NetzplanMatrixKlein()
        {
            var generator = new MatrixNetzplanGenerator(3, 3);
            var netzplan = generator.BerechneNetzplan();
            var expectedNumberOfPaths = 12;
            this.AssertPathSearchOutcome(netzplan, "A0", "C2", expectedNumberOfPaths);
        }

        // base: 6762 Pfade gefunden in 00:00:00.1020290 (66.2752746768076 pfade/ms)
        // immutable: 6762 Pfade gefunden in 00:00:00.0904301 (74.7759871989525 pfade/ms)
        // manual reverse implementation: 6762 Pfade gefunden in 00:00:00.0808063 (83.6815941331307 pfade/ms)
        // nicht-rekursiv mit ConcurrentStack: 6762 Pfade gefunden in 00:00:00.0840087 (80.4916633634374 pfade/ms)
        [TestMethod]
        public void NetzplanMatrixMittel()
        {
            var generator = new MatrixNetzplanGenerator(5, 5);
            var netzplan = generator.BerechneNetzplan();
            var expectedNumberOfPaths = 6762;
            this.AssertPathSearchOutcome(netzplan, "A0", "C2", expectedNumberOfPaths);
        }

        // base: 910480 Pfade gefunden in 00:00:24.8572389 (36.6283642227054 pfade/ms)
        // immutable stack: 910480 Pfade gefunden in 00:00:21.3353373 (42.6747413081676 pfade/ms)
        // manual reverse implementation: 910480 Pfade gefunden in 00:00:19.6791892 (46.266133769373 pfade/ms)
        // nicht-rekursiv mit ConcurrentStack: 910480 Pfade gefunden in 00:00:20.1880267 (45.0999997934419 pfade/ms)
        // nicht-rekursiv mit ConcurrentBag: 910480 Pfade gefunden in 00:00:23 (38 pfade/ms)
        [TestMethod]
        public void NetzplanMatrixGross()
        {
            var generator = new MatrixNetzplanGenerator(6, 6);
            var netzplan = generator.BerechneNetzplan();
            var expectedNumberOfPaths = 910480;
            this.AssertPathSearchOutcome(netzplan, "A0", "C2", expectedNumberOfPaths);
        }

        [TestMethod]
        public void RandomNetzplan()
        {
            var generator = new RandomNetzplanGenerator(66);
            var netzplan = generator.Netzplan_berechnen();
            Assert.IsNotNull(netzplan);
            Assert.IsNotNull(netzplan.Haltestellen);

            foreach (Haltestelle halteStelle in netzplan.Haltestellen)
            {
                Assert.IsNotNull(halteStelle.Strecken);
                Assert.IsFalse(String.IsNullOrEmpty(halteStelle.Name));
            }
        }

        [TestMethod]
        public void ZeichneNetzplaene()
        {
            // cut and paste strings to http://graphviz-dev.appspot.com/
            var dot1 = this.GenerateDot(this.EinfachsterNetzplan);
            var dot3 = this.GenerateDot(this.KomplexerNetzplan3);
            var dotBla = this.GenerateDot(this.GenerateLinearNetzplan(10));

            var randomPlan = this.GenerateDot(new RandomNetzplanGenerator(42).Netzplan_berechnen());
        }

        private void AssertPathSearchOutcome(Netzplan netzplan, int expectedNumberOfPaths)
        {
            this.AssertPathSearchOutcome(netzplan, StartHaltestellenname, ZielHaltestellenname, expectedNumberOfPaths);
        }

        private void AssertPathSearchOutcome(Netzplan netzplan, string start, string ziel, int expectedNumberOfPaths)
        {
            var sw = new Stopwatch();
            var target = new Pfadbestimmung();
            Int32 gefundenePfade = 0;
            target.OnPfad += (pfad) =>
            {
                if (pfad != null)
                {
                    gefundenePfade++;
                    Assert.AreEqual(ziel, pfad.Strecken.Last().Zielhaltestellenname, "Ziel stimmt nicht");
                    Assert.AreEqual(start, pfad.Starthaltestellenname, "Start stimmt nicht");
                }
            };

            sw.Start();
            target.Alle_Pfade_bestimmen(netzplan, start, ziel);
            var elapsed = sw.Elapsed;
            Console.WriteLine("{0} Pfade gefunden in {1} ({2} pfade/ms)", gefundenePfade, elapsed, gefundenePfade / elapsed.TotalMilliseconds);
            if (expectedNumberOfPaths >= 0)
            {
                Assert.AreEqual(expectedNumberOfPaths, gefundenePfade, "Anzahl Pfade passt nicht");
            }
        }

        [TestMethod]
        public void MatrixNetzplanTest1()
        {
            var generator = new MatrixNetzplanGenerator(3, 3);
            var netzplan = generator.BerechneNetzplan();
            var dot = this.GenerateDot(netzplan);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void MatrixNetzplanTest2()
        {
            var generator = new MatrixNetzplanGenerator(5, 5);
            var netzplan = generator.BerechneNetzplan();
            var dot = this.GenerateDot(netzplan);
            Assert.IsTrue(true);
        }

        private String GenerateDot(Netzplan netzplan)
        {
            var sb = new StringBuilder()
               .AppendLine("digraph Netzplan {");

            foreach (var haltestelle in netzplan.Haltestellen)
            {
                foreach (var strecke in haltestelle.Strecken)
                {
                    sb.AppendFormat("  \"{0}\" -> \"{1}\" [label=\"{2}\"];", haltestelle.Name, strecke.Zielhaltestellenname, strecke.Linienname);
                    sb.AppendLine();
                }
            }

            sb.AppendLine("}");
            return sb.ToString();
        }

        private Netzplan GenerateLinearNetzplan(Int32 numberOfHaltestellen)
        {
            const string linienName = "U1";
            var netzplan = new Netzplan()
            {
                Haltestellen = new Haltestelle[numberOfHaltestellen]
            };
            netzplan.Haltestellen[0] = new Haltestelle { Name = StartHaltestellenname };

            // loop over all intermideate terminals
            for (int i = 1; i < numberOfHaltestellen; i++)
            {
                var source = netzplan.Haltestellen[i - 1];
                var targetName = String.Format("H{0}", i);
                if (i == numberOfHaltestellen - 1)
                {
                    targetName = ZielHaltestellenname;
                }

                source.Strecken = new[]
              {
                  new Strecke { Linienname = linienName, Zielhaltestellenname = targetName }
              };
                netzplan.Haltestellen[i] = new Haltestelle { Name = targetName, Strecken = new Strecke[0] };
            }

            return netzplan;
        }
    }
}
