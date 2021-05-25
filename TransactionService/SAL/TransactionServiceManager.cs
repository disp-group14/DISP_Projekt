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
        private readonly IBankServiceClient _bankServiceClient;
        private readonly IOwnershipServiceClient ownershipServiceClient;
        private readonly ITaxServiceClient taxServiceClient;

        public TransactionServiceManager(IBankServiceClient bankServiceClient, IOwnershipServiceClient ownershipServiceClient, ITaxServiceClient taxServiceClient)
        {
            this._bankServiceClient = bankServiceClient;
            this.ownershipServiceClient = ownershipServiceClient;
            this.taxServiceClient = taxServiceClient;
        }

        public async override Task<TransactionReceipt> PerformTransaction(TransactionRequest request, ServerCallContext context)
        {
            // All rpcs are critical here. If one fails, all should undo changes. E.g. if buyer's BankService throws an error, ownership should not be changed.
            // How is fallback typically implemented in distributed systems?
            // To solve this implement messagequeues for financial transactions.

            // Update ownership
            var changeOwnershipRequest = new ChangeOwnershipRequest()
            {
                NewUserId = request.BuyerUserId,
                OldUserId = request.SellerUserId,
                StockId = request.StockId,
                Amount = request.ShareAmount
            };

            await this.ownershipServiceClient.ChangeOwnershipAsync(changeOwnershipRequest);

            var totalPrice = request.ShareAmount * request.SharePrice;
            // Tax transaction
            var taxReceipt = await this.taxServiceClient.TaxTransactionAsync(new TaxRequest()
            {
                Amount = totalPrice
            });

            // Charge buyer
            await _bankServiceClient.WithdrawAsync(new TransferRequest()
            {
                UserId = request.BuyerUserId,
                Amount = totalPrice
            });

            // Pay seller
            await _bankServiceClient.DepositAsync(new TransferRequest()
            {
                UserId = request.SellerUserId,
                Amount = totalPrice - taxReceipt.TaxToPay
            });

            // Respond
            return new TransactionReceipt(){TotalPrice = totalPrice, TaxPaid = taxReceipt.TaxToPay};
        }

    }
}