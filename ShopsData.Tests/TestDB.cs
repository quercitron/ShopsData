using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

using DataCollectorCore.DataObjects;

using NUnit.Framework;

using PostgreDAL;

namespace ShopsData.Tests
{
    [TestFixture]
    public class TestDB
    {
        public const string TestDbName = "shops";

        #region ProductType

        [Test]
        public void ProductTypeAddTest()
        {
            var dataStore = GetDataStore();
            var newProductType = "New Type " + Guid.NewGuid();
            dataStore.AddProductType(newProductType);
        }

        private ShopsDataStore GetDataStore()
        {
            return new ShopsDataStore(TestDbName);
        }

        [Test]
        public void ProductTypeGetTest()
        {
            var dataStore = new ShopsDataStore(TestDbName);
            dataStore.GetProductTypes();
        }

        [Test]
        public void ProductTypesTest()
        {
            var newTypeCount = 10;

            var dataStore = new ShopsDataStore(TestDbName);

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

        [Test]
        public void ProductTypeNameIsUniqueTest()
        {
            var dataStore = new ShopsDataStore(TestDbName);
            var newProductType = "New Type " + Guid.NewGuid();
            dataStore.AddProductType(newProductType);
            Assert.Throws(Is.InstanceOf(typeof(DbException)), () => dataStore.AddProductType(newProductType));
        }

        #endregion

        #region DataSource

        [Test]
        public void DataSourceAddTest()
        {
            var dataStore = new ShopsDataStore(TestDbName);
            var newDataSource = "New Type " + Guid.NewGuid();
            dataStore.AddDataSource(newDataSource);
        }

        [Test]
        public void DataSourceGetTest()
        {
            var dataStore = new ShopsDataStore(TestDbName);
            dataStore.GetDataSources();
        }

        [Test]
        public void DataSourcesTest()
        {
            var newDataSourceCount = 10;

            var dataStore = new ShopsDataStore(TestDbName);

            var initialDataSources = dataStore.GetDataSources().Select(pt => pt.Name);

            var newDataSources = new List<string>();
            for (int i = 0; i < newDataSourceCount; i++)
            {
                newDataSources.Add("New Type " + Guid.NewGuid());
            }

            foreach (var newType in newDataSources)
            {
                dataStore.AddDataSource(newType);
            }

            var mergedDataSources = dataStore.GetDataSources().Select(pt => pt.Name);

            foreach (var initialType in initialDataSources)
            {
                Assert.That(
                    mergedDataSources.Contains(initialType),
                    string.Format("Initial Data Source {0} is missing", initialType));
            }

            foreach (var newType in newDataSources)
            {
                Assert.That(
                    mergedDataSources.Contains(newType),
                    string.Format("New Data Source {0} is missing", newType));
            }
        }

        [Test]
        public void DataSourceNameIsUniqueTest()
        {
            var dataStore = new ShopsDataStore(TestDbName);
            var newDataSource = "New Type " + Guid.NewGuid();
            dataStore.AddDataSource(newDataSource);
            Assert.Throws(Is.InstanceOf(typeof(DbException)), () => dataStore.AddDataSource(newDataSource));
        }

        #endregion

        #region Products

        [Test]
        public void ProductAddTest()
        {
            var dataStore = new ShopsDataStore(TestDbName);

            var productTypeId = dataStore.GetProductTypes().First().ProductTypeId;

            var product = new Product();
            product.Name = "New Product " + Guid.NewGuid();
            product.ProductTypeId = productTypeId;

            dataStore.AddProduct(product);
        }

        [Test]
        public void ProductGetTest()
        {
            var dataStore = new ShopsDataStore(TestDbName);
            var products = dataStore.GetProducts();
            Assert.That(products, Is.Not.Empty);
        }

        [Test]
        public void ProductGetByTypeNameTest()
        {
            var dataStore = new ShopsDataStore(TestDbName);

            var productType = dataStore.GetProductTypes().First();

            var productsByName = dataStore.GetProducts(productType.Name);
            Assert.That(productsByName, Is.Not.Empty);

            var productsById = dataStore.GetProducts(productType.ProductTypeId);
            Assert.That(productsById, Is.Not.Empty);

            Assert.That(productsByName.Count, Is.EqualTo(productsById.Count));
        }

        #endregion

        #region Product Records

        [Test]
        public void ProductRecordAddTest()
        {
            var dataStore = new ShopsDataStore(TestDbName);

            var productId = dataStore.GetProducts().First().ProductId;

            var productRecord = new ProductRecord();
            productRecord.ProductId = productId;
            productRecord.Name = "New Record " + Guid.NewGuid();
            productRecord.Description = "New Description " + Guid.NewGuid();
            productRecord.Price = new Random().Next(100) + 1;
            productRecord.Rating = (float)(new Random().NextDouble() * 5);
            productRecord.AmountAvailable = new Random().Next(10);
            productRecord.Timestamp = DateTime.UtcNow;

            dataStore.AddProductRecord(productRecord);
        }

        #endregion
    }
}
