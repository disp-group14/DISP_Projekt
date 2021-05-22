using System;
using System.Threading.Tasks;
using Grpc.Core;
using OwnershipServiceGrpc.Protos;
using TaxServiceGrpc.Protos;
using TransactionService.Protos;
using static BankServiceGrpc.Protos.IBankService;
using static OwnershipServiceGrpc.Protos.IOwnershipService;
using static TaxServiceGrpc.Protos.ITaxService;
using static TransactionService.Protos.ITransactionService;

namespace TransactionService.SAL
{
    public class TransactionServiceManager : ITransactionServiceBase
    {
        private readonly IBankServiceClient bankServiceClient;
        private readonly IOwnershipServiceClient ownershipServiceClient;
        private readonly ITaxServiceClient taxServiceClient;

        public TransactionServiceManager(IBankServiceClient bankServiceClient, IOwnershipServiceClient ownershipServiceClient, ITaxServiceClient taxServiceClient)
        {
            this.bankServiceClient = bankServiceClient;
            this.ownershipServiceClient = ownershipServiceClient;
            this.taxServiceClient = taxServiceClient;
        }

        public async override Task<TransactionReceipt> PerformTransaction(TransactionRequest request, ServerCallContext context)
        {
            // All rpcs are critical here. If one fails, all should undo changes. E.g. if buyer's BankService throws an error, ownership should not be changed.
            // How is fallback typically implemented in distributed systems?


            // Tax transaction
            TaxReceipt tax = await this.taxServiceClient.TaxTransactionAsync(new TaxRequest() {
                Amount = request.Amount
            });


            // Update ownership
            ChangeOwnershipRequest ownershipRequest = new ChangeOwnershipRequest() {
                NewUserId = request.BuyerUserId
            };
            ownershipRequest.ShareIds.AddRange(request.ShareIds);

            ShareHolderResponse buyerShareHolder = await this.ownershipServiceClient.ChangeOwnershipAsync(ownershipRequest);




            return await base.PerformTransaction(request, context);
        }

    }
}