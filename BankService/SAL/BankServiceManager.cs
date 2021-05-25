using System;
using System.Threading.Tasks;
using BankService.DAL;
using BankService.Models;
using BankServiceGrpc.Protos;
using Grpc.Core;
using SharedGrpc.Protos;
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

        public override async Task<AccountInfo> Withdraw(TransferRequest request, ServerCallContext context)
        {
            var account = await accountDataManager.GetOne(account => account.UserId == request.UserId);
            account.Balance -= request.Amount;

            await accountDataManager.Update(account);

            return new AccountInfo()
            {
                UserId = account.UserId,
                Balance = account.Balance
            };
        }

        public override async Task<AccountInfo> Deposit(TransferRequest request, ServerCallContext context)
        {
            var account = await accountDataManager.GetOne(account => account.UserId == request.UserId);
            account.Balance += request.Amount;

            await accountDataManager.Update(account);

            return new AccountInfo()
            {
                UserId = account.UserId,
                Balance = account.Balance
            };
        }

        public override async Task<AccountInfo> RegisterUser(UserRegistrationRequest request, ServerCallContext context)
        {
            var account = await accountDataManager.Insert(new Account() { UserId = request.UserId, Balance = 0 });
            return new AccountInfo()
            {
                UserId = account.UserId,
                Balance = account.Balance
            };
        }

    }
}