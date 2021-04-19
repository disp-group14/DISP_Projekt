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
    public class HoldingController : ControllerBase
    {
        [HttpGet]
        public async Task<List<Holding>> Get()
        {
            throw new NotImplementedException();
        }

        [HttpGet("id")]
        public async Task<Holding> Get(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<Holding> Put(Holding holding)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<Holding> Post(Holding holding)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public async Task Delete(Holding holding)
        {
            throw new NotImplementedException();
        }
    }
}