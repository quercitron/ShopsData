using System;

using DataCollectorCore;
using DataCollectorCore.DataObjects;

using DataCollectors;

namespace TestDataCollector
{
    public interface ISourceManagerFactory
    {
        ISourceManager GetManager(string sourceName);
    }

    public class SourceManagerFactory : ISourceManagerFactory
    {
        public ISourceManager GetManager(string sourceName)
        {
            if (sourceName == null)
            {
                throw new ArgumentNullException("sourceName");
            }

            ISourceManager sourceManager;
            switch (sourceName.ToLower())
            {
                case "dns":
                    sourceManager = new DnsSourceManager();
                    break;
                case "citilink":
                    sourceManager = new CitilinkSourceManager();
                    break;
                default:
                    var message = string.Format("Data source '{0}' is not supported", sourceName);
                    throw new NotSupportedException(message);
            }
            return sourceManager;
        }
    }

    public class CitilinkSourceManager : ISourceManager
    {
        public IShopDataCollector GetDataCollector()
        {
            return new CitilinkDataCollector();
        }

        public IProductRecordHelper GetProductRecordHelper(ProductType productType)
        {
            IProductRecordHelper productRecordHelper;

            switch (productType.Name)
            {
                case ProductTypeName.Motherboard:
                    productRecordHelper = new GeneralMotherboardProductRecordHelper();
                    break;
                default:
                    var message = string.Format("No ProductRecordHelper for product type {0}.", productType.Name);
                    throw new NotSupportedException(message);
            }

            return productRecordHelper;
        }

        public IProductHelper GetProductHelper(ProductType productType)
        {
            throw new NotImplementedException();
        }
    }

    public class DnsSourceManager : ISourceManager
    {
        public IShopDataCollector GetDataCollector()
        {
            return new DnsDataCollector();
        }

        public IProductRecordHelper GetProductRecordHelper(ProductType productType)
        {
            IProductRecordHelper productRecordHelper;

            switch (productType.Name)
            {
                case ProductTypeName.Motherboard:
                    productRecordHelper = new GeneralMotherboardProductRecordHelper();
                    break;
                default:
                    var message = string.Format("No ProductRecordHelper for product type {0}.", productType.Name);
                    throw new NotSupportedException(message);
            }

            return productRecordHelper;
        }

        public IProductHelper GetProductHelper(ProductType productType)
        {
            throw new NotImplementedException();
        }
    }
}
