using System.Collections.Generic;
using System.Web.Http;

using DataCollectorCore.DataObjects;

using ShopsData.Web.Repository;

namespace ShopsData.Web.API
{
    public class LocationController : ApiController
    {
        public List<Location> Get()
        {
            var repository = new ShopsDataRepository();
            return repository.GetLocations();
        }
    }
}
