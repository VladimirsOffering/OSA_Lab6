using Microsoft.Win32;
using OSA_Lab6.Models;
using OSA_Lab6.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OSA_Lab6.ViewModels
{
    public class OpenFileViewModel : ViewModelBase
    {
        OpenFileModel openFileModel;

        private ICommand openFileCommand;
        public ICommand OpenFileCommand
        {
            get
            {
                if (openFileCommand == null)
                {
                    openFileCommand = new RelayCommand(
                        param => this.OpenFile()
                    );
                }
                return openFileCommand;
            }
        }

        string filePath;
        string infoMsg;
        public string InfoMsg
        {
            get => infoMsg;
            set
            {
                infoMsg = value;
                OnPropertyChanged("InfoMsg");
            }
        }

        bool isEnabled;
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        MainViewModel vm;

        public OpenFileViewModel(MainViewModel mainViewModel)
        {
            vm = mainViewModel;
            IsEnabled = true;
            InfoMsg = "Сначала нужно выбрать excel файл с исходными данными";
            openFileModel = new OpenFileModel();
        }
        private async void OpenFile()
        {
            filePath = openFileModel.OpenFile();
            if (filePath == "")
            {
                InfoMsg = "Файл не выбран, нужно выбрать еще раз";
            }
            else
            {
                try
                {
                    InfoMsg = "Попытка прочитать файл";
                    //await Task.Run(()=> 
                    //{
                        IsEnabled = false;
                        var result = ExcelReader.Read(filePath);
                        InfoMsg = "Файл успешно прочитан";
                    //});
                    vm.CurrentPage = new MainPage(result);

                }
                catch (Exception e)
                {
                    InfoMsg = $"Ошибка при чтении файла {e.Message}";
                    IsEnabled = true;
                }
            }
        }
    }
}
