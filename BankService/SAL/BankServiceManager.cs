using System;
using System.Threading.Tasks;
using BankService.DAL;
using BankService.Models;
using BankServiceGrpc.Protos;
using Grpc.Core;
using static BankServiceGrpc.Protos.IBankService;

namespace BankService.SAL
{
    public class BankServiceManager : IBankServiceBase
    {
        private readonly IAccountDataManager accountDataManager;

        public BankServiceManager(IAccountDataManager accountDataManager)
        {
            this.accountDataManager = accountDataManager;
        }

        public override Task<TransactionRequest> Transaction(TransactionRequest request, ServerCallContext context)
        {
            return Task.FromResult(new TransactionRequest());
        }
        public override async Task<AccountInfo> RegisterUser(UserRegistrationRequest request, ServerCallContext context)
        {
            var account = await accountDataManager.Insert(new Account(){UserId = request.UserId, Balance = 0});
            return new AccountInfo(){
                UserId = account.UserId,
                Balance = account.Balance
            };
        }

    }
}