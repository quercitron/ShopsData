using DataCollectorCore;
using NUnit.Framework;

namespace ShopsData.Tests
{
    [TestFixture]
    public abstract class DataCollectorTestsBase
    {
        [Test]
        public void GetDataTest()
        {
            var collector = GetDataCollector();
            var data = collector.GetShopData("location", "motherboard");
            Assert.That(data, Is.Not.Null);
            Assert.That(data.Success, Is.True);
        }

        protected abstract IShopDataCollector GetDataCollector();
    }
}
