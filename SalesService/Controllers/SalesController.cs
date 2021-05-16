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
    [Route("[controller]")]
    [Produces("application/json")]
    public class SalesController : ControllerBase
    {

        private readonly ILogger<SalesController> _logger;
        private readonly ILogger<SalesController> logger;
        private ISaleRequestDataManger service;
        private readonly IShareBrokerServiceClient shareBrokerServiceClient;
        private readonly IOwnershipServiceClient ownershipServiceClient;

        public SalesController(ILogger<SalesController> logger, ISaleRequestDataManger service, IShareBrokerServiceClient shareBrokerServiceClient, IOwnershipServiceClient ownershipServiceClient)
        {
            this.logger = logger;
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
            var response = ownershipServiceClient.GetStockOwners(new StockOwnerRequest(){
                StockId = saleRequest.StockId
            });

            if (response.Owners.First(user => user.Id == saleRequest.UserId) == null) {
                throw new Exception("Invalid sale request. User with id: " + saleRequest.UserId.ToString() + 
                " does not own a share in stock with id: " + saleRequest.StockId.ToString());
            }

            // Save request in db
            var saleRequestModel = await this.service.Insert(saleRequest);

            // Return result from share broker
            return this.shareBrokerServiceClient.SellShare(new OfferRequest() {
                StockId = saleRequest.StockId,
                Amount = saleRequest.Amount,
                Price = saleRequest.Price
            });
        }
    }
}
