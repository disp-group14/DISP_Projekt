using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockTraderBroker.Models;

namespace StockTraderBroker.Contollers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class StockBrokerController : Controller
    {
        public StockBrokerController()
        {

        }

        [HttpPost("PurchaseShares")]
        public async Task<SharePurchaseReceipt> RequestSharePurchase(SharePurchaseRequest request)
        {
            return null;
        }
    }
}