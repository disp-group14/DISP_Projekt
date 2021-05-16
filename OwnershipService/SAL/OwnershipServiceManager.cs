using System.Threading.Tasks;
using Grpc.Core;
using OwnershipService.DAL;
using OwnershipService.Models;
using OwnershipServiceGrpc.Protos;
using SharedGrpc.Protos;
using static OwnershipServiceGrpc.Protos.IOwnershipService;
using System.Linq;

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

        public override async Task<ShareHolderResponse> GetShareHolder(ShareHolderRequest request, ServerCallContext context) {
            // Get shareholder from db
            var shareHolder = await shareholderDataManager.GetOne(holder => holder.UserId == request.UserId);
            var shareHolderResponse = new ShareHolderResponse() {
                UserId = shareHolder.UserId
            };
            // Convert from dm-model to grpc-model
            shareHolderResponse.Shares.AddRange(shareHolder.Shares.ConvertAll<SharedGrpc.Protos.Share>(share => {
                return new SharedGrpc.Protos.Share(){
                    StockId = share.StockId,
                    Amount = shareHolder.Shares.Count,
                    Price = shareHolder.Shares.Aggregate((float)0, (acc, share) => {
                        return acc + share.Price;
                    })
                };
            }));

            return shareHolderResponse;
        }
    }
}