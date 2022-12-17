using Microsoft.Win32;
using OSA_Lab6.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OSA_Lab6.Pages
{
    /// <summary>
    /// Логика взаимодействия для OpenFile.xaml
    /// </summary>
    public partial class OpenFile : Page
    {
        OpenFileViewModel vm;
        public OpenFile(MainViewModel parent)
        {
            vm = new OpenFileViewModel(parent);
            this.DataContext = vm;
            InitializeComponent();
        }

    }
}
