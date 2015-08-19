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

        public void ProcessData()
        {
            var sources = _dataStore.GetDataSources();

            foreach (var dataSource in sources)
            {
                ProcessDataSource(dataSource);
            }
        }

        private void ProcessDataSource(DataSource dataSource)
        {
            var dataCollector = _shopDataCollectorFactory.Create(dataSource.Name);

            var productTypes = _dataStore.GetProductTypes();
            var locations = _dataStore.GetLocations();
            foreach (var location in locations)
            {
                foreach (var productType in productTypes)
                {
                    var shopDataResult = dataCollector.GetShopData(location.Name, productType.Name);
                    if (shopDataResult.Success)
                    {
                        foreach (var product in shopDataResult.Products)
                        {
                            product.LocationId = location.LocationId;
                        }
                        var context = new ProductsContext
                        {
                            DataSource = dataSource,
                            Location = location,
                            ProductType = productType,
                        };
                        AddToDb(context, shopDataResult.Products);
                    }
                    // todo: log message
                }
            }
        }

        private void AddToDb(ProductsContext context, IList<ProductRecord> productRecords)
        {
            
        }
    }

    public class ProductsContext
    {
        public DataSource DataSource { get; set; }

        public Location Location { get; set; }

        public ProductType ProductType { get; set; }
    }
}
