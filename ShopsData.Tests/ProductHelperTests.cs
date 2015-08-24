using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using DataCollectorCore.DataObjects;

using Newtonsoft.Json;

using NUnit.Framework;

using TestDataCollector;

namespace ShopsData.Tests
{
    [TestFixture]
    public class ProductHelperTests
    {
        private static readonly Dictionary<string, HelperTest> Tests = new Dictionary<string, HelperTest>
        {
            {
                "monitor",
                new HelperTest
                {
                    FirstSetting = new TestSetting
                    {
                        FilePath = "TestData/Monitor_Dns.txt",
                        Helper = new DnsMonitorHelper(),
                    },
                    SecondSetting = new TestSetting
                    {
                        FilePath = "TestData/Monitor_Citilink.txt",
                        Helper = new CitilinkMonitorHelper(),
                    },
                }
            }
        };

        [Test]
        public void GeneralProductHelperMotheboardTest()
        {
            var testName = "monitor";

            var test = Tests[testName];

            var firstNames = GetNames(test.FirstSetting);
            var secondNames = GetNames(test.SecondSetting);

            using (var writer = File.CreateText("match.txt"))
            {
                PrintMatch(firstNames, secondNames, writer);

                writer.WriteLine();
                writer.WriteLine();

                PrintMatch(secondNames, firstNames, writer);
            }
        }

        private List<string> GetNames(TestSetting setting)
        {
            return GetProductRecords(setting.FilePath)
                .Select(pr => setting.Helper.GenerateSourceProduct(new DataSource(), pr).Name)
                .OrderBy(x => x).ToList();
        }

        private class HelperTest
        {
            public TestSetting FirstSetting { get; set; }

            public TestSetting SecondSetting { get; set; }
        }

        private class TestSetting
        {
            public string FilePath { get; set; }

            public IProductRecordHelper Helper { get; set; }
        }

        private static void PrintMatch(List<string> list1, List<string> list2, StreamWriter writer)
        {
            var productHelper = new GeneralProductHelper();
            var products = list1.Select(x => new Product { Name = x }).ToArray();
            foreach (var name in list2)
            {
                var sourceProduct = new SourceProduct { Name = name };
                var matchResult = productHelper.FindMatch(sourceProduct, products);
                var result = matchResult.Success ? matchResult.Product.Name : "NO MATCH";
                writer.WriteLine("{0,-50} -- {1,-50}", name, result);
            }
        }

        private List<ProductRecord> GetProductRecords(string filePath)
        {
            List<ProductRecord> list;
            using (var reader = File.OpenText(filePath))
            {
                list = JsonConvert.DeserializeObject<List<ProductRecord>>(reader.ReadToEnd());
            }

            return list;
        }
    }
}
