using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using DataCollectorCore.DataObjects;

namespace DataCollectorFramework
{
    public interface IProductRecordHelper
    {
        string GetKey(ProductRecord productRecord);

        SourceProduct GenerateSourceProduct(DataSource dataSource, ProductRecord productRecord);
    }

    public class GeneralProductRecordHelper : IProductRecordHelper
    {
        public string GetKey(ProductRecord productRecord)
        {
            return productRecord.ExternalId;
        }

        public SourceProduct GenerateSourceProduct(DataSource dataSource, ProductRecord productRecord)
        {
            var processedName = ProcessName(productRecord);
            return new SourceProduct
            {
                DataSourceId = dataSource.DataSourceId,
                Key = GetKey(productRecord),
                Name = processedName.Name,
                Class = processedName.Class,
                Code = processedName.Code,
                OriginalName = productRecord.Name,
                Brand = productRecord.Brand,
                Timestamp = productRecord.Timestamp,
            };
        }

        // todo: do we really need to replace colors?
        public static readonly List<Tuple<string, string>> ColorsReplace = new List<Tuple<string, string>>
        {
            new Tuple<string, string>("белый", "White"),
            new Tuple<string, string>("бирюзовый", "Turquoise"),
            new Tuple<string, string>("красный", "Red"),
            new Tuple<string, string>("серебристый", "Silver"),
            new Tuple<string, string>("серый", "Gray"),
            new Tuple<string, string>("черный", "Black"),
            new Tuple<string, string>("чёрный", "Black"),
            new Tuple<string, string>("волосной", "Hairline"),
        };

        public virtual ComplexName ProcessName(ProductRecord productRecord)
        {
            var name = productRecord.Name;
            var colors = new List<String>();
            foreach (var color in ColorsReplace)
            {
                string pattern = string.Format(@"\b{0}\b", color.Item1);
                if (Regex.IsMatch(name, pattern, RegexOptions.IgnoreCase))
                {
                    name = Regex.Replace(name, pattern, "", RegexOptions.IgnoreCase);
                    colors.Add(color.Item2);
                }
            }
            return new ComplexName
            {
                Name = name.Trim(' ', '.', ','),
                Class = colors.Any() ? string.Join(" ", colors) : null,
            };
        }
    }

    public class ComplexName
    {
        public string Name { get; set; }

        public string Class { get; set; }

        public string Code { get; set; }
    }

    public class GeneralMotherboardProductRecordHelper : GeneralProductRecordHelper
    {
        public override ComplexName ProcessName(ProductRecord productRecord)
        {
            var baseResult = base.ProcessName(productRecord);

            var name = baseResult.Name;
            name = Regex.Replace(name, "Материнская плата", "Материнская плата", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, "Серверная материнская плата", "Серверная материнская плата", RegexOptions.IgnoreCase);
            if (!Regex.IsMatch(name, "Материнская плата", RegexOptions.IgnoreCase))
            {
                name = "Материнская плата " + name;
            }
            name = name.Trim();

            return new ComplexName
            {
                Name = name,
                Class = baseResult.Class,
            };
        }
    }

    public class GeneralPowerSupplyProductRecordHelper : GeneralProductRecordHelper
    {
        public override ComplexName ProcessName(ProductRecord productRecord)
        {
            var baseResult = base.ProcessName(productRecord);

            var name = baseResult.Name;
            name = Regex.Replace(name, "Блок питания БП ", "Блок питания ", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, "^БП ", "Блок питания ", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, "Блок питания", "Блок питания", RegexOptions.IgnoreCase);
            // [ATX350-PNR] -> [ATX-350PNR]
            name = Regex.Replace(name, @"\bATX-?(\d+)-?PNR", "ATX-$1PNR", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, @"ATX-", "ATX ", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, @"\bATX\s?", "", RegexOptions.IgnoreCase);

            var powerPattern = @"\b(\d+)W\b";
            var matchResult = Regex.Match(name, powerPattern, RegexOptions.IgnoreCase);
            if (matchResult.Success)
            {
                var value = matchResult.Groups[1].Value;
                var valueCount = Regex.Matches(name, value).Count;
                if (valueCount > 1)
                {
                    var regex = new Regex(powerPattern, RegexOptions.IgnoreCase);
                    name = regex.Replace(name, "", 1);
                }
            }

            var startPowerPattern = @"Блок питания (\d+W)\s\b";
            matchResult = Regex.Match(name, startPowerPattern, RegexOptions.IgnoreCase);
            if (matchResult.Success)
            {
                name = Regex.Replace(name, startPowerPattern, "Блок питания ", RegexOptions.IgnoreCase);
                name = name + " " + matchResult.Groups[1].Value;
            }

            powerPattern = @"(\d+)W\b";
            name = Regex.Replace(name, powerPattern, @"$1", RegexOptions.IgnoreCase);

            name = name.Replace("  ", " ");
            name = name.Trim(' ', '.', ',');

            return new ComplexName
            {
                Name = name,
                Class = baseResult.Class,
            };
        }
    }

    public class GeneralMonitorProductRecordHelper : GeneralProductRecordHelper
    {
        public override ComplexName ProcessName(ProductRecord productRecord)
        {
            var baseResult = base.ProcessName(productRecord);

            var name = baseResult.Name;
            // todo: add unit tests?
            name = Regex.Replace(name, "^[0-9.,]+\"", "");
            name = Regex.Replace(name, @"\s[0-9.,]+""", "");
            name = Regex.Replace(name, "Монитор ЖК", "Монитор", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, "Монитор", "Монитор", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, @"\(00/01\)", "", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, @"00\(01\)", "", RegexOptions.IgnoreCase);
            name = name.Replace("  ", " ");
            name = name.Trim(' ', '.', ',');

            return new ComplexName
            {
                Name = name,
                Class = baseResult.Class,
            };
        }
    }
}
