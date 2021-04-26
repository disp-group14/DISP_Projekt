using ShareOwnerControl.DAL.Base;
using ShareOwnerControl.DAL.Context;
using ShareOwnerControl.Models;

namespace ShareOwnerControl.DAL
{
    public interface IShareDataManager : IDataManagerBase<Share>
    {

    }

    public class ShareDataManager : DataManagerBase<Share> , IShareDataManager
    {
        public ShareDataManager(ShareOwnerControlDbContext dataContext) : base(dataContext)
        {

        }
    }
}