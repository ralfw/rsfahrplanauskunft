using System;
using System.Collections.Generic;
using System.Linq;
using rsfa.contracts.daten;

namespace rsfa.app
{
    internal class Konsolenprovider
    {
        public void Verbindungen_anzeigen(IEnumerable<Verbindung> verbindungen)
        {
            bool headerShown = false;

            foreach (var v in verbindungen)
            {
                int anzahlStrecken = v.Pfad.Strecken.Length;

                if (!headerShown)
                {
                    Console.WriteLine(
                        "Verbindungen von {0} nach {1}",
                        v.Pfad.Starthaltestellenname,
                        v.Pfad.Strecken.Last().Zielhaltestellenname);
                    headerShown = true;
                }

                Console.WriteLine();
                Console.WriteLine(
                    "Abfahrt {0}, Ankunft {1}, Reisezeit {2}",
                    v.Fahrtzeiten.First().Abfahrtszeit,
                    v.Fahrtzeiten.Last().Ankunftszeit,
                    v.Fahrtzeiten.Last().Ankunftszeit - v.Fahrtzeiten.First().Abfahrtszeit);

                for (int i = 0; i < anzahlStrecken; i++)
                {
                    Console.WriteLine(
                        "  mit {0} bis {1}, Abfahrt {2}, Ankunft {3}",
                        v.Pfad.Strecken[i].Linienname,
                        v.Pfad.Strecken[i].Zielhaltestellenname,
                        v.Fahrtzeiten[i].Abfahrtszeit.TimeOfDay,
                        v.Fahrtzeiten[i].Ankunftszeit.TimeOfDay);
                }
            }

            Console.WriteLine();
            Console.WriteLine("Der R&S Express wünscht eine gute Fahrt! Zum Beenden eine Taste drücken");
            Console.ReadKey();
        }
    }
}