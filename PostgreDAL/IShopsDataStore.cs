using System;
using System.Collections.Generic;

using DataCollectorCore.DataObjects;

namespace PostgreDAL
{
    // todo: replace all List with Enamerable?
    public interface IShopsDataStore
    {
        List<ProductType> GetProductTypes();

        void AddProductType(string productType);


        List<DataSource> GetDataSources();

        void AddDataSource(string dataSourceName);


        List<Product> GetProducts();

        List<Product> GetProducts(string productTypeName);

        List<Product> GetProducts(int productTypeId);

        void AddProduct(Product product);

        void UpdateProduct(Product product);

        List<ProductDetail> GetProductDetails(int locationId, int productId);


        void AddProductRecord(ProductRecord productRecord);

        List<ProductRecord> GetProductRecords(int limit = 0, int offset = 0);

        void UpdateProductRecord(ProductRecord productRecord);


        void AddLocation(Location location);

        List<Location> GetLocations();


        List<SourceProduct> GetSourceProducts(int dataSourceId, int productTypeId);

        void AddSourceProduct(SourceProduct sourceProduct);


        List<ProductData> GetCurrentData(int locationId, int productTypeId, int? userId = null);

        void MarkProduct(int userId, int productId, string productName = null);

        void UnmarkProduct(int userId, int productId);
    }

    public class ProductDetail
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public int DataSourceId { get; set; }
        public int Price { get; set; }
        public float Rating { get; set; }
        public DateTime Timestamp { get; set; }
        public string Description { get; set; }
        public string SourceLink { get; set; }
        public string Image { get; set; }
    }

    public class ProductData
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int ProductTypeId { get; set; }
        public int DataSourceId { get; set; }
        public int Price { get; set; }
        public float Rating { get; set; }
        public DateTime Timestamp { get; set; }
        public int LocationId { get; set; }
        public string Class { get; set; }
        public bool IsMarked { get; set; }

        public string FullName
        {
            get
            {
                return string.IsNullOrWhiteSpace(Class)
                    ? Name
                    : string.Format("{0}, {1}", Name, Class);
            }
        }
    }
}
