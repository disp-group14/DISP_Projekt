using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using ShareOwnerControl.Protos;

namespace ShareOwnerControl.Services
{
    public class GrpcService : TestService.TestServiceBase
    {
        public override Task<SharePurchaseReceipt> PurchaseRequest(SharePurchaseRequest request, ServerCallContext context)
        {
            Console.WriteLine($"Recieved request with shareholder id: {request.ShareHolderId}");
            var reply = new SharePurchaseReceipt();
            reply.ShareIds.Add(2);
            return Task.FromResult(reply);
        }
    }
}