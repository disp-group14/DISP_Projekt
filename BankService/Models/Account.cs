namespace BankService.Models
{
    public class Account : ModelBase
    {
        public int UserId { get; set; }
        public float Balance { get; set; }
    }
}