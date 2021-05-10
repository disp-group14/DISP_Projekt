using OwnershipService.DAL.Base;
using OwnershipService.Models;
using UserService.DAL;

namespace OwnershipService.DAL
{
    public interface IShareHolderDataManager : IDataManagerBase<ShareHolder>
    {

    }
    
    public class ShareHolderDataManager : DataManagerBase<ShareHolder>, IShareHolderDataManager
    {
        public ShareHolderDataManager(OwnershipServiceDbContext dataContext) : base(dataContext)
        {

        }
    }
}