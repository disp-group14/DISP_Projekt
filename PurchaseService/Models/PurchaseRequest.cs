namespace PurchaseService.Models
{
    public class PurchaseRequest : ModelBase
    {
        public int StockId { get; set; }
        public int Amount { get; set; }
        public float Price { get; set; }
        public int UserId { get; set; }
    }
}