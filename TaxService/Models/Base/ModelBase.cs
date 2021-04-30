using System;

namespace TaxService.Models
{
    public abstract class ModelBase
    {
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public int Id { get; set; }
    }
}