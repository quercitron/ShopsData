using System;
using System.Text.RegularExpressions;
using DataCollectorCore;
using DataCollectorCore.DataObjects;
using DataCollectors;

namespace DataCollectorFramework
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
                case "ulmart":
                    sourceManager = new UlmartSourceManager();
                    break;
                default:
                    var message = string.Format("Data source '{0}' is not supported", sourceName);
                    throw new NotSupportedException(message);
            }
            return sourceManager;
        }
    }

    public class UlmartSourceManager : ISourceManager
    {
        public IShopDataCollector GetDataCollector()
        {
            return new UlmartDataCollector();
        }

        public IProductRecordHelper GetProductRecordHelper(ProductType productType)
        {
            IProductRecordHelper productRecordHelper;

            switch (productType.Name)
            {
                case ProductTypeName.Motherboard:
                    productRecordHelper = new GeneralMotherboardProductRecordHelper();
                    break;
                case ProductTypeName.Monitor:
                    productRecordHelper = new GeneralMonitorProductRecordHelper();
                    break;
                case ProductTypeName.PowerSupply:
                    productRecordHelper = new GeneralPowerSupplyProductRecordHelper();
                    break;
                default:
                    var message = string.Format("No ProductRecordHelper for product type {0}.", productType.Name);
                    throw new NotSupportedException(message);
            }

            return productRecordHelper;
        }

        public IProductHelper GetProductHelper(ProductType productType)
        {
            return new GeneralProductHelper();
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
                case ProductTypeName.Monitor:
                    productRecordHelper = new GeneralMonitorProductRecordHelper();
                    break;
                case ProductTypeName.PowerSupply:
                    productRecordHelper = new GeneralPowerSupplyProductRecordHelper();
                    break;
                default:
                    var message = string.Format("No ProductRecordHelper for product type {0}.", productType.Name);
                    throw new NotSupportedException(message);
            }

            return productRecordHelper;
        }

        public IProductHelper GetProductHelper(ProductType productType)
        {
            return new GeneralProductHelper();
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
                case ProductTypeName.PowerSupply:
                    productRecordHelper = new DnsPowerSupplyHelper();
                    break;
                case ProductTypeName.Monitor:
                    productRecordHelper = new GeneralMonitorProductRecordHelper();
                    break;
                default:
                    var message = string.Format("No ProductRecordHelper for product type {0}.", productType.Name);
                    throw new NotSupportedException(message);
            }

            return productRecordHelper;
        }

        public IProductHelper GetProductHelper(ProductType productType)
        {
            return new GeneralProductHelper();
        }
    }

    public class DnsPowerSupplyHelper : GeneralPowerSupplyProductRecordHelper
    {
        protected override ComplexName ProcessName(ProductRecord productRecord)
        {
            var baseResult = base.ProcessName(productRecord);

            var name = baseResult.Name;

            string result;
            var match = Regex.Match(name, @"\[(.*)\]");
            if (match.Success)
            {
                result = string.Format("{0} {1}", productRecord.Brand, match.Groups[1].Value);
                var index = result.IndexOf("/");
                if (index >= 0)
                {
                    result = result.Substring(0, index);
                }
            }
            else
            {
                result = name.Replace("Блок питания ", "");
            }

            return new ComplexName
            {
                Name = result,
                Class = baseResult.Class,
            };
        }
    }
}
