using System.Threading.Tasks;
using System.Linq;
using Grpc.Core;
using OwnershipService.DAL;
using OwnershipServiceGrpc.Protos;
using SharedGrpc.Protos;
using static OwnershipServiceGrpc.Protos.IOwnershipService;
using ShareGrpc = SharedGrpc.Protos.Share;
using ShareHolderGrpc = OwnershipServiceGrpc.Protos.ShareHolder;
using ShareHolderModel = OwnershipService.Models.ShareHolder;
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
            var shareHolder = await shareholderDataManager.Insert(new ShareHolderModel() {UserId = request.UserId});
            return new Empty();
        }

        public override async Task<ShareHolderResponse> GetShareHolder(ShareHolderRequest request, ServerCallContext context) {
            // Get shareholder(s) from db
            var shareHolder = await shareholderDataManager.GetOne(holder => holder.UserId == request.UserId && holder.StockId == request.StockId);

            // Initialize Response
            var shareHolderResponse = new ShareHolderResponse(){
                ShareHolder = new ShareHolderGrpc(){
                    UserId = shareHolder.UserId
                }
            };
            shareHolderResponse.ShareHolder.Shares.AddRange(shareHolder.Shares.Select(shareModel => new ShareGrpc(){
                // I give up...
            }));



            return shareHolderResponse;
        }

        
    }
}