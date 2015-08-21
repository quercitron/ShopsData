using DataCollectorCore;
using DataCollectorCore.DataObjects;

namespace TestDataCollector
{
    public interface ISourceManager
    {
        IShopDataCollector GetDataCollector();

        IProductRecordHelper GetProductRecordHelper(ProductType productType);

        IProductHelper GetProductHelper(ProductType productType);
    }
}
