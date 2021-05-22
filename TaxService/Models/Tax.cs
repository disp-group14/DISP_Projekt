namespace TaxService.Models
{
    public class Tax : ModelBase
    {
        public float Amount { get; set; }
        public float Tax { get; set; }
        public float Percentage {get; set;}
    }
}