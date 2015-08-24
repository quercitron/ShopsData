using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataCollectorCore;
using DataCollectorCore.DataObjects;

using HtmlAgilityPack;

namespace DataCollectors
{
    public class CitilinkDataCollector : DataCollectorBase
    {
        public override string ShopName
        {
            get { return "Citilink"; }
        }

        protected override List<ProductRecord> GetProducts(string locationName, string url)
        {
            var result = new List<ProductRecord>();
            for (int pageNumber = 1; ; pageNumber++)
            {
                var pageUrl = string.Format(url, pageNumber);
                var products = ProcessCitilinkPage(pageUrl);
                if (products != null && products.Count != 0)
                {
                    result.AddRange(products);
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        private List<ProductRecord> ProcessCitilinkPage(string url)
        {
            //var uriString = string.Format("http://www.citilink.ru/catalog/computers_and_notebooks/parts/motherboards/?p={0}", pageNumber);
            Uri target = new Uri(url);

            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                response = client.PostAsync(target, null).Result;
            }

            /*var request = WebRequest.CreateHttp(target);

            string source;
            var response = request.GetResponse();*/


            /*if (response.ResponseUri.Equals("http://www.citilink.ru/catalog/"))
            {
                return null;
            }*/
            var responseStream = response.Content.ReadAsStreamAsync().Result;
            string source;
            using (var streamReader = new StreamReader(responseStream, Encoding.UTF8))
            {
                source = streamReader.ReadToEnd();
            }
            //var source = Encoding.GetEncoding(1251).GetString(responseBytes, 0, responseBytes.Length - 1); ;

            source = WebUtility.HtmlDecode(source);
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(source);

            return GetItems(document);
        }

        private List<ProductRecord> GetItems(HtmlDocument document)
        {
            var items = new List<ProductRecord>();

            var productRows =
                   document.DocumentNode.Descendants("div").First(x => x.Class() == "product_category_list")
                       .Descendants("tbody").First()
                       .Descendants("tr")
                       .Where(x => x.Class() != "banner_tr")
                       .ToArray();

            for (int i = 0; i < productRows.Length; i++)
            {
                var item = new ProductRecord();

                var row = productRows[i];

                var productNameTd = row.Child("td", "product_name");

                var titleNode = productNameTd.Descendants("a").First(x => x.Class() == "product_link__js");
                item.Name = titleNode.Attributes["title"].Value;

                var descriptionNode = productNameTd.Descendants("p").First(x => x.Class() == "short_description");
                item.Description = descriptionNode.InnerText;

                i++;
                row = productRows[i];

                var priceNode = row.Descendants("span").FirstOrDefault(x => x.Class() == "special");
                if (priceNode == null)
                {
                    priceNode = row.Descendants("span").FirstOrDefault(x => x.Class() == "standart");
                    if (priceNode == null)
                    {
                        break;
                    }
                }

                var priceNumNode = priceNode.Descendants("ins").First(x => x.Class() == "num");

                var priceStr = priceNumNode.InnerText.Replace(" ", "");
                if (!Regex.IsMatch(priceStr, @"^\d+$"))
                {
                    var d = 1;
                }

                var price = int.Parse(priceStr);
                item.Price = price;

                items.Add(item);
            }

            return items;
        }

        private ProductRecord GetCitilinkItem(HtmlNode node)
        {
            var item = new ProductRecord();

            var infotd = node.Descendants("td").First(x => x.Class() == "l");
            item.Name = infotd.ChildNodes.First(x => x.Name == "a").InnerText;
            item.Description = infotd.ChildNodes.First(x => x.Class() == "descr").InnerText;

            var priceNode = node.Descendants("div").FirstOrDefault(x => x.Class().Contains("club_price"));
            if (priceNode != null)
            {
                var priceString = Regex.Replace(priceNode.InnerText, "[^0-9]", "");
                item.Price = int.Parse(priceString);
            }

            return item;
        }

        protected override string GetUrl(string productType)
        {
            string url = null;
            switch (productType.ToLower())
            {
                case ProductTypeName.Motherboard:
                    //url = "http://www.citilink.ru/catalog/computers_and_notebooks/parts/motherboards/?p={0}";
                    url = "http://www.citilink.ru/catalog/computers_and_notebooks/parts/motherboards/?available=1&status=0&p={0}";
                    break;
                case ProductTypeName.PowerSupply:
                    url = "http://www.citilink.ru/catalog/computers_and_notebooks/parts/powersupply/?p={0}";
                    break;
            }
            return url;
        }
    }
}
