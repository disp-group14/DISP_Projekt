using TaxService.DAL.Base;
using UserService.Models;

namespace UserService.DAL
{
    public interface IUserDataManager : IDataManagerBase<User>
    {

    }
    
    public class UserDataManager : DataManagerBase<User>, IUserDataManager
    {
        public UserDataManager(UserServiceDbContext dataContext) : base(dataContext)
        {

        }
    }
}