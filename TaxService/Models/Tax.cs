namespace TaxService.Models
{
    public class Tax : ModelBase
    {
        public int Amount { get; set; }
        public int TaxPaid { get; set; }
    }
}