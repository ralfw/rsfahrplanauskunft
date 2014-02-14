namespace rsfa.Netzplanberechnung
{
    using System;
    using System.IO;
    using System.Text;

    using rsfa.contracts.daten;

    public class NetzplanVisualisierer
    {
        public void SchreibeDotFile(Netzplan netzplan, string dateiName)
        {
            File.WriteAllLines(dateiName, new[] { this.GenerateDot(netzplan) });
        }

        // shamelessly stolen from Pfadbestimmung
        // cut and paste strings to http://graphviz-dev.appspot.com/
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