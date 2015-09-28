using System;
using System.Collections.Generic;
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
            return new SourceProduct
            {
                DataSourceId = dataSource.DataSourceId,
                Key = GetKey(productRecord),
                Name = ProcessName(productRecord),
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

        protected virtual string ProcessName(ProductRecord productRecord)
        {
            var name = productRecord.Name;
            foreach (var color in ColorsReplace)
            {
                string pattern = string.Format(@"\b{0}\b", color.Item1);
                name = Regex.Replace(name, pattern, color.Item2, RegexOptions.IgnoreCase);
            }
            return name;
        }
    }

    class GeneralMotherboardProductRecordHelper : GeneralProductRecordHelper
    {
        protected override string ProcessName(ProductRecord productRecord)
        {
            var name = base.ProcessName(productRecord);
            return name.Replace("Материнская плата", "").Trim();
        }
    }

    class GeneralPowerSupplyProductRecordHelper : GeneralProductRecordHelper
    {
        protected override string ProcessName(ProductRecord productRecord)
        {
            var name = base.ProcessName(productRecord);
            return name.Replace("Блок питания ", "").Trim();
        }
    }

    class GeneralMonitorProductRecordHelper : GeneralProductRecordHelper
    {
        protected override string ProcessName(ProductRecord productRecord)
        {
            var name = base.ProcessName(productRecord);
            // todo: add unit tests?
            name = Regex.Replace(name, "^[0-9.,]+\"", "");
            return name.Replace("Монитор ЖК ", "").Replace("Монитор ", "").Trim();
        }
    }
}
