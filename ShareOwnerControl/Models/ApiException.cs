namespace ShareOwnerControl.Models
{
    public class ApiException : System.Exception
    {
        public ApiException(string message) : base(message)
        {
        }

        public ApiException(string message, object entity) : base(message)
        {
            this.Entity = entity;
        }

        public object Entity { get; set; }
    }
}