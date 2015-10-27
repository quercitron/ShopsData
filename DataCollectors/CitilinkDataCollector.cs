using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using DataCollectorCore;
using DataCollectorCore.DataObjects;

using HtmlAgilityPack;
using Newtonsoft.Json;

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
            int? itemsOnPageCount = null;
            for (int pageNumber = 1; ; pageNumber++)
            {
                var pageUrl = string.Format(url, pageNumber);
                var locationCookie = GetLocationCookie(locationName);
                var products = ProcessCitilinkPage(pageUrl, locationCookie);
                if (products != null && products.Count != 0)
                {
                    result.AddRange(products);
                    if (itemsOnPageCount == null)
                    {
                        itemsOnPageCount = products.Count;
                    }
                    else if (products.Count < itemsOnPageCount)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        private string GetLocationCookie(string locationName)
        {
            string result;
            switch (locationName.ToLower())
            {
                case "vrn":
                    result = "vrzh_cl%3A";
                    break;
                default:
                    var message = string.Format("Could not find location cookie for location '{0}'.", locationName);
                    throw new NotSupportedException(message);
            }
            return result;
        }

        private List<ProductRecord> ProcessCitilinkPage(string url, string locationCookie)
        {
            //var uriString = string.Format("http://www.citilink.ru/catalog/computers_and_notebooks/parts/motherboards/?p={0}", pageNumber);
            Uri target = new Uri(url);

            /*HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                response = client.PostAsync(target, null).Result;
            }*/

            var request = WebRequest.CreateHttp(target);

            request.Accept = @"text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.Headers.Add(@"Accept-Encoding", @"gzip, deflate");
            request.Headers.Add(@"Accept-Language", @"ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3");
            request.UserAgent = @"Mozilla/5.0 (Windows NT 6.1; WOW64; rv:40.0) Gecko/20100101 Firefox/40.0";

            // we shouldn't set location cookie for default location
            if (locationCookie != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(new Cookie("_space", locationCookie) { Domain = target.Host });
            }

            var response = request.GetResponse();


            /*if (response.ResponseUri.Equals("http://www.citilink.ru/catalog/"))
            {
                return null;
            }*/
            var responseStream = response.GetResponseStream();
            string source;
            using (var streamReader = new StreamReader(responseStream, Encoding.UTF8))
            {
                source = streamReader.ReadToEnd();
            }
            //var source = Encoding.GetEncoding(1251).GetString(responseBytes, 0, responseBytes.Length - 1); ;

            // todo: workaround, how to improve?
            source = source.Replace("&quot;", "'");
            source = WebUtility.HtmlDecode(source);
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(source);

            return GetItems(document);
        }

        private List<ProductRecord> GetItems(HtmlDocument document)
        {
            var productNodes =
                   document.DocumentNode.Descendants("div").First(x => x.Class() == "product_category_list")
                       .Descendants("tbody")
                       .Where(x => string.IsNullOrEmpty(x.Descendant("tr").Class()))
                       .ToArray();

            var items = new List<ProductRecord>();
            foreach (var productNode in productNodes)
            {
                var record = GetCitilinkItem(productNode);
                if (record != null)
                {
                    items.Add(record);
                }
                else
                {
                    break;
                }
            }

            return items;
        }

        private ProductRecord GetCitilinkItem(HtmlNode htmlNode)
        {
            var item = new ProductRecord();

            var rows = htmlNode.Childs("tr").ToArray();

            var infoNode = rows[0];

            var productNameTd = infoNode.Child("td", "product_name");

            var titleNode = productNameTd.Descendants("a").First(x => x.Class() == "link_gtm-js");
            item.Name = titleNode.Attributes["title"].Value;
            item.SourceLink = "http://www.citilink.ru" + titleNode.Attributes["href"].Value;

            var descriptionNode = productNameTd.Descendants("p").First(x => x.Class() == "short_description");
            item.Description = descriptionNode.InnerText;

            var imageTd = infoNode.Descendant("td", "image");
            if (imageTd != null)
            {
                var imageNode = imageTd.Descendant("img");
                if (imageNode != null)
                {
                    if (imageNode.Attributes["src"] != null)
                    {
                        item.Image = imageNode.Attributes["src"].Value;
                    }
                    else if (imageNode.Attributes["data-src"] != null)
                    {
                        item.Image = imageNode.Attributes["data-src"].Value;
                    }
                }
            }

            var ratingDiv = infoNode.Descendant("div", "ratings");
            if (ratingDiv != null)
            {
                var spans = ratingDiv.Descendants("span").ToArray();
                if (spans.Length >= 5)
                {
                    float rating = 0;
                    for (int j = 0; j < 4; j++)
                    {
                        var span = spans[j];
                        if (span.Class().Contains("half"))
                        {
                            rating = 4.5f - j;
                            break;
                        }
                        if (span.Class().Contains("selected"))
                        {
                            rating = 5 - j;
                        }
                    }
                    item.Rating = rating;
                }
            }

            var priceRow = rows[1];

            var priceNode = priceRow.Descendants("span").FirstOrDefault(x => x.Class() == "special");
            if (priceNode == null)
            {
                priceNode = priceRow.Descendants("span").FirstOrDefault(x => x.Class() == "standart");
                if (priceNode == null)
                {
                    return null;
                }
            }

            var priceNumNode = priceNode.Descendants("ins").First(x => x.Class() == "num");

            var priceStr = Regex.Replace(priceNumNode.InnerText, "[^0-9]", "");
            var price = int.Parse(priceStr);
            item.Price = price;

            //var dataNode = priceRow.Descendant("td", "product_data__gtm-js");
            var dataNode = htmlNode;
            var dataParamsStr = dataNode.Attributes["data-params"].Value;
            var dataParams = JsonConvert.DeserializeObject<DataParams>(dataParamsStr);
            item.ExternalId = dataParams.Id;
            item.Brand = dataParams.BrandName;

            return item;
        }

        private class DataParams
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("shortName")]
            public string ShortName { get; set; }

            [JsonProperty("brandName")]
            public string BrandName { get; set; }

            [JsonProperty("price")]
            public string Price { get; set; }

            [JsonProperty("categoryId")]
            public string CategoryId { get; set; }

            [JsonProperty("categoryName")]
            public string CategoryName { get; set; }
        }

        protected override GetUrlResult GetUrl(string productType)
        {
            string url = null;
            switch (productType.ToLower())
            {
                case ProductTypeName.Motherboard:
                    url = "http://www.citilink.ru/catalog/computers_and_notebooks/parts/motherboards/?available=1&status=0&p={0}";
                    break;
                case ProductTypeName.PowerSupply:
                    url = "http://www.citilink.ru/catalog/computers_and_notebooks/parts/powersupply/?available=1&status=0&p={0}";
                    break;
                case ProductTypeName.Monitor:
                    url = "http://www.citilink.ru/catalog/computers_and_notebooks/monitors/?available=1&status=0&p={0}";
                    break;
                case ProductTypeName.Screwdriver:
                    url = "http://www.citilink.ru/catalog/power_tools_and_garden_equipments/screwdrivers/?available=1&status=0&p={0}";
                    break;
                case ProductTypeName.SSD:
                    url = "http://www.citilink.ru/catalog/computers_and_notebooks/hdd/ssd_in/?available=1&status=0&p={0}";
                    break;
                case ProductTypeName.Headset:
                    url = "http://www.citilink.ru/catalog/computers_and_notebooks/pc_headset/?available=1&status=0&p={0}";
                    break;
                case ProductTypeName.CPU:
                    url = "http://www.citilink.ru/catalog/computers_and_notebooks/parts/cpu/?available=1&status=0&p={0}";
                    break;
                case ProductTypeName.Jigsaw:
                    url = "http://www.citilink.ru/catalog/power_tools_and_garden_equipments/jig_saws/?available=1&status=0&p={0}";
                    break;
                case ProductTypeName.Headphones:
                    url = "http://www.citilink.ru/catalog/mobile/handsfree/?available=1&status=0&p={0}";
                    break;
            }
            return new GetUrlResult { Url = url };
        }
    }
}
