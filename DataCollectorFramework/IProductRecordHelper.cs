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
                Name = baseResult.Name.Replace("Материнская плата", "").Trim(),
                Class = baseResult.Class,
            };
        }
    }

    class GeneralPowerSupplyProductRecordHelper : GeneralProductRecordHelper
    {
        protected override ComplexName ProcessName(ProductRecord productRecord)
        {
            var baseResult = base.ProcessName(productRecord);
            return new ComplexName
            {
                Name = baseResult.Name.Replace("Блок питания ", "").Trim(),
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
            name = name.Replace("Монитор ЖК ", "").Replace("Монитор ", "").Trim(' ', '.', ',');

            return new ComplexName
            {
                Name = name,
                Class = baseResult.Class,
            };
        }
    }
}
