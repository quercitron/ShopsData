using System.Collections.Generic;

namespace DataCollectorCore
{
    public interface IShopDataCollector
    {
        ShopDataResult GetShopData(string productType);
    }

    public class ShopDataResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public IList<Product> Products { get; set; }
    }
}
