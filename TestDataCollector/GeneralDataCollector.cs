using System.Collections.Generic;

using DataCollectorCore.DataObjects;

using DataCollectors;

using PostgreDAL;

namespace TestDataCollector
{
    public class GeneralDataCollector
    {
        private readonly ShopDataCollectorFactory _shopDataCollectorFactory = new ShopDataCollectorFactory();

        private readonly IShopsDataStore _dataStore = new ShopsDataStore("shopsdata");

        public IEnumerable<ProductRecord> GetData()
        {
            var records = new List<ProductRecord>();

            var sources = _dataStore.GetDataSources();

            foreach (var dataSource in sources)
            {
                records.AddRange(GetDataFromSource(dataSource));
            }

            return records;
        }

        private List<ProductRecord> GetDataFromSource(DataSource dataSource)
        {
            var records = new List<ProductRecord>();

            var dataCollector = _shopDataCollectorFactory.Create(dataSource.Name);

            var productTypes = _dataStore.GetProductTypes();
            foreach (var productType in productTypes)
            {
                var shopDataResult = dataCollector.GetShopData(productType.Name);
                if (shopDataResult.Success)
                {
                    records.AddRange(shopDataResult.Products);
                }
                // todo: log message
            }

            foreach (var productRecord in records)
            {
                // todo: add source id
                //productRecord.
            }

            return records;
        }
    }
}
