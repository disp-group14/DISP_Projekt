using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.PurchaseService;
using Client.SaleService;
using Client.StockService;
using Client.UserService;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class InterfaceController : ControllerBase
    {
        private readonly UserClient _userClient;
        private readonly PurchaseClient _purchaseClient;
        private readonly SalesClient _salesClient;
        private readonly StockClient _stockClient;

        public InterfaceController(UserClient userClient, PurchaseClient purchaseClient, SalesClient salesClient, StockClient stockClient)
        {
            _userClient = userClient;
            _purchaseClient = purchaseClient;
            _salesClient = salesClient;
            _stockClient = stockClient;
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

        [HttpGet("Shares")]
        public async Task<List<Stock>> GetListOfShares()
        {
            return (await _stockClient.GetAllAsync()).ToList();
        }
    }
}