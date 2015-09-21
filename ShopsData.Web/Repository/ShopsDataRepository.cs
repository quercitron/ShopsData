using System.Collections.Generic;
using PostgreDAL;

namespace ShopsData.Web.Repository
{
    public class ShopsDataRepository
    {
        public List<ProductData> GetCurrentProducts(int locationId, int productTypeId)
        {
            // todo: move name to config
            var shopsDataStore = new ShopsDataStore("shopsdata_test");
            var products = shopsDataStore.GetCurrentData(locationId, productTypeId);
            return products;
        }
    }
}