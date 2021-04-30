using System;
using System.Threading.Tasks;
using Grpc.Core;
using ShareBrokerServiceGrpc.Protos;
using static ShareBrokerServiceGrpc.Protos.IShareBrokerService;

namespace ShareBrokerService.SAL {
    public class ShareBrokerServiceManager : IShareBrokerServiceBase 
    {
        public ShareBrokerServiceManager()
        {
            
        }

        public override Task<OfferResponse> SellShare(OfferRequest request, ServerCallContext context ) {
            throw new NotImplementedException("Suck my ass");
        }

        public override Task<OfferResponse> PurchaseShare(OfferRequest request, ServerCallContext context ) {
            throw new NotImplementedException("Suck my other ass");
        }
    }
}