using System;
using System.Collections.Generic;
using System.Web.Http;
using PostgreDAL;
using ShopsData.Web.Repository;

namespace ShopsData.Web.API
{
    public class CurrentDataController : ApiController
    {
        public List<ProductData> Get(int locationId, int productTypeId)
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
            //var repository = new ShopsDataRepository();
            //return repository.GetCurrentProducts(locationId, productTypeId);
        }
    }
}
