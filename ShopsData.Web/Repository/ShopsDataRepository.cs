using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using DataCollectorCore.DataObjects;

using PostgreDAL;

namespace ShopsData.Web.Repository
{
    public class ShopsDataRepository
    {
        // todo: move name to config
        private static readonly string DbName = "shopsdata_test";

        public List<ProductData> GetCurrentProducts(int locationId, int productTypeId)
        {

            /*return new List<ProductData>
            {
                new ProductData
                {
                    DataSourceId = 1,
                    LocationId = locationId,
                    Name = "Test Name",
                    Price = 3,
                    ProductId = 4,
                    ProductTypeId = productTypeId,
                    Rating = 6,
                    Timestamp = DateTime.UtcNow,
                }
            };*/
            var shopsDataStore = new ShopsDataStore(DbName);
            var products = shopsDataStore.GetCurrentData(locationId, productTypeId);
            return products;
        }

        public List<ProductGroup> GetCurrentProductsGrouped(int locationId, int productTypeId)
        {

            /*return new List<ProductData>
            {
                new ProductData
                {
                    DataSourceId = 1,
                    LocationId = locationId,
                    Name = "Test Name",
                    Price = 3,
                    ProductId = 4,
                    ProductTypeId = productTypeId,
                    Rating = 6,
                    Timestamp = DateTime.UtcNow,
                }
            };*/
            var shopsDataStore = new ShopsDataStore(DbName);
            var products = shopsDataStore.GetCurrentData(locationId, productTypeId);
            /*for (int i = 0; i < products.Count; i++)
            {
                for (int j = 0; j < products.Count; j++)
                {
                    if (products[i].ProductId == products[j].ProductId && products[i].DataSourceId == products[j].DataSourceId)
                    {
                        throw new InvalidDataException();
                    }
                }
            }*/
            var groups = products
                .GroupBy(p => p.ProductId);
            var productsGrouped = new List<ProductGroup>();
            foreach (var g in groups)
            {
                var productGroup = new ProductGroup();
                productGroup.ProductId = g.Key;
                productGroup.ProductName = g.First().FullName;
                foreach (var record in g)
                {
                    productGroup.Records[record.DataSourceId] = record;
                }

                productsGrouped.Add(productGroup);
            }
                /*Select(g => new ProductGroup
                {
                    ProductId = g.Key,
                    ProductName = g.First().Name,
                    Records = g.ToDictionary(p => p.DataSourceId, p => p),
                })
                .ToList();*/
            return productsGrouped.OrderBy(g => g.ProductName).ToList();
        }

        public List<DataSource> GetDataSources()
        {
            var shopsDataStore = new ShopsDataStore(DbName);
            var dataSources = shopsDataStore.GetDataSources();
            return dataSources;
        }

        public List<ProductType> GetProductTypes()
        {
            var shopsDataStore = new ShopsDataStore(DbName);
            var productTypes = shopsDataStore.GetProductTypes();
            return productTypes;
        }

        public List<Location> GetLocations()
        {
            var shopsDataStore = new ShopsDataStore(DbName);
            var locations = shopsDataStore.GetLocations();
            return locations;
        }
    }

    public class ProductGroup
    {
        public ProductGroup()
        {
            Records = new Dictionary<int, ProductData>();
        }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public Dictionary<int, ProductData> Records { get; set; }
    }
}