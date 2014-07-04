using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rsfa.pfadbestimmung;
using rsfa.contracts;
using rsfa.contracts.daten;

namespace PfadbestimmungTests
{
    class RandomNetzplanGenerator : INetzplanberechnung
    {
        private Random seed;

        public RandomNetzplanGenerator(int randomSeed)
        {
            this.seed = new Random(randomSeed);
            this.NumberOfHaltestellen = 30;
            this.NumberOfLinien = 5;

            this.MinimumStopsPerLine = this.NumberOfHaltestellen / 5;
            this.MaximumStopsPerLine = this.NumberOfHaltestellen / 2;
        }

        public Int32 NumberOfHaltestellen
        {
            get;
            set;
        }

        public Int32 NumberOfLinien
        {
            get;
            set;
        }

        public Int32 MinimumStopsPerLine { get; set; }
        public Int32 MaximumStopsPerLine { get; set; }
        
        public Netzplan Netzplan_berechnen()
        {
            string[] linien = this.generiereNamen("linie").Take(this.NumberOfLinien).ToArray();
            Haltestelle[] haltestellen = this.generiereNamen("haltestelle")
                .Take(this.NumberOfHaltestellen)
                .Select(name => new Haltestelle() { Name = name, Strecken = new Strecke[0] }) 
                .ToArray();

            foreach (string linie in linien)
            {
                var startHaltestelle = haltestellen[this.seed.Next(haltestellen.Length)];
                var numberOfStops = this.seed.Next(this.MinimumStopsPerLine, this.MaximumStopsPerLine);
                for (int i = 0; i < numberOfStops; i++)
                {
                    var nextStop = haltestellen[this.seed.Next(haltestellen.Length)];
                    var strecke = new Strecke { Linienname = linie, Zielhaltestellenname = nextStop.Name };
                    
                    var strecken = new List<Strecke>(startHaltestelle.Strecken);
                    strecken.Add(strecke);
                    startHaltestelle.Strecken = strecken.ToArray();

                    startHaltestelle = nextStop;
                }
            }

            return new Netzplan { Haltestellen = haltestellen };
        }

        private IEnumerable<string> generiereNamen(string prefix)
        {
            int i=0;

            while(true)
                yield return prefix + i++;
        }
    }
}
