using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShareOwnerControl.Models;

namespace ShareOwnerControll.Controller
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class StockController : ControllerBase
    {
        [HttpGet]
        public async Task<List<Stock>> Get()
        {
            throw new NotImplementedException();
        }

        [HttpGet("id")]
        public async Task<Stock> Get(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<Stock> Put(Stock stock)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<Stock> Post(Stock stock)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public async Task Delete(Stock stock)
        {
            throw new NotImplementedException();
        }
    }


}