using System.Web.Http;

using ShopsData.Web.Repository;

namespace ShopsData.Web.API
{
    public class ProductDetailsController : ApiController
    {
        public ProductDetailsModel Get(int locationId, int productId)
        {
            var repository = new ShopsDataRepository();
            return repository.GetProductDetails(locationId, productId);
        }
    }
}
