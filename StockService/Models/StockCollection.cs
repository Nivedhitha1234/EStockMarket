using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace StockService.Models
{
    public class StockCollection
    {
        [BsonId, BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        //[Required]
        [BsonElement("CompanyCode"), BsonRepresentation(BsonType.String)]
        public string CompanyCode { get; set; }
        [BsonElement("date"), BsonRepresentation(BsonType.DateTime)]
        public DateTime Date { get; set; }
        //[Required]
        [BsonElement("stockPrice"),BsonRepresentation(BsonType.Decimal128)]
        public decimal stockPrice { get; set; }
    }
    public class StockInputs
    {
        [Required]       
        public string companyCode { get; set; }
        
        [Required]
        public decimal stockPrice { get; set; }

    }
}
