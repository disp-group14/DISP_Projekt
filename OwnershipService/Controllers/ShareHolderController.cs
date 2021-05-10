using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OwnershipService.Models;

namespace OwnershipService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class ShareHolderController : ControllerBase
    {
        public ShareHolderController()
        {

        }

        [HttpGet]
        public Task<ShareHolder> Get(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public Task<ShareHolder> Put(ShareHolder shareHolder)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public Task<ShareHolder> Post(ShareHolder shareHolder)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}