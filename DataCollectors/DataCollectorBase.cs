using System;
using System.Collections.Generic;

using DataCollectorCore;
using DataCollectorCore.DataObjects;

namespace DataCollectors
{
    public abstract class DataCollectorBase : IShopDataCollector
    {
        public abstract string ShopName { get; }

        public ShopDataResult GetShopData(string locationName, string productType)
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

                var products = GetProducts(locationName, url);
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
                        "Failed to process type {0} for shop {1} in location {2}.",
                        productType,
                        ShopName,
                        locationName),
                    Exception = exception,
                };
            }
        }

        protected abstract List<ProductRecord> GetProducts(string locationName, string url);

        protected abstract string GetUrl(string productType);
    }
}