using System;
using System.Threading.Tasks;
using Grpc.Core;
using ShareBrokerServiceGrpc.Protos;
using static SalesServiceGrpc.Protos.ISalesService;
using static PurchaseServiceGrpc.Protos.IPurchaseService;
using static ShareBrokerServiceGrpc.Protos.IShareBrokerService;
using PurchaseServiceGrpc.Protos;
using Google.Protobuf.Collections;

namespace ShareBrokerService.SAL {
    public class ShareBrokerServiceManager : IShareBrokerServiceBase 
    {
        private readonly ISalesServiceClient salesService;
        private readonly IPurchaseServiceClient purchaseService;

        public ShareBrokerServiceManager(ISalesServiceClient salesService, IPurchaseServiceClient purchaseService)
        {
            this.salesService = salesService;
            this.purchaseService = purchaseService;
        }


        private float sumMatchesPrice(RepeatedField<Share> matches) {
            float accPrice = 0;
            for (var i = 0; i < matches.Count; ++i) {
                accPrice += matches[i].Price;
            }
            return accPrice;
        }
        public override async Task<OfferResponse> SellShare(OfferRequest request, ServerCallContext context ) {
            var response = purchaseService.FindMatch(new SaleOffer() {
                StockId = request.StockId,
                Amount = request.Amount,
                Price = request.Price
            });
            if (response.Matches.Count > 0) {
                return new OfferResponse(){
                    Receipt = new OfferReceipt() {
                        StockId = request.StockId,
                        Amount = response.Matches.Count,
                        Price = sumMatchesPrice(response.Matches)
                    }
                };
            } else {
                return new OfferResponse(){
                    Registration = new OfferRegistration(){
                        Message = "We've succesfully registered your offer. Whenver a buyer matches your offer, a trade will be conducted"
                    }
                };
            }
        }

        public override Task<OfferResponse> PurchaseShare(OfferRequest request, ServerCallContext context ) {
            throw new NotImplementedException("Suck my other ass");
        }
    }
}