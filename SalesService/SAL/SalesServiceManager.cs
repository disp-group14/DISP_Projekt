using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using SalesService.DAL;
using SalesService.Models;
using SalesServiceGrpc.Protos;
using SharedGrpc.Protos;
using static SalesServiceGrpc.Protos.ISalesService;

namespace SalesService.SAL {
    public class SalesServiceManager : ISalesServiceBase 
    {
        private readonly ISaleRequestDataManger dataManager;

        public SalesServiceManager(ISaleRequestDataManger dataManager)
        {
            this.dataManager = dataManager;
        }

        private bool matchLogic(SaleRequest saleRequest, PurchaseOffer purchaseOffer ) {
            return saleRequest.Price <= purchaseOffer.Price && saleRequest.StockId == purchaseOffer.StockId;
        }

        public override async Task<MatchResponse> FindMatch(PurchaseOffer purchaseRequest, ServerCallContext context ) {
            var MatchResponse = new MatchResponse();
            MatchResponse.Matches.AddRange(
                (await this.dataManager.Get(saleRequest => matchLogic(saleRequest, purchaseRequest)))
                .Select(shareModel => (new Share {
                StockId = shareModel.StockId,
                Amount = shareModel.Amount,
                Price = shareModel.Price,
                Id = shareModel.Id
            })));
            return MatchResponse;
        }
    }
}