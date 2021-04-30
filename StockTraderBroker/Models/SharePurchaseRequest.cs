using System.Collections.Generic;

namespace StockTraderBroker.Models
{
    public class SharePurchaseRequest
    {
        public List<int> ShareIds { get; set; }
        public int ShareOwnerId { get; set; }
    }
}