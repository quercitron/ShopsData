using System.IO;
using System.Text;
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

            using (var writer = new StreamWriter("output.txt", false, Encoding.UTF8))
            {
                if (data.Success)
                {
                    foreach (var productRecord in data.Products)
                    {
                        //writer.WriteLine(productRecord);
                        writer.WriteLine(productRecord.Name);
                    }
                }
                else
                {
                    writer.WriteLine(data.Message);
                    writer.WriteLine(data.Exception);
                }
            }

            Assert.That(data, Is.Not.Null);
            Assert.That(data.Success, Is.True);
        }

        protected abstract IShopDataCollector GetDataCollector();
    }
}
