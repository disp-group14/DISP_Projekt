using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using SalesService.DAL;
using SalesService.Models;
using SalesServiceGrpc.Protos;
using SharedGrpc.Protos;
using static SalesServiceGrpc.Protos.ISalesService;
using SaleRequestGrpc = SalesServiceGrpc.Protos.SaleRequest;

namespace SalesService.SAL
{
    public class SalesServiceManager : ISalesServiceBase
    {
        private readonly ISaleRequestDataManger _saleDataManager;

        public SalesServiceManager(ISaleRequestDataManger saleDataManager)
        {
            _saleDataManager = saleDataManager;
        }

        public override async Task<SaleRequestList> FindMatch(PurchaseOffer purchaseOffer, ServerCallContext context)
        {
            var response = new SaleRequestList();
            // Get All purchase requests where stockId matches and price is lower or equal to offer.
            var saleRequests = (await this._saleDataManager.Get(saleRequest => saleRequest.StockId == purchaseOffer.StockId
            && saleRequest.Price <= purchaseOffer.Price)).OrderBy(share => share.Price);

            // Loop through purchase requests 
            for (int index = 0; index < saleRequests.Count(); index++)
            {
                // Convert to single object for easier use in logic
                var request = saleRequests.ToList()[index];

                // If amount matches exactly
                if (request.Amount == purchaseOffer.Amount)
                {
                    response.SaleRequests.Add(new SaleRequestGrpc()
                    {
                        Price = request.Price,
                        StockId = request.StockId,
                        Amount = request.Amount,
                        SellerUserId = request.UserId
                    });
                    await _saleDataManager.Delete(request);
                    break;
                }
                // purchase request amount is higher than saleOffer
                else if (request.Amount > purchaseOffer.Amount)
                {
                    response.SaleRequests.Add(new SaleRequestGrpc()
                    {
                        Price = request.Price,
                        StockId = request.StockId,
                        Amount = purchaseOffer.Amount,
                        SellerUserId = request.UserId
                    });

                    request.Amount -= purchaseOffer.Amount;
                    await _saleDataManager.Update(request);

                    break;
                }
                // purchase request amount 
                else if (request.Amount < purchaseOffer.Amount)
                {
                    response.SaleRequests.Add(new SaleRequestGrpc()
                    {
                        Price = request.Price,
                        StockId = request.StockId,
                        Amount = request.Amount,
                        SellerUserId = request.UserId
                    });
                    purchaseOffer.Amount -= request.Amount;

                    await _saleDataManager.Delete(request);
                }

            }

            return response;
        }

    }
}