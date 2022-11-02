using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CompanyService.Models
{
    public class CompanyDetails
    {   [Required] 
        [BsonId,BsonElement("_CompanyCode"), BsonRepresentation(BsonType.String)]
        public string CompanyCode { get; set; }
        [Required]
        [BsonElement("CompanyName"), BsonRepresentation(BsonType.String)]
        public string CompanyName { get; set; }
        [Required]
        [BsonElement("CompanyCEO"), BsonRepresentation(BsonType.String)]
        public string CompanyCEO { get; set; }
        [Required]      
        [BsonElement("TurnOver"), BsonRepresentation(BsonType.Decimal128)]
        [Range(10,double.MaxValue,ErrorMessage = "Company Turnover must be greater than 10Cr.")]
        public decimal TurnOver { get; set; }
        [Required]
        [BsonElement("StockExchange"), BsonRepresentation(BsonType.String)]
        public string StockExchange { get; set; }
    }

    public class CompanyAllData : CompanyDetails
    {
        public List<StockCollection> stockList { get; set; }

    }
}
