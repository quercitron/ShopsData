using System.Collections.Generic;

using DataCollectorCore.DataObjects;

namespace TestDataCollector
{
    public interface IProductHelper
    {
        MatchResult FindMatch(SourceProduct sourceProduct, IEnumerable<Product> products);

        Product GenerateProduct(SourceProduct sourceProduct);
    }

    public class MatchResult
    {
        public bool Success { get; set; }

        public Product Product { get; set; }
    }
}
