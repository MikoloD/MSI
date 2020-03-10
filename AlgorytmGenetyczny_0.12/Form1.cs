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
        public Form1()
        {
            InitializeComponent();

            int liczebnosc = 100;
            Osobnik[] Populacja = new Osobnik[liczebnosc];
            for (int i = 0; i < liczebnosc; i++)
            {
                Populacja[i] = new Osobnik();
            }
            Console.WriteLine();

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            xlWorkSheet.Cells[1, 1] = "x";
            xlWorkSheet.Cells[1, 2] = "y";
            xlWorkSheet.Cells[1, 3] = "z";

            for (int i = 0; i < liczebnosc; i++)
            {
                xlWorkSheet.Cells[i + 2, 1] = Populacja[i].x;
                xlWorkSheet.Cells[i + 2, 2] = Populacja[i].y;
                xlWorkSheet.Cells[i + 2, 3] = Populacja[i].f;
            }


            ChartObjects xlCharts = (ChartObjects)xlWorkSheet.ChartObjects(Type.Missing);
            ChartObject myChart = (ChartObject)xlCharts.Add(150, 50, 300, 250);
            Chart chartPage = myChart.Chart;
            Range chartRange;


            chartRange = xlWorkSheet.get_Range("B2:B101", Type.Missing);
            chartPage.SetSourceData(chartRange, misValue);
            chartPage.ChartType = XlChartType.xlXYScatter;
            var series = (Series)chartPage.SeriesCollection(1);
            series.XValues = xlWorkSheet.get_Range("A2:A101", Type.Missing);

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
            
        }
    }
}
