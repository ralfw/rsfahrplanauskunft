using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using rsfa.pfadbestimmung;
using rsfa.contracts;
using rsfa.contracts.daten;


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
   }
}
