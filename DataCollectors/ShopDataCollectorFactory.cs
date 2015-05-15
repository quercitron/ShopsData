using System;

using DataCollectorCore;

namespace DataCollectors
{
    public class ShopDataCollectorFactory
    {
        public IShopDataCollector Create(string sourceName)
        {
            IShopDataCollector shopDataCollector;
            switch (sourceName.ToLower())
            {
                case "dns":
                    shopDataCollector = new DnsDataCollector();
                    break;
                case "citilink":
                    shopDataCollector = new CitilinkDataCollector();
                    break;
                default:
                    var message = string.Format("Data source '{0}' is not supported", sourceName);
                    throw new NotSupportedException(message);
            }
            return shopDataCollector;
        }
    }
}
