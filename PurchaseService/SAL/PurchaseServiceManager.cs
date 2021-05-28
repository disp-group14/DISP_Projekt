using System;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using PurchaseService.DAL;
using PurchaseService.Models;
using PurchaseServiceGrpc.Protos;
using PurchaseRequestGrpc = PurchaseServiceGrpc.Protos.PurchaseRequest;
using SharedGrpc.Protos;
using static PurchaseServiceGrpc.Protos.IPurchaseService;

namespace PurchaseService.SAL
{
    public class PurchaseServiceManager : IPurchaseServiceBase
    {
        private readonly IPurchaseRequestDataManger _purchaseDataManager;

        public PurchaseServiceManager(IPurchaseRequestDataManger purchaseDataManager)
        {
            _purchaseDataManager = purchaseDataManager;
        }

        public override async Task<PurchaseRequestList> FindMatch(SaleOffer saleOffer, ServerCallContext context)
        {
            var response = new PurchaseRequestList();
            // Get All purchase requests where stockId matches and price is lower or equal to offer.
            var purchaseRequests = (await this._purchaseDataManager.Get(purchaseRequest => purchaseRequest.StockId == saleOffer.StockId
            && purchaseRequest.Price <= saleOffer.Price)).OrderBy(share => share.Price);

            // Loop through purchase requests 
            for (int index = 0; index < purchaseRequests.Count(); index++)
            {   
                // Convert to single object for easier use in logic
                var request = purchaseRequests.ToList()[index];

                // If amount matches exactly
                if (request.Amount == saleOffer.Amount)
                {
                    response.PurchaseRequests.Add(new PurchaseRequestGrpc()
                    {
                        Price = request.Price,
                        StockId = request.StockId,
                        Amount = request.Amount,
                        BuyerUserId = request.UserId
                    });
                    await _purchaseDataManager.Delete(request);
                    break;
                }
                // purchase request amount is higher than saleOffer
                else if (request.Amount > saleOffer.Amount)
                {
                    response.PurchaseRequests.Add(new PurchaseRequestGrpc()
                    {
                        Price = request.Price,
                        StockId = request.StockId,
                        Amount = saleOffer.Amount,
                        BuyerUserId = request.UserId
                    });

                    request.Amount -= saleOffer.Amount;
                    await _purchaseDataManager.Update(request);

                    break;
                }
                // purchase request amount 
                else if (request.Amount < saleOffer.Amount)
                {
                    response.PurchaseRequests.Add(new PurchaseRequestGrpc()
                    {
                        Price = request.Price,
                        StockId = request.StockId,
                        Amount = request.Amount,
                        BuyerUserId = request.UserId
                    });
                    saleOffer.Amount -= request.Amount;

                    await _purchaseDataManager.Delete(request);
                }

            }

            return response;
        }

    }
}
