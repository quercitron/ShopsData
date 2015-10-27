using System;
using System.Collections.Generic;
using System.Linq;
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
                case "key":
                    sourceManager = new KeySourceManager();
                    break;
                default:
                    var message = string.Format("Data source '{0}' is not supported", sourceName);
                    throw new NotSupportedException(message);
            }
            return sourceManager;
        }
    }

    public class UlmartSourceManager : GeneralSourceManager
    {
        public override IShopDataCollector GetDataCollector()
        {
            return new UlmartDataCollector();
        }

        public override IProductRecordHelper GetProductRecordHelper(ProductType productType)
        {
            switch (productType.Name)
            {
                case ProductTypeName.PowerSupply:
                    return new UlmartPowerSupplyHelper();
                case ProductTypeName.SSD:
                    return new UlmartSsdHelper();
                case ProductTypeName.CPU:
                    return new UlmartCpuHelper();
            }
            return base.GetProductRecordHelper(productType);
        }
    }

    public class UlmartCpuHelper : GeneralCpuProductRecordHelper
    {
        public override ComplexName ProcessName(ProductRecord productRecord)
        {
            var baseResult = base.ProcessName(productRecord);

            var name = baseResult.Name;
            var code = baseResult.Code;

            var pattern = @",\s?(AD\w+),";
            var match = Regex.Match(name, pattern, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                code = match.Groups[1].Value;
                name = Regex.Replace(name, pattern, "", RegexOptions.IgnoreCase);
            }

            return new ComplexName { Name = name, Code = code, Class = baseResult.Class };
        }
    }

    public class UlmartSsdHelper : GeneralSSDProductRecordHelper
    {
        public override ComplexName ProcessName(ProductRecord productRecord)
        {
            var baseResult = base.ProcessName(productRecord);

            var name = baseResult.Name;
            var code = baseResult.Code;

            name = name.Replace(",", "");

            var pattern = @"^(.*\s)(\S+)$";
            var match = Regex.Match(name, pattern);
            if (match.Success)
            {
                name = match.Groups[1].Value.Trim();
                code = match.Groups[2].Value.Trim();
            }

            pattern = @"\s\d+GB\b";
            match = Regex.Match(name, pattern, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                name = Regex.Replace(name, pattern, "", RegexOptions.IgnoreCase);
                name += match.Groups[0].Value;
            }

            return new ComplexName { Name = name, Code = code, Class = baseResult.Class };
        }
    }

    public class KeyMonitorHelper : GeneralMonitorProductRecordHelper
    {
        public override ComplexName ProcessName(ProductRecord productRecord)
        {
            var baseResult = base.ProcessName(productRecord);

            var name = baseResult.Name;
            var productClass = baseResult.Class;

            name = Regex.Replace(name, "Материнская плата MB ", "Материнская плата ", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, "LG Flatron", "LG", RegexOptions.IgnoreCase);

            var newColors = new List<string>();
            foreach (var color in ColorsReplace)
            {
                var pattern = string.Format(@"\b{0}\b", color.Item2);
                if (Regex.IsMatch(name, pattern, RegexOptions.IgnoreCase))
                {
                    name = Regex.Replace(name, pattern, "", RegexOptions.IgnoreCase);
                    newColors.Add(color.Item2);
                }
            }
            if (newColors.Any())
            {
                productClass += " " + String.Join(" ", newColors);
                productClass = productClass.Trim();
            }
            name = name.Trim(' ', ',', '.', '&', '/', '\\');

            return new ComplexName { Name = name, Class = productClass };
        }
    }

    public class KeyMotherboardHelper : GeneralMotherboardProductRecordHelper
    {
        public override ComplexName ProcessName(ProductRecord productRecord)
        {
            var baseResult = base.ProcessName(productRecord);

            var name = baseResult.Name;
            name = Regex.Replace(name, "Материнская плата MB ", "Материнская плата ", RegexOptions.IgnoreCase);

            return new ComplexName { Name = name, Class = baseResult.Class };
        }
    }

    public class KeySourceManager : GeneralSourceManager
    {
        public override IShopDataCollector GetDataCollector()
        {
            return new KeyDataCollector();
        }

        public override IProductRecordHelper GetProductRecordHelper(ProductType productType)
        {
            switch (productType.Name)
            {
                case ProductTypeName.Motherboard:
                    return new KeyMotherboardHelper();
                case ProductTypeName.Monitor:
                    return new KeyMonitorHelper();
                case ProductTypeName.PowerSupply:
                    return new KeyPowerSupplyHelper();
                case ProductTypeName.SSD:
                    return new KeySsdHelper();
            }
            return base.GetProductRecordHelper(productType);
        }
    }

    public class KeySsdHelper : GeneralSSDProductRecordHelper
    {
        public override ComplexName ProcessName(ProductRecord productRecord)
        {
            var baseResult = base.ProcessName(productRecord);

            var name = baseResult.Name;

            var pattern = @"\s\d+GB\b";
            var match = Regex.Match(name, pattern, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                name = Regex.Replace(name, pattern, "", RegexOptions.IgnoreCase);
                name += match.Groups[0].Value;
            }

            return new ComplexName { Name = name, Class = baseResult.Class, Code = baseResult.Code };
        }
    }

    public abstract class GeneralSourceManager : ISourceManager
    {
        public abstract IShopDataCollector GetDataCollector();

        public virtual IProductRecordHelper GetProductRecordHelper(ProductType productType)
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
                case ProductTypeName.SSD:
                    productRecordHelper = new GeneralSSDProductRecordHelper();
                    break;
                case ProductTypeName.Headset:
                    productRecordHelper = new GeneralHeadsetProductRecordHelper();
                    break;
                case ProductTypeName.CPU:
                    productRecordHelper = new GeneralCpuProductRecordHelper();
                    break;
                case ProductTypeName.Jigsaw:
                    productRecordHelper = new GeneralJigsawProductRecordHelper();
                    break;
                case ProductTypeName.Headphones:
                    productRecordHelper = new GeneralHeadphonesProductRecordHelper();
                    break;
                default:
                    var message = string.Format("No ProductRecordHelper for product type {0}.", productType.Name);
                    throw new NotSupportedException(message);
            }

            return productRecordHelper;
        }

        public virtual IProductHelper GetProductHelper(ProductType productType)
        {
            return new GeneralProductHelper();
        }
    }

    public class GeneralHeadphonesProductRecordHelper : GeneralHeadsetProductRecordHelper
    {
    }

    public class GeneralJigsawProductRecordHelper : GeneralProductRecordHelper
    {
        public override ComplexName ProcessName(ProductRecord productRecord)
        {
            var baseResult = base.ProcessName(productRecord);

            var name = baseResult.Name.Trim();
            name = Regex.Replace(name, "Лобзик", "Лобзик", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, "Makita", "Makita", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, "Bosch", "Bosch", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, "SKIL", "SKIL", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, "Black & Decker", "Black & Decker", RegexOptions.IgnoreCase);

            return new ComplexName { Name = name, Class = baseResult.Class, Code = baseResult.Code };
        }
    }

    public class GeneralCpuProductRecordHelper : GeneralProductRecordHelper
    {
        public override ComplexName ProcessName(ProductRecord productRecord)
        {
            var baseResult = base.ProcessName(productRecord);

            var name = baseResult.Name.Trim();
            name = Regex.Replace(name, "Процессор", "Процессор", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, "Pentium Dual-Core", "Pentium", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, "Intel", "Intel", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, @"\(OEM\)", "OEM", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, @"\(BOX\)", "BOX", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, @", OEM\b", " OEM", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, @", BOX\b", " BOX", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, @"\bAMD\b", "AMD", RegexOptions.IgnoreCase);
            if (!Regex.IsMatch(name, @"\bOEM\b", RegexOptions.IgnoreCase) &&
                !Regex.IsMatch(name, @"\bBOX\b", RegexOptions.IgnoreCase))
            {
                if (Regex.IsMatch(productRecord.Description, @"\bBOX\b", RegexOptions.IgnoreCase))
                {
                    name += " BOX";
                }
                else // if (Regex.IsMatch(productRecord.Description, @"\bOEM\b", RegexOptions.IgnoreCase))
                {
                    name += " OEM";
                }
            }

            return new ComplexName { Name = name, Class = baseResult.Class, Code = baseResult.Code };
        }
    }

    public class GeneralHeadsetProductRecordHelper : GeneralProductRecordHelper
    {
        public override ComplexName ProcessName(ProductRecord productRecord)
        {
            var baseResult = base.ProcessName(productRecord);

            var name = baseResult.Name;
            name = Regex.Replace(name, @"Наушники", "Наушники", RegexOptions.IgnoreCase);
            if (!Regex.IsMatch(name, @"A4\s?Tech", RegexOptions.IgnoreCase))
            {
                name = Regex.Replace(name, @"\bA4\b", "A4Tech", RegexOptions.IgnoreCase);
            }
            name = Regex.Replace(name, @"A4 Tech", "A4Tech", RegexOptions.IgnoreCase);
            name = name.Trim();

            return new ComplexName { Name = name, Class = baseResult.Class, Code = baseResult.Code };
        }
    }

    public class GeneralSSDProductRecordHelper : GeneralProductRecordHelper
    {
        public override ComplexName ProcessName(ProductRecord productRecord)
        {
            var baseResult = base.ProcessName(productRecord);

            var name = baseResult.Name;
            name = Regex.Replace(name, @"Накопитель SSD", "Накопитель SSD", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, @"жесткий диск SSD", "Накопитель SSD", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, @"SSD накопитель", "Накопитель SSD", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, @"\s?Гб", "GB", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, @"\s?Тб", "TB", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, "2.5\"", "", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, "2,5\"", "", RegexOptions.IgnoreCase);
            name = name.Replace("  ", " ");
            name = Regex.Replace(name, "Series", "", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, @"\s\s", @" ");
            name = Regex.Replace(name, @"\bmSATA\b", "", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, @"\bSATA/s\b", "", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, @"\bSATA\b", "", RegexOptions.IgnoreCase);
            name = name.Trim();

            return new ComplexName { Name = name, Class = baseResult.Class, Code = baseResult.Code };
        }
    }

    public class CitilinkSourceManager : GeneralSourceManager
    {
        public override IProductRecordHelper GetProductRecordHelper(ProductType productType)
        {
            switch (productType.Name)
            {
                case ProductTypeName.SSD:
                    return new CitilinkSSDProductRecordHelper();
            }
            return base.GetProductRecordHelper(productType);
        }

        public override IShopDataCollector GetDataCollector()
        {
            return new CitilinkDataCollector();
        }
    }

    public class CitilinkSSDProductRecordHelper : GeneralSSDProductRecordHelper
    {
        public override ComplexName ProcessName(ProductRecord productRecord)
        {
            var baseResult = base.ProcessName(productRecord);

            var name = baseResult.Name;
            var code = baseResult.Code;

            var pattern = @"^(.*\s)(\d+\s)(\S+)$";
            var match = Regex.Match(name, pattern);
            if (match.Success)
            {
                name = Regex.Replace(name, pattern, m => m.Groups[1].Value + m.Groups[3].Value).Trim();
            }

            pattern = @"^(.*\s)(\S+\s)(\S+)$";
            match = Regex.Match(name, pattern);
            if (match.Success)
            {
                code = match.Groups[2].Value.Trim();
                name = Regex.Replace(name, pattern, m => m.Groups[1].Value + m.Groups[3].Value).Trim();
            }

            return new ComplexName
            {
                Name = name,
                Class = baseResult.Class,
                Code = code,
            };
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

    public class DnsSourceManager : GeneralSourceManager
    {
        public override IShopDataCollector GetDataCollector()
        {
            return new DnsDataCollector();
        }

        public override IProductRecordHelper GetProductRecordHelper(ProductType productType)
        {
            IProductRecordHelper productRecordHelper = null;

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
                case ProductTypeName.SSD:
                    productRecordHelper = new DnsSSDProductRecordHelper();
                    break;
            }

            return productRecordHelper ?? base.GetProductRecordHelper(productType);
        }

        public override IProductHelper GetProductHelper(ProductType productType)
        {
            return new GeneralProductHelper();
        }
    }

    public class DnsSSDProductRecordHelper : GeneralSSDProductRecordHelper
    {
        public override ComplexName ProcessName(ProductRecord productRecord)
        {
            var baseResult = base.ProcessName(productRecord);

            var name = baseResult.Name;
            var code = baseResult.Code;

            var pattern = @"\[(.*)\]";
            var match = Regex.Match(name, pattern);
            if (match.Success)
            {
                code = match.Groups[1].Value;
                name = Regex.Replace(name, pattern, "").Trim();
            }

            var descr = productRecord.Description;
            descr = Regex.Replace(descr, @"Гб\b", "GB", RegexOptions.IgnoreCase);
            descr = Regex.Replace(descr, @"Тб\b", "TB", RegexOptions.IgnoreCase);
            pattern = @"\b\d+\s?(GB|TB)\b";
            match = Regex.Match(descr, pattern, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                name += " " + match.Groups[0].Value.Replace(" ", "");
            }

            return new ComplexName
            {
                Name = name,
                Class = baseResult.Class,
                Code = code,
            };
        }
    }

    public class DnsPowerSupplyHelper : GeneralPowerSupplyProductRecordHelper
    {
        public override ComplexName ProcessName(ProductRecord productRecord)
        {
            var baseResult = base.ProcessName(productRecord);

            var name = baseResult.Name;

            var pattern = @"\[(.*)\]";
            var match = Regex.Match(name, pattern);
            if (match.Success)
            {
                var model = match.Groups[1].Value;

                if (Regex.IsMatch(model, "CP-.*", RegexOptions.IgnoreCase))
                {
                    name = Regex.Replace(name, pattern, "").Trim();
                }
                else
                {
                    name = "Блок питания " + string.Format("{0} {1}", productRecord.Brand, model);
                    var index = name.IndexOf("/");
                    if (index >= 0)
                    {
                        name = name.Substring(0, index);
                    }
                }
            }

            return new ComplexName
            {
                Name = name,
                Class = baseResult.Class,
            };
        }
    }

    public class KeyPowerSupplyHelper : GeneralPowerSupplyProductRecordHelper
    {
        public override ComplexName ProcessName(ProductRecord productRecord)
        {
            var baseResult = base.ProcessName(productRecord);

            var name = baseResult.Name;
            name = Regex.Replace(name, "Semi-modular", "", RegexOptions.IgnoreCase);
            var pattern = "FSP Group";
            if (Regex.IsMatch(name, pattern, RegexOptions.IgnoreCase))
            {
                name = Regex.Replace(name, pattern, "FSP", RegexOptions.IgnoreCase);
                var regex = new Regex(@"(\d+)\b", RegexOptions.RightToLeft);
                name = regex.Replace(name, "$1PNR", 1);
            }
            name = name.Replace("  ", " ").Trim();

            return new ComplexName
            {
                Name = name,
                Class = baseResult.Class,
            };
        }
    }

    public class UlmartPowerSupplyHelper : GeneralPowerSupplyProductRecordHelper
    {
        public override ComplexName ProcessName(ProductRecord productRecord)
        {
            var baseResult = base.ProcessName(productRecord);

            var name = baseResult.Name;
            // блок питания ATX Corsair CX 500, CP-9020047-EU, 500W
            var pattern = @"CP-\d+-\w+";
            name = Regex.Replace(name, pattern, "", RegexOptions.IgnoreCase);
            name = name.Trim(' ', ',', '.');

            return new ComplexName
            {
                Name = name,
                Class = baseResult.Class,
            };
        }
    }
}
