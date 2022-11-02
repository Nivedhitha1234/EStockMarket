using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyService.Models
{
    public class CompanyInfo
    {
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string CompanyCEO { get; set; }
        public decimal TurnOver { get; set; }
        public string StockExchange { get; set; }
        public decimal stockPrice { get; set; }

    }

}
