using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SalesService.DAL;
using SalesService.Models;
using ShareBrokerServiceGrpc.Protos;
using static ShareBrokerServiceGrpc.Protos.IShareBrokerService;

namespace SalesService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class SalesController : ControllerBase
    {

        private readonly ILogger<SalesController> _logger;
        private readonly ILogger<SalesController> logger;
        private ISaleRequestDataManger service;
        private readonly IShareBrokerServiceClient shareBrokerServiceClient;

        public SalesController(ILogger<SalesController> logger, ISaleRequestDataManger service, IShareBrokerServiceClient shareBrokerServiceClient)
        {
            this.logger = logger;
            this.service = service;
            this.shareBrokerServiceClient = shareBrokerServiceClient;
        }

        [HttpGet]
        public async Task<List<SaleRequest>> Get()
        {
            return await this.service.Get();
        }

        [HttpPost]
        public async Task<SaleRequest> Post(SaleRequest saleRequest)
        {
            var saleRequestModel = await this.service.Insert(saleRequest);
            this.shareBrokerServiceClient.SellShare(new OfferRequest() {
                StockId = saleRequest.StockId,
                Amount = saleRequest.Amount,
                Price = saleRequest.Price
            });
            return saleRequest;
        }
    }
}
