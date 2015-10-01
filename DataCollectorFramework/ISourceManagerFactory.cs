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
                case ProductTypeName.Screwdriver:
                    productRecordHelper = new GeneralScrewdriverProductRecordHelper();
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
                case ProductTypeName.Screwdriver:
                    productRecordHelper = new GeneralScrewdriverProductRecordHelper();
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

    public class GeneralScrewdriverProductRecordHelper : GeneralProductRecordHelper
    {
        public override ComplexName ProcessName(ProductRecord productRecord)
        {
            var baseResult = base.ProcessName(productRecord);

            var name = baseResult.Name;
            name = Regex.Replace(name, "Ударная дрель-шуруповерт", "Шуруповерт ударный", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, "Ударный шуруповерт", "Шуруповерт ударный", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, "Аккумуляторный шуруповерт", "Шуруповерт", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, "Дрель-шуруповерт", "Шуруповерт", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, "Шуруповерт", "Шуруповерт", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, "Аккумуляторная отвертка", "Аккумуляторная отвертка", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, "Аккумуляторный гайковерт", "Гайковерт", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, "Гайковерт", "Гайковерт", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, @"\([\d\.]+\)", "");

            name = Regex.Replace(name, "ИНТЕРСКОЛ", "INTERSKOL", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, "ЗУБР", "ZUBR", RegexOptions.IgnoreCase);

            name = Regex.Replace(name, @"\(без аккумулятора и з/у\)", "(without battery and charger)", RegexOptions.IgnoreCase);

            name = name.Trim();

            return new ComplexName
            {
                Name = name,
                Class = baseResult.Class,
            };
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
                case ProductTypeName.Screwdriver:
                    productRecordHelper = new GeneralScrewdriverProductRecordHelper();
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
        public override ComplexName ProcessName(ProductRecord productRecord)
        {
            var baseResult = base.ProcessName(productRecord);

            var name = baseResult.Name;

            var match = Regex.Match(name, @"\[(.*)\]");
            if (match.Success)
            {
                name = "Блок питания " + string.Format("{0} {1}", productRecord.Brand, match.Groups[1].Value);
                var index = name.IndexOf("/");
                if (index >= 0)
                {
                    name = name.Substring(0, index);
                }
            }

            return new ComplexName
            {
                Name = name,
                Class = baseResult.Class,
            };
        }
    }
}
