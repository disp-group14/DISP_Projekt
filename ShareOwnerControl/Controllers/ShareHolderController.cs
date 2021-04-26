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
    public class ShareHolderController : ControllerBase
    {
        private IShareHolderLogic _shareHolderLogic;
        public ShareHolderController(IShareHolderLogic shareHolderLogic)
        {
            _shareHolderLogic = shareHolderLogic;
        }

        [HttpGet]
        public async Task<List<ShareHolder>> Get()
        {
            return (await _shareHolderLogic.Get(includeProperties: new string[]{nameof(ShareHolder.Holdings)})).ToList();
        }

        [HttpGet("id")]
        public async Task<ShareHolder> Get(int id)
        {
            return (await _shareHolderLogic.Get(shareHolder =>  shareHolder.Id == id, includeProperties: new string[]{nameof(ShareHolder.Holdings)})).FirstOrDefault();
        }

        [HttpPut]
        public async Task<ShareHolder> Put(ShareHolder shareHolder)
        {
            return await _shareHolderLogic.Insert(shareHolder);
        }

        [HttpPost]
        public async Task<ShareHolder> Post(ShareHolder shareHolder)
        {
            return await _shareHolderLogic.Update(shareHolder);
        }

        [HttpDelete]
        public async Task Delete(ShareHolder shareHolder)
        {
            await _shareHolderLogic.Delete(shareHolder);
        }
    }
}