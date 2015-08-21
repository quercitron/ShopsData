using System;
using System.Collections.Generic;
using System.Linq;

using DataCollectorCore.DataObjects;

using PostgreDAL;

namespace TestDataCollector
{
    public class GeneralDataCollector
    {
        private readonly SourceManagerFactory _sourceManagerFactory = new SourceManagerFactory();

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
            var sourceManager = _sourceManagerFactory.GetManager(dataSource.Name);

            var dataCollector = sourceManager.GetDataCollector();

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
            var sourceManager = _sourceManagerFactory.GetManager(context.DataSource.Name);

            var productRecordHelper = sourceManager.GetProductRecordHelper(context.ProductType);
            var productHelper = sourceManager.GetProductHelper(context.ProductType);

            var sourceProducts = _dataStore.GetSourceProducts(context.DataSource.DataSourceId, context.ProductType.ProductTypeId);
            var products = _dataStore.GetProducts(context.ProductType.ProductTypeId);

            var newProducts = new List<Product>();
            var newSourceProducts = new List<SourceProduct>();

            foreach (var productRecord in productRecords)
            {
                var key = productRecordHelper.GetKey(productRecord);

                // todo: add to db unique constraint for key
                var sourceProduct = sourceProducts.FirstOrDefault(sp => sp.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));
                if (sourceProduct == null)
                {
                    sourceProduct = productRecordHelper.GenerateSourceProduct(productRecord);

                    var matchResult = productHelper.FindMatch(sourceProduct, products);

                    Product product;
                    if (matchResult.Success)
                    {
                        product = matchResult.Product;
                    }
                    else
                    {
                        product = productHelper.GenerateProduct(sourceProduct);
                        newProducts.Add(product);
                    }

                    // todo: add ProductId column to db
                    sourceProduct.ProductId = product.ProductId;
                }
            }
        }
    }

    public class ProductsContext
    {
        public DataSource DataSource { get; set; }

        public Location Location { get; set; }

        public ProductType ProductType { get; set; }
    }
}
