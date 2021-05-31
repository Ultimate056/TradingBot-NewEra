using NewEra_Bot.Cores;
using System;

namespace NewEra_Bot.MVVM.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private decimal _currentBalance;
        public decimal CurrentBalance { 
            get { return _currentBalance; }
            set { _currentBalance = Math.Round(value,2); OnPropertyChanged(); }
        }


        public MainWindowViewModel()
        {
            _currentBalance = Math.Round(MainWindow.user.Balance,2);
        }
    }
}
