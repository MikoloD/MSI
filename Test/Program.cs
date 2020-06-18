using System;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    class Program
    {
        static List<Osobnik> Populacja = new List<Osobnik>();
        public int liczebnosc { get; set; }
        public int liczbaGeneracji { get; set; }
        private void turniej()
        {
            Random rand = new Random();
            List<Osobnik> PopulacjaNowa = new List<Osobnik>();
            while (PopulacjaNowa.Count < liczebnosc)
            {
                List<Osobnik> grupa = new List<Osobnik>();
                var pierwszy = Populacja.First();
                var ostatni = Populacja.Last();
                Populacja.Remove(pierwszy);
                Populacja.Remove(ostatni);
                grupa.Add(pierwszy);
                grupa.Add(ostatni);
                var grupaMax = grupa.Max(x => x.f);
                Osobnik o1 = grupa.First(x => x.f == grupaMax);
                for (int i = 0; i < 2; i++)
                {
                    PopulacjaNowa.Add(o1);
                }
            }
            Populacja = PopulacjaNowa;

        }
        private double sprawdzDziedzine(double x)
        {
            do
            {
                Random rand = new Random();
                var los = rand.Next(0, 2);
                if (los != 0) x *= 1.1;
                else x *= 0.9;

            } while (Math.Abs(x) >= 2);
            return x;
        }
        private void mutuj()
        {
            foreach (var elem in Populacja)
            {
                elem.x = sprawdzDziedzine(elem.x);
                elem.y = sprawdzDziedzine(elem.y);
            }
        }
        private void RysujWykres()
        {

        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            liczebnosc = 100;
            liczbaGeneracji = 2;
            int c = 7;
            for (int i = 0; i < liczebnosc; i++)
            {
                Populacja.Add(new Osobnik(c));
            }

            for (int i = 0; i < liczbaGeneracji; i++)
            {
                turniej();
                mutuj();
            }
        }
        static void Main(string[] args)
        {
            foreach (var elem in Populacja)
            {
                Console.WriteLine(elem.x + " " + elem.y + " " + elem.f);
            }
            Console.ReadKey();
        }
    }
}