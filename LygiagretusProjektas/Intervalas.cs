using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LygiagretusProjektas
{
    /// <summary>
    /// Intervalu klase
    /// </summary>
    public class Intervalas
    {
        public double pradzia { get; }
        public double pabaiga { get; }
        public double ilgis { get; }

        public Intervalas(double x1, double x2)
        {
            this.pradzia = x1;
            this.pabaiga = x2;
            ilgis = Math.Abs(this.pradzia) + Math.Abs(this.pabaiga);
        }
    }
}
