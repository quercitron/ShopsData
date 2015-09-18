using System;
using System.Collections.Generic;
using System.Linq;
using DataCollectorCore.DataObjects;
using DataCollectorFramework.Logger;
using PostgreDAL;

namespace DataCollectorFramework
{
    public class GeneralDataCollector
    {
        private readonly ILogger _logger = new Logger.Logger(typeof(GeneralDataCollector));

        private readonly SourceManagerFactory _sourceManagerFactory = new SourceManagerFactory();

        private readonly IShopsDataStore _dataStore = new ShopsDataStore("shopsdata_test");

        public void CollectData()
        {
            try
            {
                var sources = _dataStore.GetDataSources();

                foreach (var dataSource in sources)
                {
                    ProcessDataSource(dataSource);
                }
            }
            catch (Exception exception)
            {
                _logger.Error("Failed to collect data.", exception);
            }
        }

        private void ProcessDataSource(DataSource dataSource)
        {
            _logger.InfoFormat("Start processing data source '{0}'.", dataSource.Name);

            try
            {
                var sourceManager = _sourceManagerFactory.GetManager(dataSource.Name);

                var dataCollector = sourceManager.GetDataCollector();

                var locations = _dataStore.GetLocations();
                if (locations.Count == 0)
                {
                    _logger.Info("Locations list is empty.");
                }

                var productTypes = _dataStore.GetProductTypes();
                if (productTypes.Count == 0)
                {
                    _logger.Info("Product types list is empty.");
                }

                foreach (var location in locations)
                {
                    _logger.InfoFormat("Start processing location '{0}'.", location.Name);
                    foreach (var productType in productTypes)
                    {
                        _logger.InfoFormat("Start processing product type '{0}'.", productType.Name);
                        var shopDataResult = dataCollector.GetShopData(location.Name, productType.Name);
                        if (shopDataResult.Success)
                        {
                            if (shopDataResult.Products != null && shopDataResult.Products.Count > 0)
                            {
                                var date = DateTime.UtcNow;
                                foreach (var product in shopDataResult.Products)
                                {
                                    product.LocationId = location.LocationId;
                                    product.Timestamp = date;
                                }
                                var context = new ProductsContext
                                {
                                    DataSource = dataSource,
                                    Location = location,
                                    ProductType = productType,
                                };
                                AddToDb(context, shopDataResult.Products);
                            }
                            else
                            {
                                _logger.WarnFormat(
                                    "No products returned for data source '{0}', location '{1}', product type '{2}'.",
                                    dataSource.Name,
                                    location.Name,
                                    productType.Name);
                            }
                        }
                        else
                        {
                            var message = string.Format("Failed to collect data: {0}.", shopDataResult.Message);
                            _logger.WarnFormat(message, shopDataResult.Exception);
                        }
                        _logger.InfoFormat("Complete processing product type '{0}'.", productType.Name);
                    }
                    _logger.InfoFormat("Complete processing location '{0}'.", location.Name);
                }
            }
            catch (Exception exception)
            {
                var message = string.Format("Failed to process datasource '{0}'.", dataSource.Name);
                _logger.Error(message, exception);
            }

            _logger.InfoFormat("Complete processing data source '{0}'.", dataSource.Name);
        }

        private void AddToDb(ProductsContext context, IList<ProductRecord> productRecords)
        {
            var sourceManager = _sourceManagerFactory.GetManager(context.DataSource.Name);

            var productRecordHelper = sourceManager.GetProductRecordHelper(context.ProductType);
            var productHelper = sourceManager.GetProductHelper(context.ProductType);

            var sourceProducts = _dataStore.GetSourceProducts(context.DataSource.DataSourceId, context.ProductType.ProductTypeId);
            var products = _dataStore.GetProducts(context.ProductType.ProductTypeId);

            foreach (var productRecord in productRecords)
            {
                var key = productRecordHelper.GetKey(productRecord);

                // todo: add to db unique constraint for key
                var sourceProduct = sourceProducts.FirstOrDefault(sp => sp.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));
                if (sourceProduct == null)
                {
                    sourceProduct = productRecordHelper.GenerateSourceProduct(context.DataSource, productRecord);

                    var matchResult = productHelper.FindMatch(sourceProduct, products);

                    Product product;
                    if (matchResult.Success)
                    {
                        product = matchResult.Product;
                    }
                    else
                    {
                        product = productHelper.GenerateProduct(context, sourceProduct);
                        _dataStore.AddProduct(product);
                        products.Add(product);
                    }

                    sourceProduct.ProductId = product.ProductId;

                    _dataStore.AddSourceProduct(sourceProduct);
                    sourceProducts.Add(sourceProduct);
                }

                productRecord.SourceProductId = sourceProduct.SourceProductId;
            }

            foreach (var productRecord in productRecords)
            {
                _dataStore.AddProductRecord(productRecord);
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
