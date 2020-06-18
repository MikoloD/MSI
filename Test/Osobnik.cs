using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class Osobnik
    {
        public double x;
        public double y;
        public double f;
        public double c;

        private static Random rnd;
        static Osobnik()
        {
            rnd = new Random();
        }
        public Osobnik(double c)
        {
            x = -2 + 4 * rnd.NextDouble();
            y = -2 + 4 * rnd.NextDouble();
            f = -x*x - y*y + c; //zalezy od funkcji
        }
    }
}
