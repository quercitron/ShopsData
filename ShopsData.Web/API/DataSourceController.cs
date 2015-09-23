using System.Collections.Generic;
using System.Web.Http;

using DataCollectorCore.DataObjects;

using ShopsData.Web.Repository;

namespace ShopsData.Web.API
{
    public class DataSourceController : ApiController
    {
        public List<DataSource> Get()
        {
            var repository = new ShopsDataRepository();
            return repository.GetDataSources();
        }
    }
}
