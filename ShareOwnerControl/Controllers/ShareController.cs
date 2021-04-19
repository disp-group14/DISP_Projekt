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
    public class ShareController : ControllerBase
    {
        [HttpGet]
        public async Task<List<Share>> Get()
        {
            throw new NotImplementedException();
        }

        [HttpGet("id")]
        public async Task<Share> Get(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<Share> Put(Share share)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<Share> Post(Share share)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public async Task Delete(Share share)
        {
            throw new NotImplementedException();
        }
    }
}