using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using StockTraderBroker.Models;
using TestClient;
using static TestClient.TestService;
using SharePurchaseReceipt = StockTraderBroker.Models.SharePurchaseReceipt;
using SharePurchaseRequest = StockTraderBroker.Models.SharePurchaseRequest;

namespace StockTraderBroker.Contollers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class StockBrokerController : Controller
    {
        private GrpcChannel channel;
        private TestServiceClient client;
        public StockBrokerController(TestServiceClient client)
        {

        }

        [HttpPost("PurchaseShares")]
        public async Task<SharePurchaseReceipt> RequestSharePurchase(SharePurchaseRequest request)
        {
            var httpHandler = new HttpClientHandler();
            // Return `true` to allow certificates that are untrusted/invalid
            httpHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            channel = GrpcChannel.ForAddress("https://localhost:5001",
                new GrpcChannelOptions { HttpHandler = httpHandler });
                
            client = new TestServiceClient(channel);
            var reply = await client.PurchaseRequestAsync(new TestClient.SharePurchaseRequest() { ShareHolderId = 1 });
            Console.WriteLine($"Id returned: {reply.ShareIds.First()}");
            return null;
        }
    }
}