using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using PostgreDAL;

namespace ShopsData.Tests
{
    [TestFixture]
    public class TestDB
    {
        [Test]
        public void TestProductTypes()
        {
            var newTypeCount = 10;

            var dataStore = new ShopsDataStore(null);

            var initialTypes = dataStore.GetProductTypes().Select(pt => pt.Name);

            var newTypes = new List<string>();
            for (int i = 0; i < newTypeCount; i++)
            {
                newTypes.Add("New Type " + Guid.NewGuid());
            }

            foreach (var newType in newTypes)
            {
                dataStore.AddProductType(newType);
            }

            var mergedTypes = dataStore.GetProductTypes().Select(pt => pt.Name);

            foreach (var initialType in initialTypes)
            {
                Assert.That(
                    mergedTypes.Contains(initialType),
                    string.Format("Initial Type {0} is missing", initialType));
            }

            foreach (var newType in newTypes)
            {
                Assert.That(
                    mergedTypes.Contains(newType),
                    string.Format("New Type {0} is missing", newType));
            }
        }
    }
}
