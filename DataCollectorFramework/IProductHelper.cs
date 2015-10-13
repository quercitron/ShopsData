using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DataCollectorCore.DataObjects;

namespace DataCollectorFramework
{
    public interface IProductHelper
    {
        MatchResult FindMatch(SourceProduct sourceProduct, IEnumerable<Product> products);

        Product GenerateProduct(ProductsContext context, SourceProduct sourceProduct);
    }

    public class GeneralProductHelper : IProductHelper
    {
        private const int MatchLimit = 0;

        public MatchResult FindMatch(SourceProduct sourceProduct, IEnumerable<Product> products)
        {
            int bestDistance = int.MaxValue;
            var bestMatchProducts = new List<Product>();

            if (sourceProduct.Code != null)
            {
                var targetCode = CleanName(sourceProduct.Code);
                bestMatchProducts = products.Where(p => p.Code != null && CleanName(p.Code) == targetCode).ToList();
            }

            if (bestMatchProducts.Count == 0)
            {
                var targetName = CleanName(sourceProduct.Name);
                foreach (var product in products)
                {
                    // different codes -> different products
                    if (sourceProduct.Code != null && product.Code != null)
                    {
                        continue;
                    }

                    var productName = CleanName(product.Name);
                    var distance = WordsHelper.LevenshteinDistance(targetName, productName);
                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        bestMatchProducts = new List<Product> { product };
                    }
                    else if (distance == bestDistance)
                    {
                        bestMatchProducts.Add(product);
                    }
                }

                if (bestDistance > MatchLimit || bestMatchProducts.Count == 0)
                {
                    return new MatchResult { Success = false };
                }
            }

            if (bestMatchProducts.Count > 1)
            {
                var message = string.Format(
                    "For source product '{0}' found {1} best much products: {2}.",
                    sourceProduct.Name,
                    bestMatchProducts.Count,
                    string.Join(", ", bestMatchProducts.Select(p => p.Name)));
                // todo: log?
            }

            bestMatchProducts = bestMatchProducts.OrderBy(p => p.Created).ToList();

            MatchResult matchResult = null; 

            if (!string.IsNullOrWhiteSpace(sourceProduct.Class))
            {
                var product = bestMatchProducts.FirstOrDefault(p => CleanName(p.Class) == CleanName(sourceProduct.Class));
                if (product != null)
                {
                    matchResult = new MatchResult { Success = true, Product = product };
                }
                else
                {
                    product = bestMatchProducts.FirstOrDefault(p => string.IsNullOrWhiteSpace(p.Class));
                    if (product != null)
                    {
                        product.Class = sourceProduct.Class;
                        matchResult = new MatchResult { Success = true, Product = product, UpdateProduct = true };
                    }
                    else
                    {
                        matchResult = new MatchResult { Success = false };
                    }
                }
            }
            else
            {
                var product = bestMatchProducts.FirstOrDefault(p => string.IsNullOrWhiteSpace(p.Class));
                if (product != null)
                {
                    matchResult = new MatchResult { Success = true, Product = product };
                }
                else
                {
                    product = bestMatchProducts.FirstOrDefault();
                    if (product != null)
                    {
                        matchResult = new MatchResult { Success = true, Product = product };
                    }
                    else
                    {
                        matchResult = new MatchResult { Success = false };
                    }
                }
            }

            if (sourceProduct.Code != null && matchResult.Product != null && matchResult.Product.Code == null)
            {
                matchResult.Product.Code = sourceProduct.Code;
                matchResult.UpdateProduct = true;
            }
            return matchResult;
        }

        private string CleanName(string name)
        {
            return Regex.Replace((name ?? "").ToLower(), "[^0-9a-z]", "");
        }

        public Product GenerateProduct(ProductsContext context, SourceProduct sourceProduct)
        {
            return new Product
            {
                ProductTypeId = context.ProductType.ProductTypeId,
                Name = sourceProduct.Name,
                Class = sourceProduct.Class,
                Code = sourceProduct.Code,
                Created = sourceProduct.Timestamp,
            };
        }
    }

    public class MatchResult
    {
        public bool Success { get; set; }

        public Product Product { get; set; }

        public bool UpdateProduct { get; set; }
    }

    public static class WordsHelper
    {
        public static int LevenshteinDistance(string a, string b)
        {
            var d = new int[a.Length + 1, b.Length + 1];

            for (int i = 0; i <= a.Length; i++)
            {
                d[i, 0] = i;
            }
            for (int j = 0; j <= b.Length; j++)
            {
                d[0, j] = j;
            }

            for (int i = 0; i < a.Length; i++)
            {
                for (int j = 0; j < b.Length; j++)
                {
                    if (a[i] == b[j])
                    {
                        d[i + 1, j + 1] = d[i, j];
                    }
                    else
                    {
                        d[i + 1, j + 1] = d[i, j + 1] + 1;
                        if (d[i + 1, j + 1] > d[i + 1, j] + 1)
                        {
                            d[i + 1, j + 1] = d[i + 1, j] + 1;
                        }
                        if (d[i + 1, j + 1] > d[i, j] + 1)
                        {
                            d[i + 1, j + 1] = d[i, j] + 1;
                        }
                    }
                }
            }
            return d[a.Length, b.Length];
        }
        
    }
}
