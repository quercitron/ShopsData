using System;
using System.Collections.Generic;

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

            return new List<ProductData>
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
            };
            //var shopsDataStore = new ShopsDataStore(DbName);
            //var products = shopsDataStore.GetCurrentData(locationId, productTypeId);
            //return products;
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
    }
}