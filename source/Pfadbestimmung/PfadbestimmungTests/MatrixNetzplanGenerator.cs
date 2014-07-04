using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PfadbestimmungTests
{
    using rsfa.contracts.daten;

    class MatrixNetzplanGenerator 
    {
        // Zahl
        public int Laenge { get; set; }

        // Buchstabe
        public int Breite { get; set; }

        public MatrixNetzplanGenerator(int laenge, int breite)
        {
            this.Laenge = laenge;
            this.Breite = breite;
        }

        public Netzplan BerechneNetzplan()
        {
            var netzplan = new Netzplan();
            netzplan.Haltestellen = this.BerechneHalteStellen().ToArray();
            return netzplan;
        }

        private IEnumerable<Haltestelle> BerechneHalteStellen()
        {
            for (int x = 0; x < this.Breite; x++)
            {
                for (int y = 0; y < this.Laenge; y++)
                {
                    var haltestelle = new Haltestelle();
                    haltestelle.Name = this.HaltestellenName(x, y);
                    haltestelle.Strecken = this.Strecken(x, y).ToArray();
                    yield return haltestelle;
                }
            }
        }

        private IEnumerable<Strecke> Strecken(int x, int y)
        {
            if (x > 0)
            {
                yield return new Strecke() { Linienname = "X" + x + "West", Zielhaltestellenname = this.HaltestellenName(x - 1, y) };
            }

            if (y > 0)
            {
                yield return new Strecke() { Linienname = "Y" + y + "Nord", Zielhaltestellenname = this.HaltestellenName(x, y - 1) };
            }

            if (x < this.Breite - 1)
            {
                yield return new Strecke() { Linienname = "X" + x + "Ost", Zielhaltestellenname = this.HaltestellenName(x + 1, y) };
            }

            if (y < this.Laenge - 1)
            {
                yield return new Strecke() { Linienname = "Y" + y + "Süd", Zielhaltestellenname = this.HaltestellenName(x, y + 1) };
            }
        }

        private string HaltestellenName(int x, int y)
        {
            var c = Convert.ToChar(65 + x);
            ////char a = 'A';
            ////char hs = Char. Char.GetNumericValue(a)
            return c.ToString() + y;
        }
    }
}
