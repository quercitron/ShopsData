using System.Collections.Generic;

using DataCollectorCore;
using DataCollectorCore.DataObjects;

namespace PostgreDAL
{
    public interface IShopsDataStore
    {
        List<ProductType> GetProductTypes();

        void AddProductType(string productType);
    }
}
