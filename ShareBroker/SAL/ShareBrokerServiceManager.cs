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
using SharedGrpc.Protos;
using Google.Protobuf.Collections;
using TransactionService.Protos;

namespace ShareBrokerService.SAL {
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

        public override async Task<OfferResponse> SellShare(OfferRequest request, ServerCallContext context ) {
            // Find a matching purchase offer in PurchaseService
            var purchaseResponse = await purchaseService.FindMatchAsync(new SaleOffer() {
                StockId = request.StockId,
                Amount = request.Amount,
                Price = request.Price
            });

            if (purchaseResponse.Matches.Count > 0) {
                // If a match was found, 
                return await this.performTransaction(request, purchaseResponse); // OBS!!!!! Her bliver request's sælger smidt med som køber (gennem request UserId)...
                // Hvor skal BuyerUserId komme fra her? Burde det sendes tilbage fra purchaseService.FindMatchAsync? Burde b¨de sælger og køber's id indgå i hver handel?
            } else {
                // If no matches were found, return a recipt
                return new OfferResponse(){
                    Registration = new OfferRegistration(){
                        Message = "We've succesfully registered your offer. Whenver a buyer matches your offer, a trade will be conducted"
                    }
                };
            }
        }

        public override async Task<OfferResponse> PurchaseShare(OfferRequest request, ServerCallContext context ) {
            // Find a matching sale offer in salesService
            var salesSesponse = await salesService.FindMatchAsync(new SalesServiceGrpc.Protos.PurchaseOffer() {
                StockId = request.StockId,
                Amount = request.Amount,
                Price = request.Price
            });

            if (salesSesponse.Matches.Count > 0) {
                // If a match was found, perform transaction
                return await this.performTransaction(request, salesSesponse);

            } else {
                // If no matches were found, return registration notice
                return new OfferResponse(){
                    Registration = new OfferRegistration(){
                        Message = "We've succesfully registered your offer. Whenver a seller matches your offer, a trade will be conducted"
                    }
                };
            } 
        }

        private async Task<OfferResponse> performTransaction(OfferRequest request, MatchResponse matchResponse) {
            // Calculate price
            float transactionPrice = matchResponse.Matches.Aggregate((float)0, (acc, share) => share.Price + acc);

            // Setup Transaction Request
            TransactionRequest transactionRequest = new TransactionRequest() {
                BuyerUserId = request.UserId,
                Amount = transactionPrice
            };
            transactionRequest.ShareIds.AddRange(matchResponse.Matches.Select(share => share.Id));

            // Utilize Transaction service to perform transaction
            var transactionResponse = await this.transactionService.PerformTransactionAsync(transactionRequest);

            // Return OfferResponse
            return new OfferResponse(){
                Receipt = new OfferReceipt() {
                    StockId = request.StockId,
                    Amount = matchResponse.Matches.Count,
                    Price = transactionPrice
                }
            };
        }
    }
}