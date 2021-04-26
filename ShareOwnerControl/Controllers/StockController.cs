using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShareOwnerControl.BLL;
using ShareOwnerControl.Models;

namespace ShareOwnerControl.Controller
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class StockController : ControllerBase
    {
        private IStockLogic _stockLogic;
        public StockController(IStockLogic stockLogic)
        {
            _stockLogic = stockLogic;
        }
        [HttpGet]
        public async Task<List<Stock>> Get()
        {
            return (await _stockLogic.Get()).ToList();
        }

        [HttpGet("id")]
        public async Task<Stock> Get(int id)
        {
            return (await _stockLogic.Get(stock => stock.Id == id)).FirstOrDefault();
        }

        [HttpPut]
        public async Task<Stock> Put(Stock stock)
        {
            return await _stockLogic.Insert(stock);
        }

        [HttpPost]
        public async Task<Stock> Post(Stock stock)
        {
            return await _stockLogic.Update(stock);
        }

        [HttpDelete]
        public async Task Delete(Stock stock)
        {
            await _stockLogic.Delete(stock);
        }
    }


}