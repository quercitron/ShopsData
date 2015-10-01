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
            return new ShopsDataStore();
        }

        [Test]
        public void ProductTypeGetTest()
        {
            var dataStore = new ShopsDataStore();
            dataStore.GetProductTypes();
        }

        [Test]
        public void ProductTypesTest()
        {
            var newTypeCount = 10;

            var dataStore = new ShopsDataStore();

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
            var dataStore = new ShopsDataStore();
            var newProductType = "New Type " + Guid.NewGuid();
            dataStore.AddProductType(newProductType);
            Assert.Throws(Is.InstanceOf(typeof(DbException)), () => dataStore.AddProductType(newProductType));
        }

        #endregion

        #region DataSource

        [Test]
        public void DataSourceAddTest()
        {
            var dataStore = new ShopsDataStore();
            var newDataSource = "New Type " + Guid.NewGuid();
            dataStore.AddDataSource(newDataSource);
        }

        [Test]
        public void DataSourceGetTest()
        {
            var dataStore = new ShopsDataStore();
            dataStore.GetDataSources();
        }

        [Test]
        public void DataSourcesTest()
        {
            var newDataSourceCount = 10;

            var dataStore = new ShopsDataStore();

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
            var dataStore = new ShopsDataStore();
            var newDataSource = "New Type " + Guid.NewGuid();
            dataStore.AddDataSource(newDataSource);
            Assert.Throws(Is.InstanceOf(typeof(DbException)), () => dataStore.AddDataSource(newDataSource));
        }

        #endregion

        #region Products

        [Test]
        public void ProductAddTest()
        {
            var dataStore = new ShopsDataStore();

            var productTypeId = dataStore.GetProductTypes().First().ProductTypeId;

            var product = new Product();
            product.Name = "New Product " + Guid.NewGuid();
            product.ProductTypeId = productTypeId;

            dataStore.AddProduct(product);
        }

        [Test]
        public void ProductGetTest()
        {
            var dataStore = new ShopsDataStore();
            var products = dataStore.GetProducts();
            Assert.That(products, Is.Not.Empty);
        }

        [Test]
        public void ProductGetByTypeNameTest()
        {
            var dataStore = new ShopsDataStore();

            var productType = dataStore.GetProductTypes().First();

            var productsByName = dataStore.GetProducts(productType.Name);
            Assert.That(productsByName, Is.Not.Empty);

            var productsById = dataStore.GetProducts(productType.ProductTypeId);
            Assert.That(productsById, Is.Not.Empty);

            Assert.That(productsByName.Count, Is.EqualTo(productsById.Count));
        }

        #endregion

        #region locations

        [Test]
        [Ignore]
        public void LocationAddTest()
        {
            var location = new Location();
            location.LocationId = 1;
            location.Name = "City";

            var dataStore = new ShopsDataStore();
            dataStore.AddLocation(location);
        }

        #endregion

        #region Product Records

        [Test]
        public void ProductRecordAddTest()
        {
            var dataStore = new ShopsDataStore();

            var productId = dataStore.GetProducts().First().ProductId;

            var productRecord = new ProductRecord();
            productRecord.SourceProductId = productId;
            productRecord.Name = "New Record " + Guid.NewGuid();
            productRecord.Description = "New Description " + Guid.NewGuid();
            productRecord.Price = new Random().Next(100) + 1;
            productRecord.Rating = (float)(new Random().NextDouble() * 5);
            productRecord.AmountAvailable = new Random().Next(10);
            productRecord.Timestamp = DateTime.UtcNow;
            productRecord.LocationId = 1;

            dataStore.AddProductRecord(productRecord);
        }

        #endregion

        [Test]
        public void GetCurrentDataTest()
        {
            var dataStore = new ShopsDataStore();
            var products = dataStore.GetCurrentData(3, 3);
            Assert.That(products.Count, Is.GreaterThan(100));
        }
    }
}
