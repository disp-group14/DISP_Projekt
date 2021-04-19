using ShareOwnerControl.DAL.Base;
using ShareOwnerControl.DAL.Context;
using ShareOwnerControl.Models;

namespace ShareOwnerControl.DAL
{
    public interface IShareHolderDataManager : IDataManagerBase<ShareHolder>
    {

    }

    public class ShareHolderDataManager : DataManagerBase<ShareHolder> , IShareHolderDataManager
    {
        public ShareHolderDataManager(ShareOwnerControlDbContext dataContext) : base(dataContext)
        {

        }
    }
}