using SalesService.DAL.Base;
using SalesService.DAL.Context;
using SalesService.Models;

namespace SalesService.DAL 
{
    public interface ISaleRequestDataManger : IDataManagerBase<SaleRequest> {}

    public class SaleRequestDataManager : DataManagerBase<SaleRequest>, ISaleRequestDataManger
    {
        public SaleRequestDataManager(SalesServiceControlDbContext dataContext) : base (dataContext)
        {
            
        }
    }
}
