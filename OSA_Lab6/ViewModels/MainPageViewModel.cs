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

        public double MultiplyCriterion { get; private set; }
        public int MultiplyCriterionIndex { get; private set; }


        public double SavageCriterion { get; private set; }
        public int SavageCriterionIndex { get; private set; }

        public double LaplasRegretsCriterion { get; private set; }
        public int LaplasRegretsCriterionIndex { get; private set; }
        #endregion



        #region Матрицы

        public double[,] SourceMatrix { get; private set; }

        double[,] positiveMatrix;
        public double[,] PositiveMatrix
        {
            get => positiveMatrix;
            private set
            {
                positiveMatrix = value;
                OnPropertyChanged("PositiveMatrix");
            }
        }


        double[,] missedOpportunitiesMatrix;
        public double[,] MissedOpportunitiesMatrix
        {
            get => missedOpportunitiesMatrix;
            private set
            {
                missedOpportunitiesMatrix = value;
                OnPropertyChanged("MissedOpportunitiesMatrix");
            }
        }


        public List<List<double>> Rows { get; private set; }

        List<List<double>> Columns;

        List<List<double>> MissedOpportunities;
        #endregion


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


        private double[,] ConvertListToArr(IEnumerable<IEnumerable<double>> values)
        {
            double[,] result = new double[values.Count(), values.First().Count()];

            int i = 0;
            foreach (var row in values)
            {
                int j = 0;
                foreach (var item in row)
                {
                    result[i, j] = item;
                    j++;
                }
                i++;
            }
            return result;
        }


        public MainPageViewModel(double[,] values)
        {
            SourceMatrix = values;
            Rows = ConvertArrToRowsList(values);
            Columns = ConvertArrToColumnsList(values);

            Start();

        }

        public void Start()
        {

            //Часть 1             
            MaxMin();
            MaxMax();
            CalculateHurwitz(0.5);
            var Laplas = CalculateLaplas(Rows);
            LaplasCriterion = Laplas.value;
            LaplasCriterionIndex = Laplas.index;
            CalculateMultiplyCriterion();


            //Часть 2
            CalculateRegretsMatrix();
            CalculateSavage();
            var LaplasRegrets = CalculateLaplas(MissedOpportunities);
            LaplasRegretsCriterion = LaplasRegrets.value;
            LaplasRegretsCriterionIndex = LaplasRegrets.index;


        }


        public void MaxMin()
        {
            WaldCriterion = Rows.Max(x => x.Min());
            WaldCriterionIndex = Rows.FindIndex(x => x.Min() == WaldCriterion) + 1;
        }

        public void MaxMax()
        {
            GamblerCriterion = Rows.Max(x => x.Max());
            GamblerCriterionIndex = Rows.FindIndex(x => x.Max() == GamblerCriterion) + 1;
        }

        public void CalculateHurwitz(double lambda)
        {
            HurwitzCriterion = Rows.Max(x => (x.Min() * (1 - lambda) + x.Max() * lambda));
            HurwitzCriterionIndex = Rows.FindIndex(x => (x.Min() * (1 - lambda) + x.Max() * lambda) == HurwitzCriterion) + 1;
        }


        public void CalculateModifyHurwitz(double lambda, double crit)
        {
            var HurwitzCriterion = Rows.Max(x => (x.Min() * (1 - lambda) + x.Max() * lambda));
        }

        public (double value, int index) CalculateLaplas(List<List<double>> source)
        {
            var LaplasCriterion = source.Max(x => x.Average());
            var LaplasCriterionIndex = source.FindIndex(x => x.Average() == LaplasCriterion) + 1;

            return (LaplasCriterion, LaplasCriterionIndex);
        }

        public void CalculateMultiplyCriterion()
        {
            double Max = double.MinValue;
            int Index = 0;

            PositiveMatrix = new double[SourceMatrix.GetLength(0), SourceMatrix.GetLength(1)];
            Array.Copy(SourceMatrix, PositiveMatrix, SourceMatrix.Length);

            var Min = Rows.Min(x => x.Min());
            if (Min < 0)
            {
                for (int i = 0; i < PositiveMatrix.GetLength(0); i++)
                {
                    for (int j = 0; j < PositiveMatrix.GetLength(1); j++)
                    {
                        PositiveMatrix[i, j] += (Min * -1) + 1;
                    }
                }
            }

            List<List<double>> Positive = ConvertArrToRowsList(PositiveMatrix);

            foreach (var row in Positive)
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
                    MultiplyCriterionIndex = Index + 1;
                }
                Index++;
            }
        }


        public void CalculateRegretsMatrix()
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
            MissedOpportunitiesMatrix = ConvertListToArr(MissedOpportunities.Transpose());

            MissedOpportunities = ConvertArrToRowsList(MissedOpportunitiesMatrix);
        }

        public void CalculateSavage()
        {
            double MinValue = double.MaxValue;
            int MinIndex = 0;

            foreach (var row in MissedOpportunities.Select((value, i) => new { i, value }))
            {
                if (row.value.Max() < MinValue)
                {
                    MinValue = row.value.Max();
                    MinIndex = row.i + 1;
                }

            }
            SavageCriterion = MinValue;
            SavageCriterionIndex = MinIndex;

        }

    }
}
