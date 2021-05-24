using System;
using System.Threading.Tasks;
using BankServiceGrpc.Protos;
using Grpc.Core;
using OwnershipServiceGrpc.Protos;
using SharedGrpc.Protos;
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

        public async override Task<AccountInfo> PerformTransaction(TransactionRequest request, ServerCallContext context)
        {
            // All rpcs are critical here. If one fails, all should undo changes. E.g. if buyer's BankService throws an error, ownership should not be changed.
            // How is fallback typically implemented in distributed systems?

            // Update ownership
            ChangeOwnershipRequest ownershipRequest = new ChangeOwnershipRequest() {
                NewUserId = request.BuyerUserId
            };

            ownershipRequest.ShareIds.AddRange(request.Shares);
            ShareHolderResponse buyerShareHolder = await this.ownershipServiceClient.ChangeOwnershipAsync(ownershipRequest);

            // Tax transaction
            TaxReceipt taxReceipt = await this.taxServiceClient.TaxTransactionAsync(new TaxRequest() {
                Amount = request.Amount
            });

            // Charge buyer
            AccountInfo buyerAccount = await this.bankServiceClient.WithdrawAsync(new TransferRequest() {
                Amount = taxReceipt.Amount
            });


            // Respond
            return new AccountInfo(buyerAccount);
        }

    }
}