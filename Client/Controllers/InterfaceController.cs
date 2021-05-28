using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.PurchaseService;
using Client.SaleService;
using Client.StockService;
using Client.UserService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Client.Controllers
{
    public class InterfaceController : ControllerBase
    {
        private readonly UserClient _userClient;
        private readonly PurchaseClient _purchaseClient;
        private readonly SalesClient _salesClient;
        private readonly StockClient _stockClient;

        public InterfaceController(UserClient userClient,
                                   PurchaseClient purchaseClient,
                                   SalesClient salesClient,
                                   StockClient stockClient,
                                   IConfiguration configuration)
        {
            _userClient = userClient;
            _userClient.BaseUrl = configuration.GetValue<string>("UserServiceHttp1Uri");

            _purchaseClient = purchaseClient;
            _purchaseClient.BaseUrl = configuration.GetValue<string>("PurchaseServiceHttp1Uri");

            _salesClient = salesClient;
            _salesClient.BaseUrl = configuration.GetValue<string>("SalesServiceHttp1Uri");

            _stockClient = stockClient;
            _stockClient.BaseUrl = configuration.GetValue<string>("StockServiceUri");
        }

        [HttpPost("User/Create")]
        public async Task<User> CreateUser(string username, string password)
        {
            return await _userClient.CreateAsync(username, password);
        }

        [HttpPost("User/Login")]
        public async Task<User> Login(string username, string password)
        {
            return await _userClient.LoginAsync(username, password);
        }

        [HttpPost("User/Update")]
        public async Task<User> UpdateUser(int userId, string password, string newPassword, string newUsername )
        {
            return await _userClient.UpdateAsync(userId, password, newPassword, newUsername);
        }

        [HttpPost("User/Delete")]
        public async Task DeleteUser(int userId)
        {
            await _userClient.DeleteAsync(userId);
        }

        [HttpPost("Share/Purchase")]
        public async Task<PurchaseService.OfferResponse> PurchaseShare(int stockId, int amount, float price, int userId)
        {
            var purchaseRequest = new PurchaseRequest()
            {
                StockId = stockId,
                Amount = amount,
                Price = price,
                UserId = userId
            };
            return await _purchaseClient.PostAsync(purchaseRequest);
        }

        [HttpPost("Share/Sale")]
        public async Task<SaleService.OfferResponse> SalePurchase(int stockId, int amount, float price, int userId)
        {
            var saleRequest = new SaleRequest()
            {
                StockId = stockId,
                Amount = amount,
                Price = price,
                UserId = userId
            };

            return await _salesClient.PostAsync(saleRequest);
        }

        [HttpGet("Stock")]
        public async Task<List<Stock>> GetListOfSStocks()
        {
            return (await _stockClient.GetAllAsync()).ToList();
        }

        [HttpGet("Share/Sale")]
        public async Task<List<SaleService.SaleRequest>> GetListOfShareSales()
        {
            return (await _salesClient.GetAsync()).ToList();
        }

        [HttpGet("Share/Purchase")]
        public async Task<List<PurchaseService.PurchaseRequest>> GetListOfSharePurchases()
        {
            return (await _purchaseClient.GetAsync()).ToList();
        }
    }
}