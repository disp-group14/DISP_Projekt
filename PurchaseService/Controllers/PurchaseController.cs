using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PurchaseService.DAL;
using PurchaseService.Models;
using ShareBrokerServiceGrpc.Protos;
using static ShareBrokerServiceGrpc.Protos.IShareBrokerService;

namespace SalesService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class PurchaseController : ControllerBase
    {

        private readonly ILogger<PurchaseController> _logger;
        private readonly ILogger<PurchaseController> logger;
        private IPurchaseRequestDataManger service;
        private readonly IShareBrokerServiceClient shareBrokerServiceClient;

        public PurchaseController(ILogger<PurchaseController> logger, IPurchaseRequestDataManger service, IShareBrokerServiceClient shareBrokerServiceClient)
        {
            this.logger = logger;
            this.service = service;
            this.shareBrokerServiceClient = shareBrokerServiceClient;
        }

        [HttpGet]
        public async Task<List<PurchaseRequest>> Get()
        {
            return await this.service.Get();
        }

        [HttpPost]
        public async Task<PurchaseRequest> Post(PurchaseRequest purchaseRequest)
        {
            var purchaseRequestModel = await this.service.Insert(purchaseRequest);
            await this.shareBrokerServiceClient.PurchaseShareAsync(new OfferRequest() {
                StockId = purchaseRequest.StockId,
                Amount = purchaseRequest.Amount,
                Price = purchaseRequest.Price
            });
            return purchaseRequest;
        }
    }
}
