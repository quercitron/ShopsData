using System;
using System.Collections.Generic;

using DataCollectorCore;

namespace DataCollectors
{
    public abstract class DataCollectorBase
    {
        public abstract string ShopName { get; }

        public ShopDataResult GetShopData(string productType)
        {
            try
            {
                var url = GetUrl(productType);
                if (url == null)
                {
                    return new ShopDataResult
                           {
                               Success = false,
                               Message = string.Format(
                                   "Product type {0} is not supported for {1} shop",
                                   productType,
                                   ShopName),
                           };
                }

                var products = GetProducts(url);
                return new ShopDataResult
                       {
                           Success = true,
                           Products = products,
                       };
            }
            catch (Exception exception)
            {
                return new ShopDataResult
                       {
                           Success = false,
                           Message = string.Format(
                               "Failed to process type {0} for {1} shop. {2}",
                               productType,
                               ShopName,
                               exception),
                       };
            }
        }

        protected abstract List<Product> GetProducts(string url);

        protected abstract string GetUrl(string productType);
    }
}