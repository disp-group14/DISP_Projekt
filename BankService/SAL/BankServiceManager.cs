using System;
using System.Threading.Tasks;
using BankServiceGrpc.Protos;
using Grpc.Core;
using static BankServiceGrpc.Protos.BankService;

namespace BankService.SAL
{
    public class BankServiceManager : BankServiceBase
    {
        public BankServiceManager()
        {
        }

        public override Task<TransactionRequest> Transaction(TransactionRequest request, ServerCallContext context)
        {
            return Task.FromResult(new TransactionRequest());
        }
    }
}