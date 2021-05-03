
using PurchaseService.DAL.Context;
using PurchaseService.Models;
using SalesService.DAL.Base;

namespace PurchaseService.DAL 
{
    public interface IPurchaseRequestDataManger : IDataManagerBase<PurchaseRequest> {}

    public class PurchaseRequestDataManager : DataManagerBase<PurchaseRequest>, IPurchaseRequestDataManger
    {
        public PurchaseRequestDataManager(PurchaseServiceControlDbContext dataContext) : base (dataContext)
        {
            
        }
    }
}
