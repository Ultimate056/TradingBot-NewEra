using Binance.Net;
using Binance.Net.Interfaces;
using Binance.Net.Objects.Spot;
using Binance.Net.Objects.Spot.MarketData;
using Binance.Net.Objects.Spot.SpotData;
using Binance.Net.Objects.Spot.MarketStream;
using Binance.Net.SymbolOrderBooks;
using Binance.Net.Objects.Shared;
using CryptoExchange.Net.Authentication;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Binance.Net.Enums;
using System.Threading;
using NewEra_Bot.MVVM.Models;

namespace NewEra_Bot.Models
{

    public class TradingBot
    {
        public static BinanceClient client;
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        const int MaxCountOrders = 20;
        public int CountOrderOdnovremenno { get; set; }

        decimal profit; // 

        decimal comissia = 0.00075M; // 0.75%

        string _choiseSymbol;

        string _choiseStrategies;

        decimal StartVolume;
        List<OrderForBot> listofb = new List<OrderForBot>();

        public TradingBot(string symbol, string choise_str, double profit = 1, decimal StartVolume = 1)
        {
            _choiseSymbol = symbol;
            _choiseStrategies = choise_str;
            this.profit = (decimal)profit / 100;
            this.StartVolume = StartVolume / 100;
            client = new BinanceClient(new BinanceClientOptions
            {
                ApiCredentials = new ApiCredentials(MainWindow.user.GetAPIKey(), MainWindow.user.GetSecretKey())
            });

            timer.Tick += new EventHandler(timerTickUpdate);
            timer.Interval = new TimeSpan(0,0,15);
            timer.Start();
        }


        bool start_first = false;


        // Динам.данные 
        decimal Price_close;
        decimal quantity = 0;
        decimal Price_vxoda;
        decimal Nominal;
        
        // -
        static int countError = 0; 
        public void Start()
        {
            if(_choiseStrategies == "Усреднения")
            {
                if (StartVolume * MainWindow.user.Balance < 10 || StartVolume * MainWindow.user.Balance >= MainWindow.user.Balance)
                    return;
                else
                {
                    if (CountOrderOdnovremenno >= MaxCountOrders)
                        return;
                    Nominal = StartVolume * MainWindow.user.Balance;
                    Price_vxoda = client.Spot.Market.GetCurrentAvgPrice(_choiseSymbol).Data.Price;
                    quantity = Math.Round(Nominal / Price_vxoda, 4);
                    var Order = client.Spot.Order.PlaceOrder(_choiseSymbol, OrderSide.Buy, OrderType.Market, (decimal?)quantity);
                    Thread.Sleep(1000);
                    if (Order.Success)
                    {
                        Nominal = Nominal - Nominal * comissia;
                        Price_close = Math.Round((Nominal * (1 + profit)) / (decimal)Order.Data.Quantity, 2);
                        quantity = Math.Round(Order.Data.Quantity, 4);
                        // Визуальная часть
                        Order ord = new Order(Order.Data.CreateTime, Order.Data.Symbol, Order.Data.Type, Order.Data.Side, Order.Data.Quantity, Order.Data.Price, Order.Data.Status, Order.Data.OrderId);
                        ord.ConvertToStringOrder();
                        MainWindow.user.ListCompleted.Add(ord);
                        MainWindow.vm.CompletedOrders = MainWindow.user.ListCompleted;
                        // -

                        Thread.Sleep(1000);
                        // Cтавим ордер на закрытие по профиту
                        var OrderTwo = client.Spot.Order.PlaceOrder(_choiseSymbol, OrderSide.Sell, OrderType.Limit, (decimal?)quantity, price: (decimal?)Price_close, timeInForce: TimeInForce.GoodTillCancel);
                        Order ord2 = new Order(OrderTwo.Data.CreateTime, OrderTwo.Data.Symbol, OrderTwo.Data.Type, OrderTwo.Data.Side, OrderTwo.Data.Quantity, OrderTwo.Data.Price, OrderTwo.Data.Status, OrderTwo.Data.OrderId);
                        ord2.ConvertToStringOrder();
                        MainWindow.user.ListNotCompleted.Add(ord2);
                        MainWindow.vm.NoneCompletedOrders = MainWindow.user.ListNotCompleted;
                        OrderForBot ofb = new OrderForBot(OrderTwo.Data.OrderId, Price_vxoda, quantity);
                        listofb.Add(ofb);
                        CountOrderOdnovremenno++;
                    }
                }
            }
        }

        private void timerTickUpdate(object sender, EventArgs e)
        {
            foreach(var item in listofb)
            {
                if (client.Spot.Market.GetCurrentAvgPrice(_choiseSymbol).Data.Price < Math.Round(item.Price_vxoda * 0.99M, 2))
                {
                    bool find = false;
                    var Orders = client.Spot.Order.GetAllOrders(_choiseSymbol);
                    foreach (var order in Orders.Data)
                    {
                        if (!(order.Status == OrderStatus.New))
                            continue;
                        
                        for (int i = 0; i < MainWindow.user.ListNotCompleted.Count; i++)
                        {
                            if(MainWindow.user.ListCompleted[i].OrderID == item.ID_Order)
                            { 
                                if(order.OrderId == item.ID_Order)
                                {
                                    client.Spot.Order.CancelOrder(_choiseSymbol, order.OrderId);
                                    order.Status = OrderStatus.Canceled;
                                    var NewOrder = new Order(order.CreateTime, order.Symbol, order.Type, order.Side, order.Quantity, order.Price, order.Status, order.OrderId);
                                    NewOrder.ConvertToStringOrder();
                                    MainWindow.user.ListNotCompleted.RemoveAt(i);
                                    MainWindow.user.ListCompleted.Add(NewOrder);
                                    CountOrderOdnovremenno--;
                                    find = true;
                                    if (MainWindow.user.Balance >= 11)
                                    {
                                        decimal temp = StartVolume;
                                        StartVolume = Math.Round(StartVolume * 1.5M, 4);
                                        Start();
                                        StartVolume = temp;
                                    }
                                }
                            }
                        }
                    }
                    if (find)
                        listofb.Remove(item);
                    countError++;
                    return;
                }
                if (client.Spot.Market.GetCurrentAvgPrice(_choiseSymbol).Data.Price >= Math.Round(item.Price_vxoda * 0.99M, 2) && client.Spot.Market.GetCurrentAvgPrice(_choiseSymbol).Data.Price < item.Price_vxoda)
                {
                    if (CountOrderOdnovremenno <= MaxCountOrders)
                    {
                        Start();
                        return;
                    }
                    return;
                }
                MainWindow.vm.CurrentCountSuccessOrders++;
            }














            
        }
    }
}
