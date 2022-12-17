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

        double[,] values;

        public MainPageViewModel(double[,] values)
        {
            this.values = values;
        }

        public void CalculateWald()
        {

        }
    }
}
