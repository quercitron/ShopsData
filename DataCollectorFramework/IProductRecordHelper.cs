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
            };
        }

        protected virtual string ProcessName(ProductRecord productRecord)
        {
            return productRecord.Name;
        }
    }

    class GeneralMotherboardProductRecordHelper : GeneralProductRecordHelper
    {
        protected override string ProcessName(ProductRecord productRecord)
        {
            return productRecord.Name.Replace("Материнская плата", "").Trim();
        }
    }

    class GeneralPowerSupplyProductRecordHelper : GeneralProductRecordHelper
    {
        protected override string ProcessName(ProductRecord productRecord)
        {
            return productRecord.Name.Replace("Блок питания ", "").Trim();
        }
    }

    class GeneralMonitorProductRecordHelper : GeneralProductRecordHelper
    {
        protected override string ProcessName(ProductRecord productRecord)
        {
            return productRecord.Name.Replace("Монитор ", "").Trim();
        }
    }
}
