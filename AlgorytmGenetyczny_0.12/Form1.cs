using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;

namespace AlgorytmGenetyczny_0._12
{
    
    public partial class Form1 : Form
    {
        List<Osobnik> Populacja = new List<Osobnik>();
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        public double[] sumaPop = new double[200];
        public static int RandomNumber(int min, int max)
        {
            lock (syncLock)
            { 
                return random.Next(min, max);
            }
        }
        public int liczebnosc { get; set; }
        public int liczbaGeneracji { get; set; }
        public Form1()
        {
            InitializeComponent();

            
            
        }
        private void turniej()
        {
            List<Osobnik> PopulacjaNowa = new List<Osobnik>();
            while(PopulacjaNowa.Count < liczebnosc)
            {
                var grupa = Enumerable.Range(1, 2).Select(n => Populacja[RandomNumber(0, Populacja.Count)]).ToList();
                var grupaMax = grupa.Max(x => x.f);
                Osobnik o1 = grupa.First(x => x.f == grupaMax);
                PopulacjaNowa.Add(o1);
            }
            Populacja = PopulacjaNowa;
        }
        private double sprawdzDziedzine(int i,double x)
        {
            double wynik;
            do
            {
                if (RandomNumber(0, 2) != 0) wynik=x * 1.1;
                else wynik=x* 0.9;
            } while (Math.Abs(wynik) >= 2);
            return wynik;
            
        }
        
        private void mutuj()
        {
            for(int i=0;i<liczebnosc;i++)
            {
                Populacja[i].x = sprawdzDziedzine(i,Populacja[i].x);
                Populacja[i].y = sprawdzDziedzine(i,Populacja[i].y);
            }
        }
        public double fPop()
        {
            double wynik = 0;
            foreach(var elem in Populacja)
            {
                wynik += elem.f;
            }
            return wynik;
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            liczebnosc = int.Parse(textBox1.Text);
            liczbaGeneracji = int.Parse(textBox2.Text);
            const int c = 7;
            for (int i = 0; i < liczebnosc; i++)
            {
                Populacja.Add(new Osobnik(c));
            }

            for (int i = 0; i < liczbaGeneracji; i++)
            {
                turniej();
                mutuj();
                sumaPop[i] = fPop();
            }
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            xlWorkSheet.Cells[1, 1] = "x";
            xlWorkSheet.Cells[1, 2] = "y";
            xlWorkSheet.Cells[1, 3] = "ym";
            xlWorkSheet.Cells[1, 4] = "xm";
            xlWorkSheet.Cells[1, 5] = "f";

            for(int i=0;i<liczbaGeneracji;i++)
            {
                
                xlWorkSheet.Cells[i + 2, 1] = i;
                xlWorkSheet.Cells[i + 2, 2] = sumaPop[i];
                //xlWorkSheet.Cells[i + 2, 5] = Populacja[i].f;
            }
            List<string> listaPopulacji = new List<string>();

            ChartObjects xlCharts = (ChartObjects)xlWorkSheet.ChartObjects(Type.Missing);
            ChartObject myChart = xlCharts.Add(150, 50, 300, 250);
            Chart chartPage = myChart.Chart;
            Range chartRange;

            chartRange = xlWorkSheet.get_Range("B2:C201", Type.Missing);
            chartPage.SetSourceData(chartRange, misValue);
            chartPage.ChartType = XlChartType.xlXYScatter;

            var series = (Series)chartPage.SeriesCollection(1);
            series.XValues = xlWorkSheet.get_Range("A2:A202", Type.Missing);
            var series2 = (Series)chartPage.SeriesCollection(2);
            series2.XValues = xlWorkSheet.get_Range("D2:D202", Type.Missing);

            string path = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName; // return the application.exe current folder
            string fileName = Path.Combine(path, "wykres.bmp");
            string fileName2 = Path.Combine(path, "algorytm ewolucyjny");
            chartPage.Export(fileName, "BMP", misValue);
            pictureBox1.Image = new Bitmap(fileName);
            xlWorkBook.SaveAs(fileName2, XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);

            textBox3.Text = (sumaPop[liczebnosc-1] / liczebnosc).ToString();
        }

    }
}
