using System.Collections.Generic;

namespace ShareOwnerControl.Models
{
    public class Share : ModelBase
    {
        public Stock Stock { get; set; }
        public Holding Holding { get; set; }
        public int Value { get; set; }
        public bool ForSale { get; set; }
    }
}