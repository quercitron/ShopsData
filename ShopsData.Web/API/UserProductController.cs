using System.Web.Http;

using ShopsData.Web.Repository;

namespace ShopsData.Web.API
{
    public class UserProductController : ApiController
    {
        public void Post([FromBody] ProductMarkParameters parameters)
        {
            // todo: use real user id
            var userId = 1;

            var repository = new ShopsDataRepository();
            repository.MarkProduct(userId, parameters.ProductId, parameters.IsMarked);
        }

        public class ProductMarkParameters
        {
            public int ProductId { get; set; }

            public bool IsMarked { get; set; }
        }
    }
}
