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


        void AddProductRecord(ProductRecord productRecord);


        void AddLocation(Location location);

        List<Location> GetLocations();


        List<SourceProduct> GetSourceProducts(int dataSourceId, int productTypeId);
    }
}
