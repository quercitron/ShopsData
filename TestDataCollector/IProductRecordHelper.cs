using DataCollectorCore.DataObjects;

namespace TestDataCollector
{
    public interface IProductRecordHelper
    {
        string GetKey(ProductRecord productRecord);

        SourceProduct GenerateSourceProduct(ProductRecord productRecord);
    }
}
