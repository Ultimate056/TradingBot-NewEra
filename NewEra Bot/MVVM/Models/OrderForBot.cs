using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewEra_Bot.MVVM.Models
{
    public class OrderForBot
    {
        public long ID_Order { get; set; }
        public decimal Price_vxoda { get; set; }
        public decimal quantity { get; set; }

        public OrderForBot(long id, decimal pr_vx, decimal q)
        {
            ID_Order = id;
            Price_vxoda = pr_vx;
            quantity = q;
        }

        OrderForBot() { }
    }
}
