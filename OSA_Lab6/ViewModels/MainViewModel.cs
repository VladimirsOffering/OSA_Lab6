using OSA_Lab6.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OSA_Lab6.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

        Page currentPage;
        public Page CurrentPage
        {
            get => currentPage;
            set
            {
                currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }
        public Page OpenFile { get; private set; }

        public double Opacity { get; set; }

        public MainViewModel()
        {
            OpenFile = new OpenFile(this);
            CurrentPage = OpenFile;
        }
    }
}
