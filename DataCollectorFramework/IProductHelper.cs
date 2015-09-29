using System.Collections.Generic;
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
            Product bestMatch = null;

            var targetName = CleanName(sourceProduct.Name);
            foreach (var product in products)
            {
                var productName = CleanName(product.Name);
                var distance = WordsHelper.LevenshteinDistance(targetName, productName);
                if (bestMatch == null || distance < bestDistance)
                {
                    bestMatch = product;
                    bestDistance = distance;
                }
            }

            var result = new MatchResult();
            if (bestMatch == null || bestDistance > MatchLimit)
            {
                result.Success = false;
            }
            else
            {
                result.Success = true;
                result.Product = bestMatch;
            }
            return result;
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
                Created = sourceProduct.Timestamp,
            };
        }
    }

    public class MatchResult
    {
        public bool Success { get; set; }

        public Product Product { get; set; }
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
