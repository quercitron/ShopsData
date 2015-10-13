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
                var detailedProducts = AddDetails(g.ToList());

                var productGroup = new ProductGroup();
                productGroup.ProductId = g.Key;
                productGroup.ProductName = detailedProducts.First().Name;
                productGroup.ProductClass = detailedProducts.First().Class;
                productGroup.ProductCode = detailedProducts.First().Code;
                productGroup.IsMarked = detailedProducts.First().IsMarked;
                var recordsWithRating = detailedProducts.Where(p => p.Rating > 0.1);
                productGroup.Rating = recordsWithRating.Any() ? recordsWithRating.Average(p => p.Rating) : 0;
                productGroup.MinPrice = detailedProducts.Min(p => p.Price);
                foreach (var record in detailedProducts)
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

        private List<ProductDataDetailed> AddDetails(List<ProductData> records)
        {
            var result = new List<ProductDataDetailed>();

            if (records.Any())
            {
                double avaragePrice;
                var prices = records.Select(r => r.Price).OrderBy(p => p).ToList();
                if (prices.Count % 2 == 0)
                {
                    avaragePrice = (prices[prices.Count / 2 - 1] + prices[prices.Count / 2]) / 2;
                }
                else
                {
                    avaragePrice = prices[prices.Count / 2];
                }
                if (avaragePrice > 0)
                {
                    foreach (var record in records)
                    {
                        result.Add(new ProductDataDetailed(record) { PriceRelative = record.Price / avaragePrice });
                    }
                }
            }

            return result;
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
            Records = new Dictionary<int, ProductDataDetailed>();
        }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductClass { get; set; }

        public string ProductCode { get; set; }

        public bool IsMarked { get; set; }

        public Dictionary<int, ProductDataDetailed> Records { get; set; }

        public float Rating { get; set; }

        public int MinPrice { get; set; }
    }

    public class ProductDataDetailed : ProductData
    {
        public ProductDataDetailed(ProductData productData)
        {
            // todo: automapper?
            this.ProductId = productData.ProductId;
            this.Name = productData.Name;
            this.ProductTypeId = productData.ProductTypeId;
            this.DataSourceId = productData.DataSourceId;
            this.Price = productData.Price;
            this.Rating = productData.Rating;
            this.Timestamp = productData.Timestamp;
            this.LocationId = productData.LocationId;
            this.Class = productData.Class;
            this.Code = productData.Code;
            this.IsMarked = productData.IsMarked;
        }

        public double PriceRelative { get; set; }

        public string Style
        {
            get
            {
                var neutralColor = new Color(0xE7, 0xE7, 0xE7);
                var highCostColor = new Color(0xFF, 0x00, 0x00);
                var lowCostColor = new Color(0x00, 0xFF, 0x00);
                Color color;
                if (PriceRelative > 1)
                {
                    double limit = 1.3;
                    double neutralK = (limit - PriceRelative) / (limit - 1);
                    if (neutralK < 0)
                    {
                        neutralK = 0;
                    }
                    color = neutralK * neutralColor + (1 - neutralK) * highCostColor;
                }
                else
                {
                    double limit = 0.7;
                    double neutralK = (PriceRelative - limit) / (1 - limit);
                    if (neutralK < 0)
                    {
                        neutralK = 0;
                    }
                    color = neutralK * neutralColor + (1 - neutralK) * lowCostColor;
                }
                return string.Format("{{'background-color': '{0}'}}", color.GetCode());
            }
        }

        private class Color
        {
            public Color(double r, double g, double b)
            {
                R = r;
                G = g;
                B = b;
            }

            public double R { get; set; }
            public double G { get; set; }
            public double B { get; set; }

            public static Color operator *(double k, Color color)
            {
                return new Color(k * color.R, k * color.G, k * color.B);
            }

            public static Color operator /(Color color, double k)
            {
                return new Color(color.R / k, color.G / k, color.B / k);
            }

            public static Color operator +(Color color1, Color color2)
            {
                return new Color(color1.R + color2.R, color1.G + color2.G, color1.B + color2.B);
            }

            public string GetCode()
            {
                return string.Format(
                    "#{0}{1}{2}",
                    ToInt(R).ToString("X2"),
                    ToInt(G).ToString("X2"),
                    ToInt(B).ToString("X2"));
            }

            private int ToInt(double d)
            {
                var i = (int)d;
                if (i < 0)
                {
                    i = 0;
                }
                if (i > 255)
                {
                    i = 255;
                }
                return i;
            }
        }
    }
}