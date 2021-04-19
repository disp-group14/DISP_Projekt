using System.Collections.Generic;

namespace ShareOwnerControl.Models
{
    public class Stock : ModelBase
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public List<Share> Shares { get; set; }
        public List<Holding> Holdings { get; set; }
    }
}