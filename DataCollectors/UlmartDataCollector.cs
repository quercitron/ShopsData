using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

using DataCollectorCore;
using DataCollectorCore.DataObjects;

using HtmlAgilityPack;

namespace DataCollectors
{
    public class UlmartDataCollector : DataCollectorBase
    {
        public override string ShopName
        {
            get { return "Ulmart"; }
        }

        protected override List<ProductRecord> GetProducts(string locationName, string url)
        {
            int page = 1;

            var productRecords = new List<ProductRecord>();

            while (true)
            {
                var targetUri = string.Format(url, page);

                Uri target = new Uri(targetUri);

                // todo: all location
                var request = WebRequest.CreateHttp(target);
                request.Accept = @"text/html, */*; q=0.01";
                // todo: how does Accept-Encoding affect response encoding?
                //request.Headers.Add(@"Accept-Encoding", @"gzip, deflate");
                request.Headers.Add(@"Accept-Language", @"ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3");
                request.UserAgent = @"Mozilla/5.0 (Windows NT 6.1; WOW64; rv:40.0) Gecko/20100101 Firefox/40.0";
                request.Headers.Add("X-Requested-With", "XMLHttpRequest");

                /*Cookie cookie = new Cookie("city_path", "voronezh") { Domain = target.Host };
                var cookieContainer = new CookieContainer();
                cookieContainer.Add(cookie);
                request.CookieContainer = cookieContainer;*/

                var response = new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();

                /*HttpClient http = new HttpClient();
                var response = http.GetByteArrayAsync("http://www.dns-shop.ru/catalog/3633/monitory/?length_1=0");
                var result = response.Result;*/

                //String source = Encoding.GetEncoding("utf-8").GetString(result, 0, result.Length - 1);

                //var response = JsonConvert.DeserializeObject<DnsResponse>(source);

                var source = WebUtility.HtmlDecode(response);
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(source);

                /*var table = document.DocumentNode.Descendants()
                    .FirstOrDefault(
                        x => x.Class().Contains("catalog_view_list"));*/
                var products = document.DocumentNode
                    .Descendants("section").Where(x => x.Class().StartsWith("b-product "))
                    .Select(GetUlmartItem)
                    .Where(pr => pr != null)
                    .ToList();

                if (products.Count == 0)
                {
                    break;
                }

                productRecords.AddRange(products);

                page++;
            }

            return productRecords;
        }

        private ProductRecord GetUlmartItem(HtmlNode htmlNode)
        {
            var record = new ProductRecord();

            var helperNode = htmlNode.Descendant("span", "js_gtm_helper");
            //record.Name = helperNode.Attributes["data-gtm-eventProductName"].Value;
            record.ExternalId = helperNode.Attributes["data-gtm-eventProductId"].Value;
            record.Brand = helperNode.Attributes["data-gtm-eventVendorName"].Value;
            record.Price = (int)decimal.Parse(helperNode.Attributes["data-gtm-eventProductPrice"].Value);

            var center = htmlNode.Descendant("div", "b-product__center");
            var titleBlock = center.Descendant("div", "b-product__title");
            var titleNode = titleBlock.Descendants("a").First();
            record.Name = titleNode.InnerText;

            var descriptionNode = center.Descendant("div", "b-product__descr");
            record.Description = descriptionNode.InnerText;

            var hrefNode = htmlNode.Descendants("a").First(x => x.Class().Contains("js-gtm-product-click"));
            record.SourceLink = "http://www.ulmart.ru" + hrefNode.Attributes["href"].Value;

            var ratingNode = center.Descendants("div").First(x => x.Class().StartsWith("b-small-stars "));
            var ratingClass = ratingNode.Class();
            for (int i = 0; i <= 5; i++)
            {
                var pattern = "_s" + i;
                if (ratingClass.Contains(pattern))
                {
                    record.Rating = i;
                }
            }

            var imageNode = htmlNode.Descendant("img", "b-img2__img");
            if (imageNode != null)
            {
                record.Image = imageNode.Attributes["src"].Value;
            }

            return record;
        }

        protected override GetUrlResult GetUrl(string productType)
        {
            string baseUrl = "http://www.ulmart.ru/catalogAdditional/{productType}?sort=5&viewType=1&destination=&extended=" +
                             "&filters=&numericFilters=&brands=&warranties=&shops=&labels=&available=&reserved=" +
                             "&suborder=&superPrice=&specOffers=&minPrice=&maxPrice=&query=&pageNum={0}";
            string productTypeName = null;
            switch (productType)
            {
                case ProductTypeName.Monitor:
                    productTypeName = "monitors";
                    break;

                case ProductTypeName.Motherboard:
                    productTypeName = "motherboards";
                    break;

                case ProductTypeName.PowerSupply:
                    productTypeName = "power_supply2";
                    break;

                case ProductTypeName.Screwdriver:
                    productTypeName = "sandingscrew";
                    break;

                case ProductTypeName.SSD:
                    productTypeName = "hdd_ssd";
                    break;

                case ProductTypeName.Headset:
                    productTypeName = "headset";
                    break;
            }

            if (productTypeName != null)
            {
                var url = baseUrl.Replace("{productType}", productTypeName);
                return new GetUrlResult { Url = url };
            }
            return new GetUrlResult();
        }
    }
}
