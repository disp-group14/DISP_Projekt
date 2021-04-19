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
    public class ShareHolderController : ControllerBase
    {
        [HttpGet]
        public async Task<List<ShareHolder>> Get()
        {
            throw new NotImplementedException();
        }

        [HttpGet("id")]
        public async Task<ShareHolder> Get(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<ShareHolder> Put(ShareHolder shareHolder)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ShareHolder> Post(ShareHolder shareHolder)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public async Task Delete(ShareHolder shareHolder)
        {
            throw new NotImplementedException();
        }
    }
}