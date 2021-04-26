using ShareOwnerControl.BLL.Base;
using ShareOwnerControl.DAL;
using ShareOwnerControl.Models;

namespace ShareOwnerControl.BLL
{
    public interface IStockLogic : ILogicBase<Stock>
    {

    }
    public class StockLogic : LogicBase<Stock>, IStockLogic
    {
        public StockLogic(IStockDataManager dataManager): base(dataManager)
        {
            
        }
    }
}