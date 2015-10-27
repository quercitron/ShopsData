using System;
using System.Collections.Generic;

using DataCollectorCore.DataObjects;

namespace DataCollectorCore
{
    public interface IShopDataCollector
    {
        ShopDataResult GetShopData(string locationName, string productType);
    }

    public class ShopDataResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public Exception Exception { get; set; }

        public IList<ProductRecord> Products { get; set; }

        public bool NotSell { get; set; }
    }
}
