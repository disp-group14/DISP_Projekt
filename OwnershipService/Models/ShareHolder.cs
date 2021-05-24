using System.Collections.Generic;

namespace OwnershipService.Models 
{
    public class ShareHolder : ModelBase
    {
        public int UserId { get; set; }
        public int StockId { get; set; }
        public List<Share> Shares { get; set; }
    }
}