using System;
using System.Linq;
using System.Threading.Tasks;
using BankServiceGrpc.Protos;
using Grpc.Core;
using UserService.DAL;
using UserService.Models;
using UserServiceGrpc.Protos;
using static BankServiceGrpc.Protos.BankService;
using static UserServiceGrpc.Protos.IUserService;
using User = UserServiceGrpc.Protos.User;

namespace UserService.SAL
{
    public class UserServiceManager : IUserServiceBase
    {
        private readonly IUserDataManager userDataManager;

        public UserServiceManager(IUserDataManager userDataManager)
        {
            this.userDataManager = userDataManager;
        }

        public override async Task<User> GetUser(UserRequest request, ServerCallContext context)
        {
            var dbEntity = (await userDataManager.Get(user => user.Id == request.UserId)).FirstOrDefault();
            return (
            dbEntity != null
            ? new User()
            {
                UserId = dbEntity.Id,
                Username = dbEntity.Username
            }
            : null
            );
        }
    }
}