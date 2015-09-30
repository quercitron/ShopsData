using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using DataCollectorCore;
using DataCollectorCore.DataObjects;

using HtmlAgilityPack;
using Newtonsoft.Json;

namespace DataCollectors
{
    public class DnsDataCollector : DataCollectorBase
    {
        public override string ShopName
        {
            get { return "DNS"; }
        }

        protected override List<ProductRecord> GetProducts(string locationName, string url)
        {
            int page = 0;
            int offset = 0;

            var productRecords = new List<ProductRecord>();

            while (true)
            {
                var targetUri = string.Format(url, page, offset);

                //Uri target = new Uri("http://www.dns-shop.ru/catalog/3633/monitory/?length_1=0");
                Uri target = new Uri(targetUri);
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

                var response = JsonConvert.DeserializeObject<DnsResponse>(source);

                source = WebUtility.HtmlDecode(response.Response);
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(source);

                /*var table = document.DocumentNode.Descendants()
                    .FirstOrDefault(
                        x => x.Class().Contains("catalog_view_list"));*/
                var products = document.DocumentNode
                    .Descendants("div").Where(x => x.Class().EndsWith("ec-price-item"))
                    .Select(GetDnsItemNewSite)
                    .ToList();

                if (products.Count == 0)
                {
                    break;
                }

                productRecords.AddRange(products);

                offset = response.Offset;
            }

            return productRecords;
        }

        private ProductRecord GetDnsItemNewSite(HtmlNode htmlNode)
        {
            var item = new ProductRecord();

            //item.Name = htmlNode.Attributes["data-ec-item-title"].Value;
            var nameNode = htmlNode.DescendantFirstOrDefault("div", "item-name").Descendants("a").First();
            item.Name = nameNode.InnerText;

            item.Brand = htmlNode.Attributes["data-ec-item-brand"].Value;
            item.ExternalId = htmlNode.Attributes["data-ec-item-id"].Value;

            var descNode = htmlNode.DescendantFirstOrDefault("div", "item-desc").Child("a", "ec-price-item-link");
            item.Description = descNode.InnerHtml;

            var priceNode = htmlNode.DescendantFirstOrDefault("div", "price_g");
            var priceStr = Regex.Replace(priceNode.InnerText, @"[^\d]", "");
            item.Price = int.Parse(priceStr);

            var ratingNode = htmlNode.DescendantFirstOrDefault("div", "product-item-rating rating");
            if (ratingNode != null)
            {
                var ratingAttr = ratingNode.Attributes["data-rating"];
                if (ratingAttr != null)
                {
                    float rating;
                    if (float.TryParse(ratingAttr.Value, out rating))
                    {
                        item.Rating = rating;
                    }
                }
            }

            // todo: add rating

            return item;
        }

        protected override string GetUrl(string productType)
        {
            string url = null;
            switch (productType.ToLower())
            {
                case ProductTypeName.Monitor:
                    url = "http://www.dns-shop.ru/catalog/3633/monitory/ajax/?p={0}&offset={1}";
                    break;
                case ProductTypeName.Motherboard:
                    url = "http://www.dns-shop.ru/catalog/3660/materinskie-platy/ajax/?p={0}&offset={1}";
                    break;
                case ProductTypeName.PowerSupply:
                    url = "http://www.dns-shop.ru/catalog/3670/bloki-pitaniya/ajax/?p={0}&offset={1}";
                    break;
            }
            return url;
        }
    }

    internal class DnsResponse
    {
        [JsonProperty("total_count")]
        public int TotalCount { get; set; }

        [JsonProperty("result")]
        public int Result { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("is_end")]
        public bool IsEnd { get; set; }

        [JsonProperty("response")]
        public string Response { get; set; }
    }
}
