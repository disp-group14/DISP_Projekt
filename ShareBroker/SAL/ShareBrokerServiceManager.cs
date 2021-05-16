using System;
using System.Threading.Tasks;
using Grpc.Core;
using ShareBrokerServiceGrpc.Protos;
using static SalesServiceGrpc.Protos.ISalesService;
using static PurchaseServiceGrpc.Protos.IPurchaseService;
using static ShareBrokerServiceGrpc.Protos.IShareBrokerService;
using PurchaseServiceGrpc.Protos;
using SharedGrpc.Protos;
using Google.Protobuf.Collections;
using static TransactionService.Proto.ITransactionService;
using TransactionService.Proto;

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


        private float sumMatchesPrice(RepeatedField<Share> matches) {
            float accPrice = 0;
            for (var i = 0; i < matches.Count; ++i) {
                accPrice += matches[i].Price;
            }
            return accPrice;
        }
        public override async Task<OfferResponse> SellShare(OfferRequest request, ServerCallContext context ) {
            // Find a matching purchase offer in PurchaseService
            var response = await purchaseService.FindMatchAsync(new SaleOffer() {
                StockId = request.StockId,
                Amount = request.Amount,
                Price = request.Price
            });

            if (response.Matches.Count > 0) {
                // If a match was found, 
                return new OfferResponse(){
                    Receipt = new OfferReceipt() {
                        StockId = request.StockId,
                        Amount = response.Matches.Count,
                        Price = sumMatchesPrice(response.Matches)
                    }
                };
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
            var response = await salesService.FindMatchAsync(new SalesServiceGrpc.Protos.PurchaseOffer() {
                StockId = request.StockId,
                Amount = request.Amount,
                Price = request.Price
            });

            if (response.Matches.Count > 0) {
                // If a match was found, perform transaction

                var transactionResponse = await this.transactionService.PerformTransactionAsync(new TransactionRequest() {});


                return new OfferResponse(){
                    Receipt = new OfferReceipt() {
                        StockId = request.StockId,
                        Amount = response.Matches.Count,
                        Price = sumMatchesPrice(response.Matches)
                    }
                };
            } // If no matches were found, return registration notice
                return new OfferResponse(){
                    Registration = new OfferRegistration(){
                        Message = "We've succesfully registered your offer. Whenver a seller matches your offer, a trade will be conducted"
                    }
                };
        }
    }
}