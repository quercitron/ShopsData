using System;
using System.IO;

using DataCollectorCore;

using DataCollectors;

using PostgreDAL;

namespace TestDataCollector
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var dataStore = new ShopsDataStore("shops");

            var dataSources = dataStore.GetDataSources();
            var productTypes = dataStore.GetProductTypes();
            var factory = new DataCollectorFactory();
            foreach (var dataSource in dataSources)
            {
                var dataCollector = factory.Create(dataSource.Name);
                foreach (var productType in productTypes)
                {
                    var shopData = dataCollector.GetShopData("location", productType.Name);
                    using (var writer = File.CreateText("output.txt"))
                    {
                        if (shopData.Success)
                        {
                            foreach (var productRecord in shopData.Products)
                            {
                                writer.WriteLine(productRecord);
                            }
                        }
                        else
                        {
                            writer.WriteLine(shopData.Message);
                            writer.WriteLine(shopData.Exception);
                        }
                    }
                }
            }
        }

        private static void GetData()
        {
            var dataCollector = new DnsDataCollector();

            var data = dataCollector.GetShopData("location", "motherboard");

            using (var writer = File.CreateText("output.txt"))
            {
                if (data.Success)
                {
                    foreach (var item in data.Products)
                    {
                        writer.WriteLine(item.Name);
                        writer.WriteLine(item.Price);
                        writer.WriteLine(item.Description);
                        writer.WriteLine();
                    }
                }
                else
                {
                    writer.WriteLine(data.Message);
                }
            }
        }
    }

    public class DataCollectorFactory
    {
        public IShopDataCollector Create(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            IShopDataCollector result;
            switch (name.ToLower())
            {
                case "dns":
                    result = new DnsDataCollector();
                    break;
                case "citilink":
                    result = new CitilinkDataCollector();
                    break;
                default:
                    var message = string.Format("Data source '{0}' is not supported", name);
                    throw new NotSupportedException(message);
            }
            return result;
        }
    }
}
