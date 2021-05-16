using System.Threading.Tasks;
using Grpc.Core;
using OwnershipService.DAL;
using OwnershipService.Models;
using OwnershipServiceGrpc.Protos;
using SharedGrpc.Protos;
using static OwnershipServiceGrpc.Protos.IOwnershipService;

namespace OwnershipService.SAL
{
    public class OwnershipServiceManager : IOwnershipServiceBase
    {
        private readonly IShareHolderDataManager shareholderDataManager;

        public OwnershipServiceManager(IShareHolderDataManager shareholderDataManager)
        {
            this.shareholderDataManager = shareholderDataManager;
        }

        public override async Task<Empty> RegisterUser(UserRegistrationRequest request, ServerCallContext context)
        {
            var shareHolder = await shareholderDataManager.Insert(new ShareHolder() {UserId = request.UserId});
            return new Empty();
        }
    }
}