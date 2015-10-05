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
                var urlResult = GetUrl(productType);
                if (urlResult != null && urlResult.NotSell)
                {
                    // todo: add logging?
                    return new ShopDataResult { Success = true };
                }
                if (urlResult == null || urlResult.Url == null)
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

                var products = GetProducts(locationName, urlResult.Url);
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

        protected abstract GetUrlResult GetUrl(string productType);
    }

    public class GetUrlResult
    {
        public string Url { get; set; }

        public bool NotSell { get; set; }
    }
}