using System;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using ShareBrokerServiceGrpc.Protos;
using static SalesServiceGrpc.Protos.ISalesService;
using static PurchaseServiceGrpc.Protos.IPurchaseService;
using static ShareBrokerServiceGrpc.Protos.IShareBrokerService;
using static TransactionService.Protos.ITransactionService;
using PurchaseServiceGrpc.Protos;
using TransactionService.Protos;
using SalesServiceGrpc.Protos;

namespace ShareBrokerService.SAL
{
    public class ShareBrokerServiceManager : IShareBrokerServiceBase
    {
        private readonly ISalesServiceClient salesService;
        private readonly IPurchaseServiceClient purchaseService;
        private readonly ITransactionServiceClient transactionService;

        public ShareBrokerServiceManager(ISalesServiceClient salesService, IPurchaseServiceClient purchaseService, ITransactionServiceClient transactionService)
        {
            this.salesService = salesService;
            this.purchaseService = purchaseService;
            this.transactionService = transactionService;
        }

        public override async Task<OfferResponse> SellShare(OfferRequest request, ServerCallContext context)
        {
            // Find a matching purchase offer in PurchaseService
            var purchaseResponse = await purchaseService.FindMatchAsync(new SaleOffer()
            {
                StockId = request.StockId,
                Amount = request.Amount,
                Price = request.Price
            });

            if (purchaseResponse.PurchaseRequests.Count > 0)
            {
                // If a match was found, 
                return await this.performTransaction(request, purchaseResponse);
            }
            else
            {
                // If no matches were found, return a recipt
                return new OfferResponse()
                {
                    Registration = new OfferRegistration()
                    {
                        Message = "We've succesfully registered your offer. Whenver a buyer matches your offer, a trade will be conducted"
                    }
                };
            }
        }

        public override async Task<OfferResponse> PurchaseShare(OfferRequest request, ServerCallContext context)
        {
            // Find a matching sale offer in salesService
            var salesResponse = await salesService.FindMatchAsync(new PurchaseOffer()
            {
                StockId = request.StockId,
                Amount = request.Amount,
                Price = request.Price
            });

            if (salesResponse.SaleRequests.Count > 0)
            {
                // If a match was found, perform transaction
                return await performTransaction(request, salesResponse);

            }
            else
            {
                // If no matches were found, return registration notice
                return new OfferResponse()
                {
                    Registration = new OfferRegistration()
                    {
                        Message = "We've succesfully registered your offer. Whenver a seller matches your offer, a trade will be conducted"
                    }
                };
            }
        }

        private async Task<OfferResponse> performTransaction(OfferRequest offerRequest, SaleRequestList saleRequestList)
        {
            // Setup Transaction Request
            float totalAmountPaid = 0.0F;
            foreach (var saleRequest in saleRequestList.SaleRequests)
            {
                var transactionRequest = new TransactionRequest()
                {
                    StockId = saleRequest.StockId,
                    ShareAmount = saleRequest.Amount,
                    SharePrice = saleRequest.Price,
                    SellerUserId = saleRequest.SellerUserId,
                    BuyerUserId = offerRequest.UserId
                };

                var transactionResponse = await transactionService.PerformTransactionAsync(transactionRequest);
                // Add up total
                totalAmountPaid += transactionResponse.TotalPrice;
            }

            return new OfferResponse()
            {
                Receipt = new OfferReceipt()
                {
                    StockId = offerRequest.StockId,
                    Amount = offerRequest.Amount,
                    Price = totalAmountPaid
                }
            };
        }

        private async Task<OfferResponse> performTransaction(OfferRequest offerRequest, PurchaseRequestList purchaseRequests)
        {
            float totalAmountPaid = 0.0F;
            foreach (var purchaseRequest in purchaseRequests.PurchaseRequests)
            {
                var transactionRequest = new TransactionRequest()
                {
                    StockId = purchaseRequest.StockId,
                    ShareAmount = purchaseRequest.Amount,
                    SharePrice = purchaseRequest.Price,
                    SellerUserId = offerRequest.UserId,
                    BuyerUserId = purchaseRequest.BuyerUserId
                };

                var transactionResponse = await transactionService.PerformTransactionAsync(transactionRequest);
                // Add up total
                totalAmountPaid += transactionResponse.TotalPrice;
            }


            return new OfferResponse()
            {
                Receipt = new OfferReceipt()
                {
                    StockId = offerRequest.StockId,
                    Amount = offerRequest.Amount,
                    Price = totalAmountPaid
                }
            };
        }
    }
}