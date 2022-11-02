using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockService.Models
{
    public class StockPriceData
    {
        public DateTime date { get; set; }
        public decimal stockPrice { get; set; }
    }
}
