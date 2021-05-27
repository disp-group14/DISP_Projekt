namespace OwnershipService.Models
{
    public class Share : ModelBase
    {
        public float PurchasePrice { get; set; } 
        public int StockId { get; set; }
        public ShareHolder ShareHolder { get; set; }
        public int ShareHolderId { get; set; }
    }
}