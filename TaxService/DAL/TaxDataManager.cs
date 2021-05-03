using TaxService.DAL.Base;
using TaxService.Models;

namespace TaxService.DAL
{
    public interface ITaxDataManager : IDataManagerBase<Tax> 
    {

    }
    public class TaxDataManager :  DataManagerBase<Tax>, ITaxDataManager
    {
        public TaxDataManager(TaxServiceDbContext dataContext) : base(dataContext)
        {

        }
    }
}