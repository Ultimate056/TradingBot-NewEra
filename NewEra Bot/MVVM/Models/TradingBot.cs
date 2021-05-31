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

namespace NewEra_Bot.Models
{

    public class TradingBot
    {
        const int MaxCountOrders = 20;
        string APIKey;
        string APISECRET;
        public TradingBot(string APIKey, string APISECRET)
        {
            this.APIKey = APIKey;
            this.APISECRET = APISECRET;
            var client = new BinanceClient(new BinanceClientOptions
            {
                ApiCredentials = new ApiCredentials(this.APIKey, this.APISECRET)
            });
            var startResult = client.Spot.UserStream.StartUserStream();
            if (startResult.Success)
                Console.WriteLine("Подключение открыто успешно");
            else
                Console.WriteLine("Ошибка подключения");

            // Работает
            var LastPrice = client.Spot.Market.Get24HPrice("LTCUSDT");
            Console.WriteLine(Math.Round(LastPrice.Data.LastPrice, 4));
            var balance = client.General.GetAccountInfo();
            BinanceAccountInfo b = new BinanceAccountInfo();
            if(balance.Success)
            {
                b = balance.Data;
                foreach(var zh in b.Balances)
                {
                    if(zh.Asset=="LTC" || zh.Asset=="BTC" || zh.Asset=="USDT")
                    {
                        Console.WriteLine(zh.Asset);
                        Console.WriteLine(zh.Free);
                        Console.WriteLine(zh.Locked);
                    }
                }
            }
            else
            {
                Console.WriteLine("error");
            }
            var bb = client.Spot.Order.PlaceOrder("LTCUSDT", OrderSide.Sell, OrderType.Limit, (decimal?)0.2, price: 200, timeInForce: TimeInForce.GoodTillCancel);
            if (bb.Success)
                Console.WriteLine("Ордер поставлен");
            else
                Console.WriteLine("Ордер не поставлен");




        }
    }
}
