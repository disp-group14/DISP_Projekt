namespace TaxService.Models
{
    public class Tax : ModelBase
    {
        public float Amount { get; set; }
        public float TaxPaid { get; set; }
        public float Percentage {get; set;}
    }
}