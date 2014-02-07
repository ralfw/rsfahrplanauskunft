using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rsfa.contracts;
using rsfa.contracts.daten;

namespace rsfa.Netzplanberechnung
{
    public class Netzplanberechnung : INetzplanberechnung
    {
        private IFahrplanProvider fahrplanprovider;

        public Netzplanberechnung(IFahrplanProvider fahrplanProvider)
        {
            this.fahrplanprovider = fahrplanProvider;
        }

        public Netzplan Netzplan_berechnen()
        {
            var netzplan = new Netzplan { Haltestellen = this.FülleHalteStellen().ToArray() };
            return netzplan;
        }

        private IEnumerable<Haltestelle> FülleHalteStellen()
        {
            var alleHalteStellen = this.AlleHaltestellen();
            foreach (var linienName in this.fahrplanprovider.Liniennamen)
            {
                var haltestellenFürLinie = this.fahrplanprovider.Haltestellen_für_Linie(linienName);
                for (int index = 1; index < haltestellenFürLinie.Length; index++)
                {
                    var haltestellenName = haltestellenFürLinie[index - 1];
                    var zielHaltestellenName = haltestellenFürLinie[index];
                    var halteStelle = alleHalteStellen[haltestellenName];
                    var zielHalteStelle = alleHalteStellen[zielHaltestellenName];
                    this.Strecke_Hinzufügen(halteStelle, linienName, zielHalteStelle);
                }
            }

            return alleHalteStellen.Values;
        }

        private Dictionary<string, Haltestelle> AlleHaltestellen()
        {
            var halteStellen = new Dictionary<string, Haltestelle>();
            foreach (var linienName in this.fahrplanprovider.Liniennamen)
            {
                foreach (var haltestellenName in this.fahrplanprovider.Haltestellen_für_Linie(linienName)
                    .Where(haltestellenName => !halteStellen.ContainsKey(haltestellenName)))
                {
                    halteStellen[haltestellenName] = new Haltestelle
                                                         {
                                                             Strecken = new Strecke[] { }, 
                                                             Name = haltestellenName
                                                         };
                }
            }

            return halteStellen;
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
