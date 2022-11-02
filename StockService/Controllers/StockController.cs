using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockService.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace StockService.Controllers
{
    [Route("api/v1.0/market/[controller]/[action]")]
    [ApiController]
    public class StockController : Controller
    {
        private readonly IMongoCollection<StockCollection> _stockCollection;
        public StockController()
        {
            var dbhost = "localhost";
            var dbName = "EStockDB";
            var connectionstring = $"mongodb://{dbhost}:27017/{dbName}";
            var mongoUrl = MongoUrl.Create(connectionstring);
            var mongoClient = new MongoClient(mongoUrl);
            var database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
            _stockCollection = database.GetCollection<StockCollection>("stockData");
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockCollection>>> getall()
        {
            return await _stockCollection.Find(Builders<StockCollection>.Filter.Empty).ToListAsync();
        }
        [HttpPost]
        public async Task<ActionResult> add(StockInputs stockInputs)
        {
            StockCollection stockObj = new StockCollection { 
                CompanyCode = stockInputs.companyCode,
                stockPrice = stockInputs.stockPrice,
                Date = DateTime.Now
            };
            await _stockCollection.InsertOneAsync(stockObj);
            return Ok();
        }

        [HttpGet("{companycode}/{startdate}/{enddate}")]
        public async Task<ActionResult<IEnumerable<StockPriceData>>> Get(string companycode, DateTime startdate, DateTime enddate)
        {
            var builder = Builders<StockCollection>.Filter;
            var filter = builder.And(builder.Eq("CompanyCode", companycode), builder.Gte("Date", startdate), builder.Lte("Date", enddate));
            //var result = _stockCollection.Find(filter).Project(a=>new {a.Date,a.stockPrice }).ToListAsync();
            var projection = Builders<StockCollection>.Projection.Include("Date").Include("stockPrice").Exclude("_id");
            return await _stockCollection.Find(filter).Project<StockPriceData>(projection).ToListAsync();

        }
    }
}
