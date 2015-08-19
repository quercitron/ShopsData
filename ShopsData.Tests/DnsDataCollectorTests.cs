using DataCollectorCore;
using DataCollectors;

namespace ShopsData.Tests
{
    public class DnsDataCollectorTests : DataCollectorTestsBase
    {
        protected override IShopDataCollector GetDataCollector()
        {
            return new DnsDataCollector();
        }
    }
}
