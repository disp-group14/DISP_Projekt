using System;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using SalesService.DAL;
using SalesService.Models;
using SalesServiceGrpc.Protos;
using static SalesServiceGrpc.Protos.ISalesService;
using static ShareBrokerServiceGrpc.Protos.IShareBrokerService;

namespace SalesService.SAL {
    public class SalesServiceManager : ISalesServiceBase 
    {
        private readonly ISaleRequestDataManger dataManager;

        public SalesServiceManager(ISaleRequestDataManger dataManager)
        {
            this.dataManager = dataManager;
        }

        private bool matchLogic(SaleRequest saleRequest, PurchaseOffer purchaseOffer ) {
            return saleRequest.Price == purchaseOffer.Price && saleRequest.StockId == purchaseOffer.StockId;
        }

        public override async Task<MatchResponse> FindMatch(PurchaseOffer purchaseRequest, ServerCallContext context ) {
            // Create match response
            var MatchResponse = new MatchResponse();
            MatchResponse.Matches.AddRange(
                (await this.dataManager.Get(saleRequest => matchLogic(saleRequest, purchaseRequest)))
                .Select(saleRequest => (new Share {
                StockId = saleRequest.Id,
                Amount = saleRequest.Amount
            })));
            return MatchResponse;
        }
    }
}