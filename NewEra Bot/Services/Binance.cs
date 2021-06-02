using Binance.Net;
using Binance.Net.Objects.Spot;
using Binance.Net.Objects.Spot.SpotData;
using CryptoExchange.Net.Authentication;
using System;
using System.Collections.Generic;
using System.IO;

namespace NewEra_Bot.Models
{
    public static class MyBinance
    {
        public static string path { get; set; } = "C:\\Users\\79204\\source\\repos\\NewEra Bot\\NewEra Bot\\Resources\\info2.txt";

        public static Dictionary<string, decimal> Balances;

        public static User user;

        public static decimal StartUSDT;

        public static decimal StartBNB;

        // Проверка, есть ли уже заранее созданные настройки, если да, то чтение их из файла
        public static bool IsAlreadyConnection()
        {
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                if (sr.ReadLine() == null)
                    return false;     
                else
                    return true;
            }
        }

        public static void GetBalances()
        {
            var client = new BinanceClient(new BinanceClientOptions
            {
                ApiCredentials = new ApiCredentials(user.GetAPIKey(), user.GetSecretKey())
            });
            var balance = client.General.GetAccountInfo();
            BinanceAccountInfo b = new BinanceAccountInfo();
            if (balance.Success)
            {
                // Заполним словарь
                b = balance.Data;

                foreach (var zh in b.Balances)
                {
                    if (zh.Asset == "USDT")
                    {
                        StartUSDT = zh.Free;
                    }
                    if(zh.Asset =="BNB")
                    {
                        StartBNB = zh.Free;
                    }
                }
            }
        }

        public static void ConnectionToBinance(User user)
        {

        }
    }
}
