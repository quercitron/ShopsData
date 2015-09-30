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
                OriginalName = productRecord.Name,
                Brand = productRecord.Brand,
                Timestamp = productRecord.Timestamp,
            };
        }

        // todo: do we really need to replace colors?
        private static readonly List<Tuple<string, string>> ColorsReplace = new List<Tuple<string, string>>
        {
            new Tuple<string, string>("белый", "White"),
            new Tuple<string, string>("бирюзовый", "Turquoise"),
            new Tuple<string, string>("красный", "Red"),
            new Tuple<string, string>("серебристый", "Silver"),
            new Tuple<string, string>("серый", "Gray"),
            new Tuple<string, string>("черный", "Black"),
            new Tuple<string, string>("чёрный", "Black"),
        };

        protected virtual ComplexName ProcessName(ProductRecord productRecord)
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
                Name = name,
                Class = colors.Any() ? string.Join(" ", colors) : null,
            };
        }
    }

    public class ComplexName
    {
        public string Name { get; set; }

        public string Class { get; set; }
    }

    class GeneralMotherboardProductRecordHelper : GeneralProductRecordHelper
    {
        protected override ComplexName ProcessName(ProductRecord productRecord)
        {
            var baseResult = base.ProcessName(productRecord);
            return new ComplexName
            {
                Name = Regex.Replace(baseResult.Name, "Материнская плата", "", RegexOptions.IgnoreCase).Trim(),
                Class = baseResult.Class,
            };
        }
    }

    public class GeneralPowerSupplyProductRecordHelper : GeneralProductRecordHelper
    {
        protected override ComplexName ProcessName(ProductRecord productRecord)
        {
            var baseResult = base.ProcessName(productRecord);

            var name = baseResult.Name;
            name = Regex.Replace(name, "Блок питания ", "", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, "^ATX ", "", RegexOptions.IgnoreCase);

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

            var startPowerPattern = @"^(\d+W)\s\b";
            matchResult = Regex.Match(name, startPowerPattern, RegexOptions.IgnoreCase);
            if (matchResult.Success)
            {
                name = Regex.Replace(name, startPowerPattern, "", RegexOptions.IgnoreCase);
                name = name + " " + matchResult.Groups[1].Value;
            }

            powerPattern = @"\b(\d+)W\b";
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

    class GeneralMonitorProductRecordHelper : GeneralProductRecordHelper
    {
        protected override ComplexName ProcessName(ProductRecord productRecord)
        {
            var baseResult = base.ProcessName(productRecord);

            var name = baseResult.Name;
            // todo: add unit tests?
            name = Regex.Replace(name, "^[0-9.,]+\"", "");
            name = Regex.Replace(name, @"\s[0-9.,]+""", "");
            name = Regex.Replace(name, "Монитор ЖК ", "", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, "Монитор ", "", RegexOptions.IgnoreCase);
            name = name.Trim(' ', '.', ',');

            return new ComplexName
            {
                Name = name,
                Class = baseResult.Class,
            };
        }
    }
}
