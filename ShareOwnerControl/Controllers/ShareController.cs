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
    public class ShareController : ControllerBase
    {
        private readonly IShareLogic _shareLogic;
        public ShareController(IShareLogic shareLogic)
        {
            _shareLogic = shareLogic;
        }
        
        [HttpGet]
        public async Task<List<Share>> Get()
        {
            return (await _shareLogic.Get(includeProperties: new string[]{nameof(Share.Stock)})).ToList();
        }

        [HttpGet("id")]
        public async Task<Share> Get(int id)
        {
            return (await _shareLogic.Get(share => share.Id == id, includeProperties: new string[]{nameof(Share.Stock)})).FirstOrDefault();
        }

        [HttpPut]
        public async Task<Share> Put(Share share)
        {
            return await _shareLogic.Insert(share);
        }

        [HttpPost]
        public async Task<Share> Post(Share share)
        {
            return await _shareLogic.Update(share);
        }

        [HttpDelete]
        public async Task Delete(Share share)
        {
            await _shareLogic.Delete(share);
        }
    }
}