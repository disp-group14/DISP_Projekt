using System.Collections.Generic;

namespace StockTraderBroker.Models
{
    public class SharePurchaseReceipt
    {
        public List<int> ShareIds { get; set; }
        public int TaxAmountPayed { get; set; }
    }
}