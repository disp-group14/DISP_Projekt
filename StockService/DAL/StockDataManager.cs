using StockService.DAL.Base;
using StockService.Models;
using UserService.DAL;

namespace StockService.DAL
{
    public interface IStockDataManager : IDataManagerBase<Stock>
    {

    }
    
    public class StockDataManager : DataManagerBase<Stock>, IStockDataManager
    {
        public StockDataManager(StockServiceDbContext dataContext) : base(dataContext)
        {

        }
    }
}