using NewEra_Bot.Models;
using System;
namespace ConsoleTest
{
    class Program
    {
        static User us;
        static void Main(string[] args)
        {
            if(MyBinance.IsAlreadyConnection())
            {
                us = new User(MyBinance.path);
                Console.WriteLine(us.GetName());
                Console.WriteLine(us.GetReferalID());
                Console.WriteLine(us.GetUserID());
                Console.WriteLine(us.GetAPIKey());
                Console.WriteLine(us.GetSecretKey());
            }
            TradingBot tr = new TradingBot(us.GetAPIKey(), us.GetSecretKey());



            Console.ReadKey();
        }
    }
}
