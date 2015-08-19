using DataCollectorCore;
using DataCollectors;
using NUnit.Framework;

namespace ShopsData.Tests
{
    public class CitilinkDataCollectorTests : DataCollectorTestsBase
    {
        protected override IShopDataCollector GetDataCollector()
        {
            return new CitilinkDataCollector();
        }
    }
}
