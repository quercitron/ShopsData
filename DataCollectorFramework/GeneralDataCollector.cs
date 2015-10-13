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

        private readonly IShopsDataStore _dataStore = new ShopsDataStore();

        public void ReprocessRecords()
        {
            try
            {
                _logger.Info("Product records reprocessing started.");

                var locations = _dataStore.GetLocations();
                var dataSources = _dataStore.GetDataSources();
                var productTypes = _dataStore.GetProductTypes();

                // todo: batch processing
                int batchSize = 50000;
                for (int offset = 0;; offset += batchSize)
                {
                    _logger.InfoFormat("Process records {0}-{1}.", offset + 1, offset + batchSize);

                    var productRecords = _dataStore.GetProductRecords(batchSize, offset);

                    _logger.InfoFormat("{0} records taken.", productRecords.Count);

                    if (productRecords.Count == 0)
                    {
                        break;
                    }
                    var recordGroups = productRecords.GroupBy(pr => new { pr.LocationId, pr.DataSourceId, pr.ProductTypeId });
                    foreach (var recordGroup in recordGroups)
                    {
                        var context = new ProductsContext
                        {
                            Location = locations.First(l => l.LocationId == recordGroup.Key.LocationId),
                            DataSource = dataSources.First(ds => ds.DataSourceId == recordGroup.Key.DataSourceId),
                            ProductType = productTypes.First(pt => pt.ProductTypeId == recordGroup.Key.ProductTypeId),
                        };
                        var records = recordGroup.ToList();
                        AddToDb(context, records, true);
                    }

                    _logger.Info("Batch processed.");
                }

                _logger.Info("Product records reprocessing complete.");
            }
            catch (Exception exception)
            {
                var message = "Failed to reprocess product records.";
                _logger.Error(message, exception);
            }
        }

        public void CollectData()
        {
            try
            {
                var productTypes = _dataStore.GetProductTypes();

                foreach (var productType in productTypes)
                {
                    ProcessProductType(productType);
                }
            }
            catch (Exception exception)
            {
                _logger.Error("Failed to collect data.", exception);
            }
        }

        private void ProcessProductType(ProductType productType)
        {
            _logger.InfoFormat("Start processing product type '{0}'.", productType.Name);

            try
            {
                var dataSources = _dataStore.GetDataSources();

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
                    foreach (var dataSource in dataSources)
                    {
                        try
                        {
                            _logger.InfoFormat("Start processing data source '{0}'.", dataSource.Name);

                            var sourceManager = _sourceManagerFactory.GetManager(dataSource.Name);
                            var dataCollector = sourceManager.GetDataCollector();

                            var shopDataResult = dataCollector.GetShopData(location.Name, productType.Name);
                            if (shopDataResult.Success)
                            {
                                if (shopDataResult.Products != null && shopDataResult.Products.Count > 0)
                                {
                                    _logger.InfoFormat("{0} records collected.", shopDataResult.Products.Count);

                                    var date = DateTime.UtcNow;
                                    foreach (var product in shopDataResult.Products)
                                    {
                                        product.DataSourceId = dataSource.DataSourceId;
                                        product.LocationId = location.LocationId;
                                        product.ProductTypeId = productType.ProductTypeId;
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
                                    var message = string.Format(
                                        "No products returned for data source '{0}', location '{1}', product type '{2}'.",
                                        dataSource.Name,
                                        location.Name,
                                        productType.Name);
                                    _logger.Error(message, shopDataResult.Exception);
                                }
                            }
                            else
                            {
                                var message = string.Format("Failed to collect data: {0}", shopDataResult.Message);
                                _logger.Error(message, shopDataResult.Exception);
                            }
                            _logger.InfoFormat("Complete processing data source '{0}'.", dataSource.Name);
                        }
                        catch (Exception exception)
                        {
                            var message = string.Format(
                                "Failed to process product type '{0}' for data source '{1}'.",
                                productType.Name,
                                dataSource.Name);
                            _logger.Error(message, exception);
                        }
                    }
                    _logger.InfoFormat("Complete processing location '{0}'.", location.Name);
                }
            }
            catch (Exception exception)
            {
                var message = string.Format("Failed to process product type '{0}'.", productType.Name);
                _logger.Error(message, exception);
            }
            _logger.InfoFormat("Complete processing product type '{0}'.", productType.Name);
        }

        private void AddToDb(ProductsContext context, IList<ProductRecord> productRecords, bool updateProductRecords = false)
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
                        var duplicate = sourceProducts
                            .FirstOrDefault(
                                sp => sp.ProductId == matchResult.Product.ProductId &&
                                      sp.DataSourceId == sourceProduct.DataSourceId);
                        if (duplicate != null)
                        {
                            var message =
                                string.Format(
                                    "External duplicate found: two equal products ('{0}' and '{1}') " +
                                    "with different externalid ('{2}' and '{3}') in the same datasource ('{4}').",
                                    duplicate.OriginalName,
                                    sourceProduct.OriginalName,
                                    duplicate.Key,
                                    sourceProduct.Key,
                                    context.DataSource.Name);
                            _logger.Warn(message);
                            // todo: how to handle external duplicate?
                            // todo: add new fake product?
                        }

                        if (matchResult.UpdateProduct)
                        {
                            _dataStore.UpdateProduct(matchResult.Product);
                        }

                        product = matchResult.Product;
                    }
                    else
                    {
                        product = productHelper.GenerateProduct(context, sourceProduct);
                        _dataStore.AddProduct(product);
                        products.Add(product);

                        ValidateProduct(product);
                    }

                    sourceProduct.ProductId = product.ProductId;

                    _dataStore.AddSourceProduct(sourceProduct);
                    sourceProducts.Add(sourceProduct);
                }

                productRecord.SourceProductId = sourceProduct.SourceProductId;
            }

            if (updateProductRecords)
            {
                foreach (var productRecord in productRecords)
                {
                    _dataStore.UpdateProductRecord(productRecord);
                }
            }
            else
            {
                foreach (var productRecord in productRecords)
                {
                    _dataStore.AddProductRecord(productRecord);
                }
            }
        }

        private void ValidateProduct(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                _logger.WarnFormat("Product with empty name was generated, productId: {0}.", product.ProductId);
            }
            else if (product.Name.All(ch => !char.IsLetter(ch)))
            {
                _logger.WarnFormat("Product name '{0}' doesn't contain any letter, productId: {1}.", product.Name, product.ProductId);
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
