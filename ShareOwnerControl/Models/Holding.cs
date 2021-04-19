using System.Collections.Generic;

namespace ShareOwnerControl.Models
{
    public class Holding : ModelBase
    {
        public Stock Stock { get; set; }
        public ShareHolder ShareHolder { get; set; }
        public List<Share> Shares { get; set; }
    }
}