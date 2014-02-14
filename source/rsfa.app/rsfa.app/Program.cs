using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rsfa.verbindungsbewertung;

namespace rsfa.app
{
    using System.IO;

    using rsfa.contracts;
    using rsfa.contracts.daten;

    class Program
    {
        static void Main(string[] args)
        {
            var kommando = new Kommandozeilenportal(args);
            var konsole = new Konsolenprovider();

            var fpprov = new FahrplanProvider.FahrplanProvider();
            var netz = new Netzplanberechnung.Netzplanberechnung(fpprov);
            var pfade = new pfadbestimmung.Pfadbestimmung();
            var verb = new VerbindungsErzeugung.VerbindungsErzeugung(fpprov);
            var bewert = new Verbindungsbewertung();

            pfade.OnPfad += pfad => verb.Verbindugen_zu_Pfad_bilden(pfad, kommando.Startzeit);
            verb.OnVerbindung += bewert.Verbindungen_bewerten;
            bewert.OnVerbindungenKomplett += konsole.Verbindungen_anzeigen;

            var netzplan = netz.Netzplan_berechnen();
            SchreibeDotFile(netzplan);
            pfade.Alle_Pfade_bestimmen(netzplan, kommando.Starthaltestellenname, kommando.Zielhaltestellenname);
        }

        private static void SchreibeDotFile(Netzplan netzplan)
        {
            File.WriteAllLines(@"C:\tmp\dot1.txt", new[] { GenerateDot(netzplan) });
        }

        private static String GenerateDot(Netzplan netzplan)
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
