using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShareOwnerControl.BLL;
using ShareOwnerControl.Models;

namespace ShareOwnerControl.Controller
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class HoldingController : ControllerBase
    {
        private readonly IHoldingLogic _holdingLogic;

        public HoldingController(IHoldingLogic holdingLogic)
        {
            _holdingLogic = holdingLogic;
        }
        [HttpGet]
        public async Task<List<Holding>> Get()
        {
            return (await _holdingLogic.Get()).ToList();
        }

        [HttpGet("id")]
        public async Task<Holding> Get(int id)
        {
            return (await _holdingLogic.Get(holding => holding.Id == id)).FirstOrDefault();
        }

        [HttpPut]
        public async Task<Holding> Put(Holding holding)
        {
            return await _holdingLogic.Insert(holding);
        }

        [HttpPost]
        public async Task<Holding> Post(Holding holding)
        {
            return await _holdingLogic.Update(holding);
        }

        [HttpDelete]
        public async Task Delete(Holding holding)
        {
            await _holdingLogic.Delete(holding);
        }
    }
}