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
            var repository = new ShopsDataRepository();
            return repository.GetCurrentProducts(locationId, productTypeId);
        }
    }
}
