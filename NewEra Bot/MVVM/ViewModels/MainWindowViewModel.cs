using Binance.Net.Enums;
using NewEra_Bot.Core;
using NewEra_Bot.Cores;
using NewEra_Bot.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace NewEra_Bot.MVVM.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private decimal _currentBalance;
        public decimal CurrentBalance { 
            get { return _currentBalance; }
            set { _currentBalance = Math.Round(value,2); OnPropertyChanged(); }
        }

        private uint _currentCountSuccessOrders;
        public uint CurrentCountSuccessOrders
        {
            get { return _currentCountSuccessOrders; }
            set { _currentCountSuccessOrders = value; OnPropertyChanged(); }
        }

        private decimal _currentCountBNB;
        public decimal CurrentCountBNB
        {
            get { return _currentCountBNB; }
            set { _currentCountBNB = Math.Round(value,5); OnPropertyChanged(); }
        }

        private ObservableCollection<Order> _noneCompleted;
        public ObservableCollection<Order> NoneCompletedOrders
        {
            get { return _noneCompleted; }
            set { _noneCompleted = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Order> _completed;
        public ObservableCollection<Order> CompletedOrders
        {
            get { return _completed; }
            set { _completed = value; OnPropertyChanged(); }
        }

        public RelayCommand Command1 { get; set; }


        public MainWindowViewModel()
        {
            _currentCountBNB = Math.Round(MainWindow.user.CountBNB,5);
            _currentCountSuccessOrders = 0;
            _noneCompleted = new ObservableCollection<Order>();
            _completed = new ObservableCollection<Order>();
            _currentBalance = Math.Round(MainWindow.user.Balance,2);
        }
    }
}
