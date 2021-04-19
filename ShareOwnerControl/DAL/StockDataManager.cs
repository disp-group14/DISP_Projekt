using ShareOwnerControl.DAL.Base;
using ShareOwnerControl.DAL.Context;
using ShareOwnerControl.Models;

namespace ShareOwnerControl.DAL
{
    public interface IStockDataManager : IDataManagerBase<Stock>
    {

    }

    public class StockDataManager : DataManagerBase<Stock> , IStockDataManager
    {
        public StockDataManager(ShareOwnerControlDbContext dataContext) : base(dataContext)
        {

        }
    }
}