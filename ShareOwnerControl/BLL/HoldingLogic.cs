using ShareOwnerControl.BLL.Base;
using ShareOwnerControl.DAL;
using ShareOwnerControl.Models;

namespace ShareOwnerControl.BLL
{
    public interface IHoldingLogic : ILogicBase<Holding>
    {

    }
    public class HoldingLogic : LogicBase<Holding>, IHoldingLogic
    {
        public HoldingLogic(IHoldingDataManager dataManager): base(dataManager)
        {
            
        }
    }
}