using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewEra_Bot.Models
{
    class TradingCouple
    {
        public string Crypto { get; set; }
        public string Fiat { get; set; }

        public override string ToString()
        {
            return Crypto + Fiat;
        }
    }
}
