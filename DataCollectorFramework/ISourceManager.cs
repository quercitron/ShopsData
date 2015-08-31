using DataCollectorCore;
using DataCollectorCore.DataObjects;

namespace DataCollectorFramework
{
    public interface ISourceManager
    {
        IShopDataCollector GetDataCollector();

        IProductRecordHelper GetProductRecordHelper(ProductType productType);

        IProductHelper GetProductHelper(ProductType productType);
    }
}
