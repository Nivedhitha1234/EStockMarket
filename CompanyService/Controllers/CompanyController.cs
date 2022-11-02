using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyService.Models;
using MongoDB.Driver;
using System.Text.Json;

namespace CompanyService.Controllers
{
    [Route("api/v1.0/market/[controller]/[action]")]
    [ApiController]
    public class CompanyController : Controller
    {
        private readonly IMongoCollection<CompanyDetails> _companyCollection;
        private readonly IMongoCollection<StockCollection> _stocksCollection;
        public CompanyController()
        {
            var dbhost = "localhost";
            var dbName = "EStockDB";
            var connectionstring = $"mongodb://{dbhost}:27017/{dbName}";
            var mongoUrl = MongoUrl.Create(connectionstring);
            var mongoClient = new MongoClient(mongoUrl);
            var database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
            _companyCollection = database.GetCollection<CompanyDetails>("companydetails");
            _stocksCollection = database.GetCollection<StockCollection>("stockData");
        }   

        
        [HttpPost]
        public ActionResult register(CompanyDetails company)
        {
            try
            {
                _companyCollection.InsertOne(company);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }           
           
        }

        [HttpGet]
        public ActionResult<IEnumerable<CompanyInfo>> getAll()
        {
            List<CompanyInfo> result = new List<CompanyInfo>();
            try
            {
                //var test = _companyCollection.AsQueryable().Where(a => a.CompanyCode);
                var query = _companyCollection.Aggregate().Lookup<CompanyDetails, StockCollection, CompanyAllData>(_stocksCollection, a => a.CompanyCode, a => a.CompanyCode, a => a.stockList)
                .ToEnumerable()
                .SelectMany(a => a.stockList.Select(b => new {
                    a.CompanyCode,
                    a.CompanyName,
                    a.CompanyCEO,
                    a.TurnOver,
                    a.StockExchange,
                    b.stockPrice,
                    b.Date
                }))
                .ToList();

                if (query.Count <= 0)
                {
                    //return StatusCode(400, "No data present");
                    return BadRequest(StatusCode(400, "No Data present"));
                }
                else
                {
                    result = query.AsQueryable()
                                   .OrderByDescending(q => q.Date)
                                   .GroupBy(q => q.CompanyCode)
                                   .Select(cmpny => new CompanyInfo
                                   {
                                       CompanyCode = cmpny.First().CompanyCode,
                                       CompanyName = cmpny.First().CompanyName,
                                       CompanyCEO = cmpny.First().CompanyCEO,
                                       TurnOver = cmpny.First().TurnOver,
                                       StockExchange = cmpny.First().StockExchange,
                                       stockPrice = cmpny.First().stockPrice
                                   })
                                   .ToList();
                    return result;
                }
                                   
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
          
        }
        
        [HttpGet("{companycode}")]
        public ActionResult<CompanyInfo> info(string companycode)
        {
            CompanyInfo result = new CompanyInfo();
            try
            {
                var query = _companyCollection.Aggregate().Lookup<CompanyDetails, StockCollection, CompanyAllData>(_stocksCollection, a => a.CompanyCode, a => a.CompanyCode, a => a.stockList)
                .ToEnumerable()
                .Where(a => a.CompanyCode == companycode)
                .SelectMany(a => a.stockList.Select(b => new {
                    a.CompanyCode,
                    a.CompanyName,
                    a.CompanyCEO,
                    a.TurnOver,
                    a.StockExchange,
                    b.stockPrice,
                    b.Date
                }))
                .ToList();

                if (query.Count <= 0)
                {
                    return StatusCode(400, "No data present");
                    //return BadRequest(StatusCode(400, "No Data present"));
                }
                else
                {
                    result = query.AsQueryable()
                                   .OrderByDescending(q => q.Date)
                                   .GroupBy(q => q.CompanyCode)
                                   .Select(cmpny => new CompanyInfo
                                   {
                                       CompanyCode = cmpny.First().CompanyCode,
                                       CompanyName = cmpny.First().CompanyName,
                                       CompanyCEO = cmpny.First().CompanyCEO,
                                       TurnOver = cmpny.First().TurnOver,
                                       StockExchange = cmpny.First().StockExchange,
                                       stockPrice = cmpny.First().stockPrice
                                   }).SingleOrDefault();
                    return result;
                }                
                
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
            //return result;
            //var filterDefinition = Builders<CompanyDetails>.Filter.Eq(x => x.CompanyCode, companycode);
            //return await _companyCollection.Find(filterDefinition).SingleOrDefaultAsync();
        }

        [HttpDelete("{companycode}")]
        public ActionResult delete(string companycode)
        {
            try
            {
                var filter1 = Builders<CompanyDetails>.Filter.Eq(x => x.CompanyCode, companycode);
                var filter2 = Builders<StockCollection>.Filter.Eq(x => x.CompanyCode, companycode);
                var count1 = _companyCollection.Find(filter1).FirstOrDefault();
                var count2 = _stocksCollection.Find(filter2).FirstOrDefault();
                if (count1 != null || count2 != null)
                {
                    _stocksCollection.DeleteMany(filter2);
                    _companyCollection.DeleteOne(filter1);
                    return Ok();
                }
                else
                {
                    return BadRequest(StatusCode(400, "No Data present"));
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
            
        }       
        
    }
}
