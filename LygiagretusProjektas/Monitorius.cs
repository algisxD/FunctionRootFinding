using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace LygiagretusProjektas
{
    class Monitorius
    {
        public int saknuSkaicius { get; set; }
        Intervalas[] intervalai;
        //List<Intervalas> intervalai;

        object resultlocker = new object();
        public bool dirba = true;
        public int intervaluSkaicius;
        int skaicius = 0;

        public Monitorius(int intervaluSkaicius)
        {
            this.saknuSkaicius = 0;
            this.intervaluSkaicius = intervaluSkaicius;
            intervalai = new Intervalas[this.intervaluSkaicius];
        }

        /// <summary>
        /// Metodas prideti intervalam i monitoriaus masyva
        /// </summary>
        /// <param name="intervalas"></param>
        public void pridetiIntervala(Intervalas intervalas)
        {
            intervalai[skaicius] = intervalas;
            skaicius++;
        }

        public void spausdintiIntervalus()
        {
            for (int i = 0; i < intervaluSkaicius; i++)
            {
                Console.WriteLine(i + ": " + intervalai[i].pradzia + " " + intervalai[i].pabaiga);
            }
        }

        /// <summary>
        /// Metodas apdorojantis intervala
        /// </summary>
        /// <param name="zingsnis"></param>
        /// <param name="index"></param>
        public void apdorotiIntervala(double zingsnis, int index)
        {
            //Console.WriteLine(index);
            Intervalas intervalas = intervalai[index];
            //Console.WriteLine("Gijos nr: " + index + " intervalas:" + intervalas.pradzia +" " + intervalas.pabaiga);
            int temp = ieskotiSaknuSkaiciaus(intervalas, zingsnis);
            //Console.WriteLine(intervalai[index].pradzia + " " + intervalai[index].pabaiga);
            lock (resultlocker)
            {
                saknuSkaicius = saknuSkaicius + temp;
            }
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
    }
}
