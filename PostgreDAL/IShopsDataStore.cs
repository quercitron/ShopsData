using System.Collections.Generic;

namespace PostgreDAL
{
    public interface IShopsDataStore
    {
        List<string> GetProductTypes();

        void AddProductType(string productType);
    }
}
