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
      private Netzplan EinfachsterNetzplan = new Netzplan()
      {
         Haltestellen = new[]
         {
            new Haltestelle
            {
               Name = "Start",
               Strecken = new[]
               {
                  new Strecke { Linienname = "U1 Ost", Zielhaltestellenname = "Ziel" },
               },
            },
            new Haltestelle
            {
               Name = "Ziel",
               Strecken = new[]
               {
                  new Strecke { Linienname = "U1 West", Zielhaltestellenname = "Start" },
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
               Name = "Start",
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
                  new Strecke { Linienname = "U1 Ost", Zielhaltestellenname = "Ziel" },
               },
            },
            new Haltestelle
            {
               Name = "Ziel",
               Strecken = new[]
               {
                  new Strecke { Linienname = "U1 West", Zielhaltestellenname = "Start" },
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
               Name = "Start",
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
                  new Strecke { Linienname = "U1 Ost", Zielhaltestellenname = "Ziel" },
                  new Strecke { Linienname = "U2", Zielhaltestellenname = "Ziel" },
               },
            },
            new Haltestelle
            {
               Name = "Ziel",
               Strecken = new[]
               {
                  new Strecke { Linienname = "U1 West", Zielhaltestellenname = "Start" },
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
               Name = "Start",
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
                  new Strecke { Linienname = "U1 Ost", Zielhaltestellenname = "Ziel" },
                  new Strecke { Linienname = "Bus", Zielhaltestellenname = "Ziel" },
                  new Strecke { Linienname = "U1 West", Zielhaltestellenname = "Start" },
               },
            },
            new Haltestelle
            {
               Name = "Ziel",
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
         var target = new Pfadbestimmung();
         var start = "Start";
         var ziel = "Ziel";

         Int32 gefundenePfade = 0;
         target.OnPfad += (pfad) =>
         {
            gefundenePfade++;
            Assert.AreEqual(ziel, pfad.Strecken.Last().Zielhaltestellenname, "Ziel stimmt nicht");
            Assert.AreEqual(start, pfad.Starthaltestellenname, "Start stimmt nicht");
         };

         target.Alle_Pfade_bestimmen(this.EinfachsterNetzplan, start, ziel);
         Assert.AreEqual(1, gefundenePfade, "Keine pfade gefunden");
      }

      [TestMethod]
      public void KomplexerNetzplanMitEinemPfad()
      {
         var target = new Pfadbestimmung();
         var start = "Start";
         var ziel = "Ziel";

         Int32 gefundenePfade = 0;
         target.OnPfad += (pfad) =>
         {
            gefundenePfade++;
            Assert.AreEqual(ziel, pfad.Strecken.Last().Zielhaltestellenname, "Ziel stimmt nicht");
            Assert.AreEqual(start, pfad.Starthaltestellenname, "Start stimmt nicht");
         };

         target.Alle_Pfade_bestimmen(this.KomplexerNetzplan, start, ziel);
         Assert.AreEqual(1, gefundenePfade, "Keine pfade gefunden");
      }

      [TestMethod]
      public void KomplexerNetzplanMitZwoaPfaden()
      {
         var target = new Pfadbestimmung();
         var start = "Start";
         var ziel = "Ziel";

         Int32 gefundenePfade = 0;
         target.OnPfad += (pfad) =>
         {
            gefundenePfade++;
            Assert.AreEqual(ziel, pfad.Strecken.Last().Zielhaltestellenname, "Ziel stimmt nicht");
            Assert.AreEqual(start, pfad.Starthaltestellenname, "Start stimmt nicht");
         };

         target.Alle_Pfade_bestimmen(this.KomplexerNetzplan2, start, ziel);
         Assert.AreEqual(2, gefundenePfade, "Anzahl Pfade passt nicht");
      }

      [TestMethod]
      public void KomplexerNetzplanMitZwoaPfaden3()
      {
         var target = new Pfadbestimmung();
         var start = "Start";
         var ziel = "Ziel";

         Int32 gefundenePfade = 0;
         target.OnPfad += (pfad) =>
            {
               gefundenePfade++;
               Assert.AreEqual(ziel, pfad.Strecken.Last().Zielhaltestellenname, "Ziel stimmt nicht");
               Assert.AreEqual(start, pfad.Starthaltestellenname, "Start stimmt nicht");
            };

         target.Alle_Pfade_bestimmen(this.KomplexerNetzplan3, start, ziel);
         Assert.AreEqual(2, gefundenePfade, "Anzahl Pfade passt nicht");
      }

      [TestMethod]
      public void ZeichneNetzpläne()
      {
         // cut and paste strings to http://graphviz-dev.appspot.com/
         var dot1 = this.GenerateDot(this.EinfachsterNetzplan);
         var dot3 = this.GenerateDot(this.KomplexerNetzplan3);
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
   }
}
