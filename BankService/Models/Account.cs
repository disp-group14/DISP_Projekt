namespace BankService.Models
{
    public class Account : ModelBase
    {
        public int UserId { get; set; }
        public int Balance { get; set; }
    }
}