namespace rsfa.Netzplanberechnung
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using rsfa.contracts.daten;

    public class NetzplanVisualisierer
    {
        public void SchreibeDotFile(Netzplan netzplan, string dateiName)
        {
            File.WriteAllLines(dateiName, new[] { this.GenerateDot(netzplan) });
        }

        public void SchreibeCSharpFile(Netzplan netzplan, string dateiName)
        {
            File.WriteAllLines(dateiName, new[] { this.GenerateCSharp(netzplan) });
        }

        // shamelessly stolen from Pfadbestimmung
        // cut and paste strings to http://graphviz-dev.appspot.com/
        private String GenerateDot(Netzplan netzplan)
        {
            var sb = new StringBuilder().AppendLine("digraph Netzplan {");

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

        private String GenerateCSharp(Netzplan netzplan)
        {
            var sb = new StringBuilder().AppendFormat(netzplanformat, this.GenerateCSharpHalteStellen(netzplan));
            return sb.ToString();
        }

        private string GenerateCSharpHalteStellen(Netzplan netzplan)
        {
            var sb = new StringBuilder();
            foreach (var haltestelle in netzplan.Haltestellen)
            {
                var streckenString = string.Join("," + Environment.NewLine, GenerateStrecken(netzplan, haltestelle).ToArray());
                sb.AppendFormat(halteStellenformat, haltestelle.Name, streckenString);
            }

            return sb.ToString();
        }

        private IEnumerable<string> GenerateStrecken(Netzplan netzplan, Haltestelle haltestelle)
        {
            return haltestelle.Strecken.Select(strecke => string.Format(streckenformat, strecke.Linienname, strecke.Zielhaltestellenname));
        }

        private const string netzplanformat = @"
    private Netzplan testNetzplan = new Netzplan()
      {{
         Haltestellen = new[]
         {{
            {0}
         }}
      }};";

        private const string halteStellenformat = @"new Haltestelle
            {{
               Name = ""{0}"",
               Strecken = new[]
               {{
{1}
               }},
            }},
            ";

        private const string streckenformat = @"                    new Strecke {{ Linienname = ""{0}"", Zielhaltestellenname = ""{1}"" }}";
    }
}