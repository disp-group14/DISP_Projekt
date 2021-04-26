using ShareOwnerControl.BLL.Base;
using ShareOwnerControl.DAL;
using ShareOwnerControl.Models;

namespace ShareOwnerControl.BLL
{
    public interface IShareHolderLogic : ILogicBase<ShareHolder>
    {

    }
    public class ShareHolderLogic : LogicBase<ShareHolder>, IShareHolderLogic
    {
        public ShareHolderLogic(IShareHolderDataManager dataManager): base(dataManager)
        {
            
        }
    }
}