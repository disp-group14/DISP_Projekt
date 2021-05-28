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
using OwnershipServiceGrpc.Protos;
using static OwnershipServiceGrpc.Protos.IOwnershipService;

namespace SalesService.Controllers
{
    [ApiController]
    [Route("salesService/[controller]")]
    [Produces("application/json")]
    public class SalesController : ControllerBase
    {

        private ISaleRequestDataManger service;
        private readonly IShareBrokerServiceClient shareBrokerServiceClient;
        private readonly IOwnershipServiceClient ownershipServiceClient;

        public SalesController(ISaleRequestDataManger service, IShareBrokerServiceClient shareBrokerServiceClient, IOwnershipServiceClient ownershipServiceClient)
        {
            this.service = service;
            this.shareBrokerServiceClient = shareBrokerServiceClient;
            this.ownershipServiceClient = ownershipServiceClient;
        }

        [HttpGet]
        public async Task<List<SaleRequest>> Get()
        {
            return await this.service.Get();
        }

        [HttpPost]
        public async Task<OfferResponse> Post(SaleRequest saleRequest)
        {
            // Verify user ownership
            var response = await ownershipServiceClient.GetSharesInStockAsync(new SharesInStockRequest(){
                UserId = saleRequest.UserId,
                StockId = saleRequest.StockId
            });

            // Validate user owns correct amount of shares
            if(response.Shares.Count < saleRequest.Amount)
            {
                throw new Exception("Invalid sale request. User with id: " + saleRequest.UserId.ToString() + 
                " does not own enough shares in stock with id: " + saleRequest.StockId.ToString());
            }

            // Return result from share broker
            var brokerResponse = await this.shareBrokerServiceClient.SellShareAsync(new OfferRequest() {
                StockId = saleRequest.StockId,
                Amount = saleRequest.Amount,
                Price = saleRequest.Price,
                UserId = saleRequest.UserId
            });
            
            if (brokerResponse.ResponseCase == OfferResponse.ResponseOneofCase.Registration) {
                // Save request in db
                var saleRequestModel = await this.service.Insert(saleRequest);
            }
            return brokerResponse;
        }
    }
}
