namespace OwnershipService.Models
{
    public class Share : ModelBase
    {
        public float Price { get; set; } 
        public int StockId { get; set; }
        public ShareHolder ShareHolder { get; set; }
    }
}