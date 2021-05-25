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
using OwnershipServiceGrpc.Protos;
using static OwnershipServiceGrpc.Protos.IOwnershipService;

namespace SalesService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class PurchaseController : ControllerBase
    {
        private IPurchaseRequestDataManger service;
        private readonly IShareBrokerServiceClient shareBrokerServiceClient;
        private readonly IOwnershipServiceClient ownershipServiceClient;

        public PurchaseController(IPurchaseRequestDataManger service, IShareBrokerServiceClient shareBrokerServiceClient, IOwnershipServiceClient ownershipServiceClient)
        {
            this.service = service;
            this.shareBrokerServiceClient = shareBrokerServiceClient;
            this.ownershipServiceClient = ownershipServiceClient;
        }

        [HttpGet]
        public async Task<List<PurchaseRequest>> Get()
        {
            return await this.service.Get();
        }

        [HttpPost]
        public async Task<OfferResponse> Post(PurchaseRequest purchaseRequest)
        {
            var offer = new OfferRequest();
            // Return result from share broker
            var brokerResponse = await this.shareBrokerServiceClient.PurchaseShareAsync(new OfferRequest() {
                StockId = purchaseRequest.StockId,
                Amount = purchaseRequest.Amount,
                Price = purchaseRequest.Price,
                UserId = purchaseRequest.UserId
            });
            
            if (brokerResponse.ResponseCase == OfferResponse.ResponseOneofCase.Registration) {
                // Save request in db
                var purchaseRequestModel = await this.service.Insert(purchaseRequest);
            }
            return brokerResponse;
        }
    }
}
