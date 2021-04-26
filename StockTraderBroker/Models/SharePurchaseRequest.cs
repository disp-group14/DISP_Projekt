using System.Collections.Generic;

namespace StockTraderBroker.Models
{
    public class SharePurchaseRequest
    {
        public List<int> ShareId { get; set; }
        public int ShareOwnerId { get; set; }
    }
}