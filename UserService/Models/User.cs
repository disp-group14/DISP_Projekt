using UserService.Models;

namespace UserService.Models
{
    public class User : ModelBase
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}