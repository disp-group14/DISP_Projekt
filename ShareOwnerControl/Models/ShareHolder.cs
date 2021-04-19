using System.Collections.Generic;

namespace ShareOwnerControl.Models
{
    public class ShareHolder : ModelBase
    {
        public string Name { get; set; }
        public int PortfolioValue { get; set; }
        public int Balance { get; set; }
        public List<Holding> Holdings { get; set; }
    }
}