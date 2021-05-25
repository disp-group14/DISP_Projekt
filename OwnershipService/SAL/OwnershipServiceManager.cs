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
        private readonly IShareHolderDataManager _shareholderDataManager;
        private readonly IShareDataManager _shareDataManager;

        public OwnershipServiceManager(IShareHolderDataManager shareholderDataManager, IShareDataManager shareDataManager)
        {
            _shareholderDataManager = shareholderDataManager;
            _shareDataManager = shareDataManager;
        }

        public override async Task<Empty> RegisterUser(UserRegistrationRequest request, ServerCallContext context)
        {
            var shareHolder = await _shareholderDataManager.Insert(new ShareHolderModel() { UserId = request.UserId });
            return new Empty();
        }

        public override async Task<Empty> ChangeOwnership(ChangeOwnershipRequest request, ServerCallContext context)
        {
            // Get shares with stock id and owned by seller
            var shares = await _shareDataManager.Get(share => share.ShareHolder.Id == request.OldUserId && share.StockId == request.StockId);

            // Get db tracked shareholder to transfer ownership
            var newShareHolder = await _shareholderDataManager.GetOne(shareHolder => shareHolder.Id == request.NewUserId);
            
            // Loop for amount of shares which need to have ownership transfered
            for(int index = 0; index < request.Amount; index++)
            {
                shares[index].ShareHolder = newShareHolder;
                await _shareDataManager.Update(shares[index]);
            }
    
            return new Empty();
        }

        public override async Task<SharesInStockResponse> GetSharesInStock(SharesInStockRequest request, ServerCallContext context)
        {
            // Get shareholder(s) from db
            var shareHolder = await _shareholderDataManager.GetOne(
                holder => holder.UserId == request.UserId,
                includeProperties: new string[] { nameof(ShareHolder.Shares) });

            var response = new SharesInStockResponse();
            response.Shares.AddRange(shareHolder.Shares.Where(share => share.StockId == request.StockId).Select(shareModel => new ShareGrpc() {
                Id = shareModel.Id,
                StockId = shareModel.StockId
            }));

            return response;
        }


    }
}