using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CompanyService.Models
{
    public class StockCollection
    {
        [BsonId, BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("CompanyCode"), BsonRepresentation(BsonType.String)]
        public string CompanyCode { get; set; }
        [BsonElement("date"), BsonRepresentation(BsonType.DateTime)]
        public DateTime Date { get; set; }
        [BsonElement("stockPrice"), BsonRepresentation(BsonType.Decimal128)]
        public decimal stockPrice { get; set; }
    }
}
