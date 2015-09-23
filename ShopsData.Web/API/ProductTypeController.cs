using System.Collections.Generic;
using System.Web.Http;

using DataCollectorCore.DataObjects;

using ShopsData.Web.Repository;

namespace ShopsData.Web.API
{
    public class ProductTypeController : ApiController
    {
        public List<ProductType> Get()
        {
            var repository = new ShopsDataRepository();
            return repository.GetProductTypes();
        }
    }
}
