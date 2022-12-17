using MoreLinq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSA_Lab6.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {

        #region Propeties
        public double WaldCriterion { get; private set; }
        public int WaldCriterionIndex { get; private set; }

        public double GamblerCriterion { get; private set; }
        public int GamblerCriterionIndex { get; private set; }

        public double HurwitzCriterion { get; private set; }
        public int HurwitzCriterionIndex { get; private set; }

        public double LaplasCriterion { get; private set; }
        public int LaplasCriterionIndex { get; private set; }

        public double SavageCriterion { get; private set; }
        public int SavageCriterionIndex { get; private set; }


        public double MultiplyCriterion { get; private set; }
        public int MultiplyCriterionIndex { get; private set; }
        #endregion

        public double[,] values { get; set; }


        public List<List<double>> Rows { get; private set; }

        List<List<double>> Columns;

        List<List<double>> MissedOpportunities;


        private List<List<double>> ConvertArrToRowsList(double[,] values)
        {
            List<List<double>> result = new List<List<double>>();

            for (int i = 0; i < values.GetLength(0); i++)
            {
                var row = new List<double>();
                for (int j = 0; j < values.GetLength(1); j++)
                {
                    row.Add(values[i, j]);
                }
                result.Add(row);
            }
            return result;
        }

        private List<List<double>> ConvertArrToColumnsList(double[,] values)
        {
            List<List<double>> result = new List<List<double>>();

            for (int i = 0; i < values.GetLength(1); i++)
            {
                var column = new List<double>();
                for (int j = 0; j < values.GetLength(0); j++)
                {
                    column.Add(values[j, i]);
                }
                result.Add(column);
            }
            return result;
        }


        public MainPageViewModel(double[,] values)
        {
            this.values = values;
            Rows = ConvertArrToRowsList(values);
            Columns = ConvertArrToColumnsList(values);

            Start();

        }

        public async void Start()
        {
            await Task.Run(() => 
            {
                //Часть 1             
                MaxMin();
                MaxMax();
                CalculateHurwitz(0.5);
                CalculateLaplas();
                CalculateMultiplyCriterion();


                //Часть 2
                SavageCriterion = CalculateSavage(Columns).Value;
                SavageCriterionIndex = CalculateSavage(Columns).Index;

            });
        }


        public void MaxMin()
        {
            WaldCriterion = Rows.Max(x => x.Min());
            WaldCriterionIndex = Rows.FindIndex(x => x.Min() == WaldCriterion)+1;
        }

        public void MaxMax()
        {
            GamblerCriterion = Rows.Max(x => x.Max());
            GamblerCriterionIndex = Rows.FindIndex(x => x.Max() == GamblerCriterion) + 1;
        }

        public void CalculateHurwitz(double lambda)
        {
            HurwitzCriterion = Rows.Max(x => (x.Min() * (1-lambda) + x.Max() * lambda));
            HurwitzCriterionIndex = Rows.FindIndex(x => (x.Min() * (1 - lambda) + x.Max() * lambda) == HurwitzCriterion)+1;
        }


        public void CalculateModifyHurwitz(double lambda, double crit)
        {
            var HurwitzCriterion = Rows.Max(x => (x.Min() * (1 - lambda) + x.Max() * lambda));
        }

        public void CalculateLaplas()
        {
            LaplasCriterion = Rows.Max(x => x.Average());
            LaplasCriterionIndex = Rows.FindIndex(x => x.Average() == LaplasCriterion) + 1;
        }

        public void CalculateMultiplyCriterion()
        {
            double Max = double.MinValue;
            int Index = 0;
            foreach (var row in Rows)
            {
                double mul = 1;
                foreach (var item in row)
                {
                    mul *= item;
                }
                if (mul > Max)
                {
                    Max = mul;
                    MultiplyCriterion = Max;
                    MultiplyCriterionIndex = Index+1;
                }
                Index++;
            }
        }


        public (double Value, int Index) CalculateSavage(List<List<double>> source)
        {
            MissedOpportunities = new List<List<double>>();

            foreach (List<double> column in Columns)
            {
                var MissedColumn = new List<double>();
                var max = column.Max();
                foreach (double value in column)
                {
                    MissedColumn.Add(max - value);
                }

                MissedOpportunities.Add(MissedColumn);
            }

            double MinValue = double.MaxValue;
            int MinIndex = 0;

            foreach (var row in MissedOpportunities.Transpose().Select((value, i) => new { i, value }))
            {
                if (row.value.Max() < MinValue)
                {
                    MinValue = row.value.Max();
                    MinIndex = row.i+1;
                }
                
            }

            return (MinValue, MinIndex);

        }

    }
}
