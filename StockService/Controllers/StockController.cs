using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockService.DAL;
using StockService.Models;

namespace StockService.Controllers
{
    [ApiController]
    [Route("stockService/[controller]")]
    [Produces("application/json")]
    public class StockController : ControllerBase
    {
        private readonly IStockDataManager _stockDataManager;

        public StockController(IStockDataManager stockDataManager)
        {
            _stockDataManager = stockDataManager;
        }

        [HttpGet]
        public async Task<List<Stock>> GetAll()
        {
            return await _stockDataManager.Get();
        }
    }
}
