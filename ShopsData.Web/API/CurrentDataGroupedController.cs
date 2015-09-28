using System.Collections.Generic;
using System.Web.Http;

using ShopsData.Web.Repository;

namespace ShopsData.Web.API
{
    public class CurrentDataGroupedController : ApiController
    {
        public List<ProductGroup> Get(int locationId, int productTypeId)
        {
            var repository = new ShopsDataRepository();
            return repository.GetCurrentProductsGrouped(locationId, productTypeId);
        }
    }
}
