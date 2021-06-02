using Binance.Net.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewEra_Bot.Models
{
    public class Order
    {
        DateTime dateCreate;
        public string S_dateCreate { get; set; }
        public string symbol { get; set; }
        OrderType type;
        public string S_type { get; set; }

        OrderSide side;
        public string S_side { get; set; }

        OrderStatus status;
        public string S_status { get; set; }

        decimal quantity;
        public string S_q { get; set; }

        decimal price;
        public string S_price { get; set; }

        public Brush br { get; set; }

        public long OrderID { get; set; }

        public Order(DateTime dt, string symbol, OrderType type, OrderSide side, decimal quant, decimal price, OrderStatus status, long OrderID)
        {
            dateCreate = dt;
            this.symbol = symbol;
            this.type = type;
            this.side = side;
            quantity = Math.Round(quant,4);
            this.price = Math.Round(price,4);
            this.status = status;
            this.OrderID = OrderID;
        }

        public void ConvertToStringOrder()
        {
            S_dateCreate = dateCreate.ToString();
            S_type = type.ToString();
            S_side = side.ToString();
            S_status = status.ToString();
            S_q = Convert.ToString(quantity);
            S_price = Convert.ToString(price);

            if (S_side == "Buy")
                br = new SolidBrush(Color.Green);
            else if (S_side == "Sell")
                br = new SolidBrush(Color.Red);
            else
                br = new SolidBrush(Color.White);
        }

        public Order() { }
    }
}
