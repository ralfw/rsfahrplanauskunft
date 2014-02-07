using rsfa.contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsfa.FahrplanProvider
{
    public class FahrplanProvider : IFahrplanProvider
    {
        private class Linie
        {
            internal Linie(string linienName, string[] haltestellen, TimeSpan[] timespan)
            {
                this.LinienName = linienName;
                this.Haltestellen = haltestellen;
                this.Timespan = timespan;
            }

            public string LinienName { get; set; }
            public string[] Haltestellen { get; set; }
            public TimeSpan[] Timespan { get; set; }
        }

        private readonly List<Linie> _linien;


        public FahrplanProvider()
        {
            _linien = new List<Linie>
                {
                    new Linie("Kaiser-Franz Linie 8 West",
                              new string[]
                                  {
                                      "Untergiesing", "Grünwalder Stadion", "Staatskanzlei", "Olympiastation", "Kufstein",
                                      "Lichtgestalthausen"
                                  },
                              new TimeSpan[]
                                  {
                                      TimeSpan.FromMinutes(0), TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(2),
                                      TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(2),
                                  }),
                    new Linie("Transrapid Linie Stoiber Nord",
                              new string[]
                                  {"Untergiesing", "Wolfratshausen", "Staatskanzlei", "Brüssel", "Lichtgestalthausen"},
                              new TimeSpan[]
                                  {
                                      TimeSpan.FromMinutes(0), TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(2),
                                      TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(2)
                                  }),
                    new Linie("Hans-Dampf-Seehofer Linie Ost",
                              new string[]
                                  {
                                      "Lichtgestalthausen", "Ingolstadt", "Ingolstädter Strasse", "Staatskanzlei",
                                      "Bei der Freundin in Berlin", "Obertupfing", "Untergiesing"
                                  },
                              new TimeSpan[]
                                  {
                                      TimeSpan.FromMinutes(0), TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(2),
                                      TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(2)
                                  }),
                    new Linie("Kaiser-Franz Linie 8 Ost",
                              new string[]
                                  {
                                      "Lichtgestalthausen", "Kufstein", "Olympiastation", "Staatskanzlei",
                                      "Grünwalder Stadion", "Untergiesing",
                                  },
                              new TimeSpan[]
                                  {
                                      TimeSpan.FromMinutes(0), TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(2),
                                      TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(2),
                                  }),
                    new Linie("Transrapid Linie Stoiber Süd",
                              new string[]
                                  {"Lichtgestalthausen", "Brüssel", "Staatskanzlei", "Wolfratshausen", "Untergiesing",},
                              new TimeSpan[]
                                  {
                                      TimeSpan.FromMinutes(0), TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(2),
                                      TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(2)
                                  }),
                    new Linie("Hans-Dampf-Seehofer Linie West",
                              new string[]
                                  {
                                      "Untergiesing", "Obertupfing", "Bei der Freundin in Berlin", "Staatskanzlei",
                                      "Ingolstädter Strasse", "Ingolstadt", "Lichtgestalthausen",
                                  },
                              new TimeSpan[]
                                  {
                                      TimeSpan.FromMinutes(0), TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(2),
                                      TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(2)
                                  })
                };
        }



        public DateTime[] Abfahrtszeiten_bei_Haltestelle(string linienname, string haltestellenname)
        {
            Linie linie =null;
            TimeSpan linienOffset = TimeSpan.FromMinutes(0);
            
            foreach (var item in this._linien)
            {
                if (item.LinienName == linienname)
                {
                    linienOffset += TimeSpan.FromMinutes(3);
                    linie = item;
                    break;
                }
            }

            TimeSpan haltestellenOffset = TimeSpan.FromMinutes(0);

            for (int index= 0;index < linie.Haltestellen.Count(); index++)
            {
                haltestellenOffset += linie.Timespan[index];
                if (linie.Haltestellen[index] == haltestellenname)
                {
                    return new DateTime[] { new DateTime(2014, 2, 7, 16, 38, 0) + haltestellenOffset + linienOffset, new DateTime(2014, 2, 7, 16, 43, 0) + haltestellenOffset, new DateTime(2014, 2, 7, 16, 48, 0) + haltestellenOffset + linienOffset, };
                }
            }

            throw new InvalidOperationException(string.Format("Unbekannter Linien- oder Haltestellenname: {0}/{1}", linienname, haltestellenname));
        }

        public TimeSpan Fahrtdauer_für_Strecke(string linienname, string haltestellenname)
        {
            Linie linie = null;
            foreach (var item in this._linien)
            {
                if (item.LinienName == linienname)
                {
                    linie = item;
                    break;
                }
            }

            for (int index = 0; index < linie.Haltestellen.Count(); index++)
            {
                if (linie.Haltestellen[index] == haltestellenname)
                {
                    return linie.Timespan[index];
                }
            }

            throw new InvalidOperationException(string.Format("Unbekannter Linien- oder Haltestellenname: {0}/{1}", linienname, haltestellenname));
        }


        public string[] Haltestellen_für_Linie(string linienname)
        {
            foreach (var item in this._linien)
            {
                if (item.LinienName == linienname)
                {
                    return item.Haltestellen;
                }
            }
            
            throw new InvalidOperationException("Linienname nicht bekannt: " + linienname);
        }

        /// <summary>
        /// /sdfsad
        /// </summary>
        public string[] Liniennamen
        {
            get { return _linien.Select(l => l.LinienName).ToArray(); }
        }
    }
}
