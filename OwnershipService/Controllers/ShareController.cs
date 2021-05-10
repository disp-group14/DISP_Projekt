using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OwnershipService.Models;

namespace OwnershipService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class ShareController : ControllerBase
    {
        public ShareController()
        {

        }

        [HttpGet]
        public Task<Share> Get(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public Task<Share> Put(Share share)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public Task<Share> Post(Share share)
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