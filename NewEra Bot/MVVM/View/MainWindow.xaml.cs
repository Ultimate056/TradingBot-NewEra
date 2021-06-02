using Binance.Net;
using Binance.Net.Enums;
using Binance.Net.Objects.Spot;
using Binance.Net.Objects.Spot.SpotData;
using CryptoExchange.Net.Authentication;
using NewEra_Bot.Models;
using NewEra_Bot.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace NewEra_Bot
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static User user;
        public static MainWindowViewModel vm;
        public static List<string> str =new List<String> { "Шортовая", "Лонговая", "Усреднения" };
        public static BinanceClient client;
        TradingBot tr;


        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        public void StartFillInfoAboutUser()
        {
            if (MyBinance.IsAlreadyConnection())
                user = new User(MyBinance.path);
            else
            {
                string name = "", ReferalID = "", API_Key = "", Secret_Key = "";
                long ID_User = 0;
                ModalWindow mw1 = new ModalWindow("Имя: ");
                if (mw1.ShowDialog() == true)
                    name = mw1.GetValue();
                ModalWindow mw2 = new ModalWindow("ReferalID: ");
                if (mw2.ShowDialog() == true)
                    ReferalID = mw2.GetValue();
                ModalWindow mw3 = new ModalWindow("API Key: ");
                if (mw3.ShowDialog() == true)
                    API_Key = mw3.GetValue();
                ModalWindow mw4 = new ModalWindow("Secret Key: ");
                if (mw4.ShowDialog() == true)
                    Secret_Key = mw4.GetValue();
                ModalWindow mw5 = new ModalWindow("ID пользователя: ");
                if (mw5.ShowDialog() == true)
                    ID_User = long.Parse(mw5.GetValue());
                user = new User(name, ReferalID, ID_User, API_Key, Secret_Key);
            }
        }
        public MainWindow()
        {
            StartFillInfoAboutUser();
            MyBinance.user = user;
            InitializeComponent();
            client = new BinanceClient(new BinanceClientOptions
            {
                ApiCredentials = new ApiCredentials(user.GetAPIKey(), user.GetSecretKey())
            });
            NameUser.Text = user.GetName();
            ReferalID.Text = user.GetReferalID();
            ID_User.Text = user.GetUserID().ToString();
            Secret_Key.Text = user.GetSecretKey();
            API_Key.Text = user.GetAPIKey();
            MyBinance.GetBalances();
            user.Balance = MyBinance.StartUSDT;
            user.CountBNB = MyBinance.StartBNB;
            ListSymbols.ItemsSource = InitializeSymbols();

            ListStrategies.ItemsSource = str;
            vm = new MainWindowViewModel();
            DataContext = vm;

            timer.Tick += new EventHandler(timerTickUpdateNotCompletedOrders);
            timer.Tick += new EventHandler(timerTickUpdateBalanceUser);
            timer.Interval = new TimeSpan(0, 0, 5);
            timer.Start();
        }

        private void timerTickUpdateNotCompletedOrders(object sender, EventArgs e)
        {
            var Orders = client.Spot.Order.GetAllOrders("LTCUSDT");
            foreach(var order in Orders.Data)
            {
                if (order.Status == OrderStatus.New)
                    continue;
                for(int i = 0; i < user.ListNotCompleted.Count; i++)
                {
                    if (order.OrderId == user.ListNotCompleted[i].OrderID)
                    {
                        var NewOrder = new Order(order.CreateTime, order.Symbol, order.Type, order.Side, order.Quantity, order.Price, order.Status, order.OrderId);
                        NewOrder.ConvertToStringOrder();
                        user.ListNotCompleted.RemoveAt(i);
                        user.ListCompleted.Add(NewOrder);
                        vm.NoneCompletedOrders = user.ListNotCompleted;
                        vm.CompletedOrders = user.ListCompleted;
                        if (tr != null)
                            tr.CountOrderOdnovremenno--;
                    }
                }
            }
        }

        private void timerTickUpdateBalanceUser(object sender, EventArgs e)
        {
            var balance = client.General.GetAccountInfo();
            BinanceAccountInfo b = new BinanceAccountInfo();
            if (balance.Success)
            {
                b = balance.Data;

                // Проблема оптимизации
                foreach (var zh in b.Balances)
                {
                    if (zh.Asset == "USDT")
                    {
                        if (user.Balance != zh.Free)
                        {
                            user.Balance = zh.Free;
                            vm.CurrentBalance = user.Balance;
                        }            
                    }
                }
            }

           var bnb = client.General.GetUserCoins().Data;
           // Проблема оптимизации
           foreach(var item in bnb)
            {
                if(item.Name == "BNB")
                {
                    user.CountBNB = item.Free;
                    vm.CurrentCountBNB = user.CountBNB;
                }    
            }
        }

        private List<String> InitializeSymbols()
        {
            List<string> col = new List<string>();
            var symbols = client.Spot.System.GetExchangeInfo().Data.Symbols;
            foreach(var symbol in symbols)
            {
                if (symbol.Name.EndsWith("USDT"))
                    col.Add(symbol.Name);
            }
            return col;

            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tr = new TradingBot(ListSymbols.SelectedItem.ToString(), ListStrategies.SelectedItem.ToString(), double.Parse(Profit.Text), decimal.Parse(StartVolume.Text));
            tr.Start();
        }
    }
}
