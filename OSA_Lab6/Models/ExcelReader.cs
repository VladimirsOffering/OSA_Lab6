using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace OSA_Lab6.Models
{
    static public class ExcelReader
    {
        public static double[,] Read(string filename)
        {
            Application excelApp = new Application();
            Workbook excelBook = excelApp.Workbooks.Open(filename);
            _Worksheet excelSheet = excelBook.Sheets[1];
            Range excelRange = excelSheet.UsedRange;

            int rows = excelRange.Rows.Count;
            int cols = excelRange.Columns.Count;

            //List<List<double>> values = new List<List<double>>();

            double[,] values = new double[rows, cols];

            for (int i = 1; i <= rows; i++)
            {
                for (int j = 1; j <= cols; j++)
                {
                    if (excelRange.Cells[i, j] != null && excelRange.Cells[i, j].Value2 != null)
                        values[i-1,j-1] = Double.Parse((excelRange.Cells[i, j].Value2).ToString());
                }
            }

            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            return values;
        }
    }
}
