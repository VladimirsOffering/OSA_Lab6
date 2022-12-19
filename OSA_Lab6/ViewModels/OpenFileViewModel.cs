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

        string buttonText;
        public string ButtonText
        {
            get => buttonText;
            set
            {
                buttonText = value;
                OnPropertyChanged("ButtonText");
            }
        }

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
            InfoMsg = "Для начала нужно выбрать excel файл с альтернативами";
            openFileModel = new OpenFileModel();
            ButtonText = "Выбор исходного файла с альтернативами";
        }

        string AFilePath;
        string PFilePath;

        double[,] resultA;
        double[,] resultP;
 
        private async void OpenFile()
        {
            if (resultA == null)
            {
                ButtonText = "Выбор исходного файла с альтернативами";
                AFilePath = openFileModel.OpenFile();
                if (AFilePath == "")
                {
                    InfoMsg = "Файл не выбран, нужно выбрать еще раз";
                    return;
                }
                else
                {
                    try
                    {
                        InfoMsg = "Попытка прочитать файл";
                        //await Task.Run(()=> 
                        //{
                        IsEnabled = false;
                        resultA = ExcelReader.Read(AFilePath);
                        InfoMsg = "Хорошо, теперь нужно выбрать следующий файл";
                        ButtonText = "Выбор исходного файла с вероятностями P";
                        IsEnabled = true;
                        return;
                        //});

                    }
                    catch (Exception e)
                    {
                        InfoMsg = $"Ошибка при чтении файла {e.Message}";
                        IsEnabled = true;
                    }
                }

            }
            if (resultP == null)
            {
                PFilePath = openFileModel.OpenFile();
                if (PFilePath == "")
                {
                    InfoMsg = "Файл не выбран, нужно выбрать еще раз";
                    return;
                }
                else
                {
                    try
                    {
                        InfoMsg = "Попытка прочитать файл";
                        //await Task.Run(()=> 
                        //{
                        IsEnabled = false;
                        resultP = ExcelReader.Read(PFilePath);
                        IsEnabled = true;
                        InfoMsg = "Файлы успешно прочитаны, осталось запустить";
                        ButtonText = "Начать";
                        return;
                        //});

                    }
                    catch (Exception e)
                    {
                        InfoMsg = $"Ошибка при чтении файла {e.Message}";
                        IsEnabled = true;
                    }
                }

            }

            

            vm.CurrentPage = new MainPage(resultA,resultP);

        }
    }
}
