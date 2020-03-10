using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorytmGenetyczny_0._12
{
    public class Osobnik
    {
        public double x;
        public double y;
        public double f;

        private static Random rnd;
        static Osobnik()
        {
            rnd = new Random();
        }
        public Osobnik()
        {
            this.x = -2 + 4 * rnd.NextDouble();
            this.y = -2 + 4 * rnd.NextDouble();
            this.f = this.x + this.y; //zalezy od funkcji
        }
        public double mutujX()
        {
            return x +(-2 + 4 * rnd.NextDouble())/4.0;
        }
        public double mutujY()
        {
            return y + (-2 + 4 * rnd.NextDouble()) / 4.0;
        }
    }
}
