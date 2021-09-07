using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace LygiagretusProjektas
{
    class Program
    {
        public const double zingsnis = 0.002;
        private static Monitorius monitorius;

        //Giju skaicius 2, 4, 6 arba 8
        public static int gijuSkaicius = 8;

        static void Main(string[] args)
        {
            Stopwatch laikmatis1 = new Stopwatch();
            Stopwatch laikmatis2 = new Stopwatch();
            int intervaloPradzia, intervaloPabaiga;

            intervaloPradzia = 0;
            intervaloPabaiga = 48;

            Console.WriteLine("Intervalas: " + intervaloPradzia + " <= x <= " + intervaloPabaiga);
            Console.WriteLine("Gijų skaicius: " + gijuSkaicius);
            Console.WriteLine();
            Console.WriteLine("Skaiciuojant lygiagreciai:");

            
            Intervalas intervalas = new Intervalas(intervaloPradzia, intervaloPabaiga);
            List<Intervalas> intervaluSarasas;
            intervaluSarasas = padalintiIntervala(intervalas, gijuSkaicius);


            monitorius = new Monitorius(intervaluSarasas.Count);
            pildytiReiksmem(intervaluSarasas);
            monitorius.spausdintiIntervalus();

            laikmatis1.Start();
            sukurtiGijas(gijuSkaicius);
            laikmatis1.Stop();

            Console.WriteLine("Saknu skaicius: " + monitorius.saknuSkaicius);

            Console.WriteLine("Uztrukta laiko: " + laikmatis1.Elapsed);

            Console.WriteLine();
            Console.WriteLine("Skaiciuojant paprastai:");
            laikmatis2.Start();
            Console.WriteLine("Saknu skaicius: " + ieskotiSaknuSkaiciaus(intervalas, zingsnis));
            laikmatis2.Stop();
            Console.WriteLine("Uztrukta laiko: " + laikmatis2.Elapsed);

            Console.ReadLine();
        }

        public static void pildytiReiksmem(List<Intervalas> intervaluSarasas)
        {
            foreach(var intervalas in intervaluSarasas)
            {
                monitorius.pridetiIntervala(intervalas);
            }
        }

        /// <summary>
        /// Metodas sukuriantis gijas
        /// </summary>
        /// <param name="gijuSkaicius"></param>
        static public void sukurtiGijas(int skaicius)
        {
            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < skaicius; i++)
            {
                Thread gija = new Thread(() =>
                {
                    int gijosSkaicius = Thread.CurrentThread.ManagedThreadId - 3;
                    monitorius.apdorotiIntervala(zingsnis, gijosSkaicius);
                });
                threads.Add(gija);
            }

            foreach (Thread thread in threads)
                thread.Start();
                
            foreach (Thread thread in threads)
                thread.Join();
        }

        /// <summary>
        /// Metodas ieskantis funkcijos saknu skaiciaus duotame intervale
        /// </summary>
        /// <param name="intervalas"></param>
        /// <param name="zingsnis"></param>
        /// <returns></returns>
        public static int ieskotiSaknuSkaiciaus(Intervalas intervalas, double zingsnis)
        {
            double taskas1 = intervalas.pradzia;
            double taskas2 = intervalas.pradzia + zingsnis;

            double atsakymas1 = 0;
            double atsakymas2 = 0;
            int skaicius = 0;
            while (taskas2 <= intervalas.pabaiga)
            {
                atsakymas1 = funkcija(taskas1);
                atsakymas2 = funkcija(taskas2);
                if (atsakymas1 > 0 && atsakymas2 < 0 || atsakymas1 < 0 && atsakymas2 > 0)
                    skaicius++;
                taskas1 = taskas2;
                taskas2 = taskas2 + zingsnis;
            }
            return skaicius;
        }

        /// <summary>
        /// Funkcija, kurios saknu bus ieskoma
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        static public double funkcija(double x)
        {
            return (double)(Math.Sin(x) / 2 + Math.Cos(x));
        }

        /// <summary>
        /// Metodas padalinantis intervala i tam tikra skaiciu mazesniu intervalu
        /// </summary>
        /// <param name="intervalas"></param>
        /// <returns></returns>
        public static List<Intervalas> padalintiIntervala(Intervalas intervalas, int gijuSkaicius)
        {
            double taskas1 = intervalas.pradzia;
            double taskas2 = intervalas.pabaiga;
            List<Intervalas> intervalai = new List<Intervalas>();

            double visasIntervalas = Math.Abs(intervalas.pradzia) + Math.Abs(intervalas.pabaiga);

            double mazesniuIntervaluIlgiai = visasIntervalas / gijuSkaicius;

            Intervalas laikinas;

            for (int i = 0; i < visasIntervalas / mazesniuIntervaluIlgiai; i++)
            {
                laikinas = new Intervalas(taskas1, taskas1 + mazesniuIntervaluIlgiai);
                taskas1 = taskas1 + mazesniuIntervaluIlgiai;
                intervalai.Add(laikinas);
            }
            return intervalai;
        }
    }
}
