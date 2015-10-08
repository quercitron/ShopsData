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
            // todo: replace with real user id
            var products = shopsDataStore.GetCurrentData(locationId, productTypeId, 1);
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
                productGroup.IsMarked = g.First().IsMarked;
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
            return productsGrouped.OrderByDescending(g => g.IsMarked).ThenBy(g => g.ProductName).ToList();
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
            var dataSources = shopsDataStore.GetDataSources();

            var sourceDetailsGroups = details.GroupBy(d => d.DataSourceId);
            productDetailsModel.SourceDetails = new List<SourceDetail>();
            foreach (var dataSource in dataSources)
            {
                var sourceDetail = new SourceDetail { DataSource = dataSource };
                var sourceGroup = sourceDetailsGroups.FirstOrDefault(x => x.Key == dataSource.DataSourceId);
                if (sourceGroup != null)
                {
                    sourceDetail.Records = sourceGroup.ToList();
                    sourceDetail.LastRecord = sourceGroup.OrderByDescending(x => x.Timestamp).FirstOrDefault();
                }
                productDetailsModel.SourceDetails.Add(sourceDetail);
            }

            var prices = new Prices();
            prices.Labels = new List<string> { "Date" };
            prices.Labels.AddRange(dataSources.Select(ds => ds.Name));

            var dsDict = new Dictionary<int, int>();
            for (int i = 0; i < dataSources.Count; i++)
            {
                dsDict.Add(dataSources[i].DataSourceId, i);
            }
            prices.PlotData = new List<object[]>();
            var date = new DateTime();
            object[] plotData = null;
            foreach (var productDetail in details.OrderBy(d => d.Timestamp))
            {
                // todo: increase time interval?
                if (productDetail.Timestamp.Subtract(date).TotalMinutes < 5)
                {
                    plotData[dsDict[productDetail.DataSourceId] + 1] = productDetail.Price;
                }
                else
                {
                    date = productDetail.Timestamp;

                    if (plotData != null)
                    {
                        prices.PlotData.Add(plotData);
                    }
                    plotData = new object[dataSources.Count + 1];
                    plotData[0] = productDetail.Timestamp;
                    plotData[dsDict[productDetail.DataSourceId] + 1] = productDetail.Price;
                }
            }
            if (plotData != null)
            {
                prices.PlotData.Add(plotData);
            }

            productDetailsModel.Prices = prices;

            return productDetailsModel;
        }

        public void MarkProduct(int userId, int productId, bool isMarked)
        {
            var shopsDataStore = new ShopsDataStore();
            if (isMarked)
            {
                shopsDataStore.MarkProduct(userId, productId);
            }
            else
            {
                shopsDataStore.UnmarkProduct(userId, productId);
            }
        }
    }

    public class ProductDetailsModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public List<SourceDetail> SourceDetails { get; set; }
        public Prices Prices { get; set; }
    }

    public class Prices
    {
        public List<string> Labels { get; set; }
        public List<object[]> PlotData { get; set; }
    }

    public class SourceDetail
    {
        public DataSource DataSource { get; set; }
        public ProductDetail LastRecord { get; set; }
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

        public bool IsMarked { get; set; }

        public Dictionary<int, ProductData> Records { get; set; }
    }
}