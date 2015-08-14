using System.Collections.Generic;

using DataCollectorCore.DataObjects;

namespace PostgreDAL
{
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


        void AddProductRecord(ProductRecord productRecord);


        void AddLocation(Location location);
    }
}
