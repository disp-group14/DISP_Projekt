using OwnershipService.DAL.Base;
using OwnershipService.Models;
using UserService.DAL;

namespace OwnershipService.DAL
{
    public interface IShareDataManager : IDataManagerBase<Share>
    {

    }
    
    public class ShareDataManager : DataManagerBase<Share>, IShareDataManager
    {
        public ShareDataManager(OwnershipServiceDbContext dataContext) : base(dataContext)
        {

        }
    }
}