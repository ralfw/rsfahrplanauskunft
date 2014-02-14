using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using rsfa.pfadbestimmung;
using rsfa.contracts;
using rsfa.contracts.daten;
using System.Text;


namespace PfadbestimmungTests
{
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
        public void NetzplanSehrLangemPfad()
        {
            var netzplan = this.GenerateLinearNetzplan(1000);
            var expectedNumberOfPaths = 1;

            this.AssertPathSearchOutcome(netzplan, expectedNumberOfPaths);
        }

        private void AssertPathSearchOutcome(Netzplan netzplan, int expectedNumberOfPaths)
        {
            this.AssertPathSearchOutcome(netzplan, StartHaltestellenname, ZielHaltestellenname, expectedNumberOfPaths);
        }

        private void AssertPathSearchOutcome(Netzplan netzplan, string start, string ziel, int expectedNumberOfPaths)
        {
            var target = new Pfadbestimmung();
            Int32 gefundenePfade = 0;
            target.OnPfad += (pfad) =>
            {
                gefundenePfade++;
                Assert.AreEqual(ziel, pfad.Strecken.Last().Zielhaltestellenname, "Ziel stimmt nicht");
                Assert.AreEqual(start, pfad.Starthaltestellenname, "Start stimmt nicht");
            };

            target.Alle_Pfade_bestimmen(netzplan, start, ziel);
            Assert.AreEqual(expectedNumberOfPaths, gefundenePfade, "Anzahl Pfade passt nicht");
        }

        [TestMethod]
        public void ZeichneNetzpläne()
        {
            // cut and paste strings to http://graphviz-dev.appspot.com/
            var dot1 = this.GenerateDot(this.EinfachsterNetzplan);
            var dot3 = this.GenerateDot(this.KomplexerNetzplan3);
            var dotBla = this.GenerateDot(this.GenerateLinearNetzplan(10));
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
