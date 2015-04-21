using System.IO;

using DataCollectors;

namespace TestDataCollector
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var dataCollector = new DnsDataCollector();

            var data = dataCollector.GetShopData("motherboard");

            using (var writer = File.CreateText("output.txt"))
            {
                if (data.Success)
                {
                    foreach (var item in data.Products)
                    {
                        writer.WriteLine(item.Name);
                        writer.WriteLine(item.Price);
                        writer.WriteLine(item.Description);
                        writer.WriteLine();
                    }
                }
                else
                {
                    writer.WriteLine(data.Message);
                }
            }
        }
    }
}
