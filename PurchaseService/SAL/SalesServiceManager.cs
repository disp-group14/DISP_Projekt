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

        private bool matchLogic(PurchaseRequest purchaseRequest, SaleOffer saleOffer ) {
            return purchaseRequest.Price == saleOffer.Price && purchaseRequest.StockId == saleOffer.StockId;
        }

        public override async Task<MatchResponse> FindMatch(SaleOffer saleOffer, ServerCallContext context ) {
            var MatchResponse = new MatchResponse();
            MatchResponse.Matches.AddRange(
                (await this.dataManager.Get(purchaseRequest => matchLogic(purchaseRequest, saleOffer)))
                .Select(purchaseRequest => (new Share {
                StockId = purchaseRequest.Id,
                Amount = purchaseRequest.Amount
            })));
            return MatchResponse;
        }
    }
}