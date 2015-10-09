using System;
using System.Globalization;
using System.IO;

using DataCollectorCore;
using DataCollectorFramework;
using DataCollectors;

using Newtonsoft.Json;

using PostgreDAL;

namespace TestDataCollector
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

            //TestGeneralDataCollector();
            GetData();

            //ReprocessRecords();
        }

        private static void ReprocessRecords()
        {
            var collector = new GeneralDataCollector();
            collector.ReprocessRecords();
        }

        private static void TestGeneralDataCollector()
        {
            try
            {
                var dataCollector = new GeneralDataCollector();
                dataCollector.CollectData();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private static void GetDataFromDataCollector()
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
            var dataCollector = new KeyDataCollector();

            var data = dataCollector.GetShopData("vrn", ProductTypeName.SSD);
            //var dnsPowerSupplyHelper = new DnsPowerSupplyHelper();

            using (var writer = File.CreateText("output.txt"))
            {
                if (data.Success)
                {
                    writer.Write(JsonConvert.SerializeObject(data.Products, Formatting.Indented));
                }
                else
                {
                    writer.WriteLine(data.Message);
                    writer.WriteLine(data.Exception);

                    Console.WriteLine(data.Message);
                    Console.WriteLine(data.Exception);
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
