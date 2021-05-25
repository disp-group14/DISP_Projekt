using System.Linq;
using System.Threading.Tasks;
using BankServiceGrpc.Protos;
using Microsoft.AspNetCore.Mvc;
using UserService.DAL;
using UserService.Models;
using static BankServiceGrpc.Protos.IBankService;
using static OwnershipServiceGrpc.Protos.IOwnershipService;

namespace UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IUserDataManager userDataManager;
        private readonly IBankServiceClient bankServiceClient;
        private readonly IOwnershipServiceClient ownershipServiceClient;

        public UserController(IUserDataManager userDataManager, IBankServiceClient bankServiceClient, IOwnershipServiceClient ownershipServiceClient)
        {
            this.userDataManager = userDataManager;
            this.bankServiceClient = bankServiceClient;
            this.ownershipServiceClient = ownershipServiceClient;
        }

        [HttpPost("Create")]
        public async Task<User> Create(string username, string password)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                // Create user in db
                var user = await userDataManager.Insert(new User() { Username = username, Password = password });

                // Register user with bank service
                await bankServiceClient.RegisterUserAsync(new SharedGrpc.Protos.UserRegistrationRequest() { UserId = user.Id });

                // Register user with ownership service
                await ownershipServiceClient.RegisterUserAsync(new SharedGrpc.Protos.UserRegistrationRequest() { UserId = user.Id });

                return user;
            }
            else 
            {
                throw new ApiException("Username or password not set");
            }

        }

        [HttpPost("Update")]
        public async Task<User> Update(int userId, string password, string newPassword, string newUsername)
        {
            // Check if user exists and have given the correct password
            var user = (await userDataManager.Get(user => user.Id == userId && user.Password == password)).FirstOrDefault();

            // Update if user found
            if (user != null)
            {
                user.Username = newUsername;
                user.Password = newPassword;
                await userDataManager.Update(user);
                return user;
            }
            else
            {
                // If user not found throw error
                throw new ApiException("User does not exist or password does not match the userId");
            }

        }

        [HttpPost("Login")]
        public async Task<User> Login(string username, string password)
        {
            // return user on login
            return (await userDataManager.Get(user => user.Username == username && user.Password == password)).FirstOrDefault();
        }

        [HttpDelete("Delete")]
        public async Task DeleteAsync(int userId)
        {
            // Simple delete
            await userDataManager.Delete(new User() { Id = userId });

            // TODO: How to handle user deletion in other services?
        }
    }
}