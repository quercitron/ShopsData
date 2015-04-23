using System;
using System.Collections.Generic;

using DataCollectorCore.DataObjects;

namespace PostgreDAL
{
    public class ShopsDataStore : IShopsDataStore
    {
        private readonly string _dbName;

        public ShopsDataStore(string dbName)
        {
            if (dbName == null)
            {
                throw new ArgumentNullException("dbName");
            }

            _dbName = dbName;
        }

        public List<ProductType> GetProductTypes()
        {
            throw new NotImplementedException();
        }

        public void AddProductType(string productType)
        {
            
        }
    }
}
