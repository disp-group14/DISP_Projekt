using System;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using PurchaseService.DAL;
using PurchaseService.Models;
using PurchaseServiceGrpc.Protos;
using static PurchaseServiceGrpc.Protos.IPurchaseService;

namespace PurchaseService.SAL {
    public class PurchaseServiceManager : IPurchaseServiceBase 
    {
        private readonly IPurchaseRequestDataManger dataManager;

        public PurchaseServiceManager(IPurchaseRequestDataManger dataManager)
        {
            this.dataManager = dataManager;
        }

        public override async Task<MatchResponse> FindMatch(SaleOffer saleOffer, ServerCallContext context ) {
            var MatchResponse = new MatchResponse();
            var matches = (await this.dataManager.Get(purchaseRequest => purchaseRequest.StockId == saleOffer.StockId))
                .Where(offer => offer.Price <= saleOffer.Price)
                .Select(purchaseRequest => (new Share {
                StockId = purchaseRequest.Id,
                Amount = purchaseRequest.Amount
            }));
            MatchResponse.Matches.AddRange(matches);
            return MatchResponse;
        }
    }
}