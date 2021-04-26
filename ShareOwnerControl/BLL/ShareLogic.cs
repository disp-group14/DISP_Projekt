using ShareOwnerControl.BLL.Base;
using ShareOwnerControl.DAL;
using ShareOwnerControl.Models;

namespace ShareOwnerControl.BLL
{
    public interface IShareLogic : ILogicBase<Share>
    {

    }
    public class ShareLogic : LogicBase<Share>, IShareLogic
    {
        public ShareLogic(IShareDataManager dataManager): base(dataManager)
        {
            
        }
    }
}