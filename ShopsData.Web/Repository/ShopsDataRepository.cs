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
        public List<ProductData> GetCurrentProducts(int locationId, int productTypeId)
        {
            var shopsDataStore = new ShopsDataStore();
            var products = shopsDataStore.GetCurrentData(locationId, productTypeId);
            return products;
        }

        public List<ProductGroup> GetCurrentProductsGrouped(int locationId, int productTypeId)
        {
            var shopsDataStore = new ShopsDataStore();
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
            var shopsDataStore = new ShopsDataStore();
            var dataSources = shopsDataStore.GetDataSources();
            return dataSources;
        }

        public List<ProductType> GetProductTypes()
        {
            var shopsDataStore = new ShopsDataStore();
            var productTypes = shopsDataStore.GetProductTypes();
            return productTypes;
        }

        public List<Location> GetLocations()
        {
            var shopsDataStore = new ShopsDataStore();
            var locations = shopsDataStore.GetLocations();
            return locations;
        }

        public ProductDetailsModel GetProductDetails(int locationId, int productId)
        {
            var shopsDataStore = new ShopsDataStore();
            var details = shopsDataStore.GetProductDetails(locationId, productId);

            var productDetailsModel = new ProductDetailsModel();
            productDetailsModel.ProductId = productId;
            productDetailsModel.ProductName = details.First().Name;
            productDetailsModel.SourceDetails =
                details
                    .GroupBy(d => d.DataSourceId)
                    .ToDictionary(g => g.Key, g => new SourceDetail { Records = g.ToList() });

            return productDetailsModel;
        }
    }

    public class ProductDetailsModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public Dictionary<int, SourceDetail> SourceDetails { get; set; }
    }

    public class SourceDetail
    {
        public List<ProductDetail> Records { get; set; }
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