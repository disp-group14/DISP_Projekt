using ShareOwnerControl.DAL.Base;
using ShareOwnerControl.DAL.Context;
using ShareOwnerControl.Models;

namespace ShareOwnerControl.DAL
{
    public interface IHoldingDataManager : IDataManagerBase<Holding>
    {

    }

    public class HoldingDataManager : DataManagerBase<Holding> , IHoldingDataManager
    {
        public HoldingDataManager(ShareOwnerControlDbContext dataContext) : base(dataContext)
        {

        }
    }
}