using System.Collections.Generic;

using DataCollectorCore.DataObjects;

namespace TestDataCollector
{
    public interface IProductHelper
    {
        MatchResult FindMatch(SourceProduct sourceProduct, IEnumerable<Product> products);

        Product GenerateProduct(ProductsContext context, SourceProduct sourceProduct);
    }

    public class GeneralProductHelper : IProductHelper
    {
        public MatchResult FindMatch(SourceProduct sourceProduct, IEnumerable<Product> products)
        {
            throw new System.NotImplementedException();
        }

        public Product GenerateProduct(ProductsContext context, SourceProduct sourceProduct)
        {
            return new Product
            {
                ProductTypeId = context.ProductType.ProductTypeId,
                Name = sourceProduct.Name,
            };
        }
    }

    public class MatchResult
    {
        public bool Success { get; set; }

        public Product Product { get; set; }
    }
}
