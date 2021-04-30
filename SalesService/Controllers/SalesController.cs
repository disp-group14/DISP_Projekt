using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SalesService.DAL;
using SalesService.Models;

namespace SalesService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class SalesController : ControllerBase
    {

        private readonly ILogger<SalesController> _logger;
        private ISaleRequestDataManger service;

        public SalesController(ILogger<SalesController> logger, ISaleRequestDataManger service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<List<SaleRequest>> Get()
        {
            return await this.service.Get();
        }

        [HttpPost]
        public async Task<SaleRequest> Post(SaleRequest saleRequest)
        {
            return await this.service.Insert(saleRequest);
        }
    }
}
