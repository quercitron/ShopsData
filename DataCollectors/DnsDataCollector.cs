using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

using DataCollectorCore;

using HtmlAgilityPack;

namespace DataCollectors
{
    public class DnsDataCollector : DataCollectorBase, IShopDataCollector
    {
        public override string ShopName
        {
            get { return "DNS"; }
        }

        protected override List<Product> GetProducts(string url)
        {
            //Uri target = new Uri("http://www.dns-shop.ru/catalog/3633/monitory/?length_1=0");
            Uri target = new Uri(url);
            var request = WebRequest.CreateHttp(target);
            /*Cookie cookie = new Cookie("city_path", "voronezh") { Domain = target.Host };
            var cookieContainer = new CookieContainer();
            cookieContainer.Add(cookie);
            request.CookieContainer = cookieContainer;*/

            var source = new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();

            /*HttpClient http = new HttpClient();
            var response = http.GetByteArrayAsync("http://www.dns-shop.ru/catalog/3633/monitory/?length_1=0");
            var result = response.Result;*/

            //String source = Encoding.GetEncoding("utf-8").GetString(result, 0, result.Length - 1);
            source = WebUtility.HtmlDecode(source);
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(source);

            var table = document.DocumentNode.Descendants()
                .FirstOrDefault(
                    x => x.Class().Contains("catalog_view_list"));
            var products = table.Descendants()
                .First(x => x.Name == "tbody").Descendants()
                .Where(
                    x => x.Name == "tr" && x.Attributes["data-price_item_id"] != null)
                .Select(GetDnsItem)
                .ToList();
            return products;
        }

        private Product GetDnsItem(HtmlNode htmlNode)
        {
            var item = new Product();

            var priceStr = htmlNode.Descendants().First(x => x.Class().Contains("price")).InnerText;
            item.Price = double.Parse(priceStr);

            var title = htmlNode.Descendants().First(x => x.Class() == "title");
            item.Name = title.Descendants().First(x => x.Name == "a" && x.Class() != "image").InnerText;
            item.Description = title.Descendants().First(x => x.Name == "p").InnerText;

            return item;
        }

        protected override string GetUrl(string productType)
        {
            string url = null;
            switch (productType.ToLower())
            {
                case "monitor":
                    url = "http://www.dns-shop.ru/catalog/3633/monitory/?length_1=0";
                    break;
                case "motherboard":
                    url = "http://www.dns-shop.ru/catalog/3660/materinskie-platy/?length_1=0";
                    break;
            }
            return url;
        }
    }
}
