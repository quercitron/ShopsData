using DataCollectorCore.DataObjects;

namespace TestDataCollector
{
    public interface IProductRecordHelper
    {
        string GetKey(ProductRecord productRecord);

        SourceProduct GenerateSourceProduct(DataSource dataSource, ProductRecord productRecord);
    }

    class GeneralMotherboardProductRecordHelper : IProductRecordHelper
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
                Name = productRecord.Name.Replace("Материнская плата", "").Trim(),
                OriginalName = productRecord.Name,
            };
        }
    }
}
