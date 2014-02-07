using System;
using System.Collections.Generic;
using System.Linq;
using rsfa.contracts;
using rsfa.contracts.daten;

namespace rsfa.Netzplanberechnung
{
    public class Netzplanberechnung : INetzplanberechnung
    {
        private IFahrplanProvider fahrplanprovider;
        private Dictionary<string, Haltestelle> alleHalteStellen = new Dictionary<string, Haltestelle>();

        public Netzplanberechnung(IFahrplanProvider fahrplanProvider)
        {
            this.fahrplanprovider = fahrplanProvider;
        }

        public Netzplan Netzplan_berechnen()
        {
            var netzplan = new Netzplan { Haltestellen = this.BerechneHalteStellen().ToArray() };
            return netzplan;
        }

        private IEnumerable<Haltestelle> BerechneHalteStellen()
        {
            foreach (var linienName in this.fahrplanprovider.Liniennamen)
            {
                var haltestellenFürLinie = this.fahrplanprovider.Haltestellen_für_Linie(linienName);
                for (int index = 1; index < haltestellenFürLinie.Length; index++)
                {
                    var haltestellenName = haltestellenFürLinie[index - 1];
                    var zielHaltestellenName = haltestellenFürLinie[index];

                    Haltestelle halteStelle = this.FindeOderErzeugeHalteStelle(haltestellenName);
                    Haltestelle zielHalteStelle = this.FindeOderErzeugeHalteStelle(zielHaltestellenName);

                    this.Strecke_Hinzufügen(halteStelle, linienName, zielHalteStelle);
                }
            }

            return alleHalteStellen.Values;
        }

        private Haltestelle FindeOderErzeugeHalteStelle(String haltestellenName)
        {
            if (!alleHalteStellen.ContainsKey(haltestellenName))
            {
                alleHalteStellen[haltestellenName] = new Haltestelle
                {
                    Strecken = new Strecke[] { },
                    Name = haltestellenName
                };
            }

            return alleHalteStellen[haltestellenName];
        }

        private void Strecke_Hinzufügen(Haltestelle halteStelle, string linienname, Haltestelle ziel)
        {
            var strecke = new Strecke();
            strecke.Linienname = linienname;
            strecke.Zielhaltestellenname = ziel.Name;
            
            var aktuelleStrecken = halteStelle.Strecken.ToList();
            aktuelleStrecken.Add(strecke);
            halteStelle.Strecken = aktuelleStrecken.ToArray();
        }
    }
}
