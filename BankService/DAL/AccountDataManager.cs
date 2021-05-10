using BankService.DAL.Base;
using BankService.Models;
using UserService.DAL;

namespace BankService.DAL
{
    public interface IAccountDataManager : IDataManagerBase<Account>
    {

    }
    public class AccountDataManager : DataManagerBase<Account>, IAccountDataManager
    {
        public AccountDataManager(BankServiceDbContext dataContext) : base(dataContext)
        {

        }
    }

}