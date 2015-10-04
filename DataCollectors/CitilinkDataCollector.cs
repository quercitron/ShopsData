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
            for (int pageNumber = 1; ; pageNumber++)
            {
                var pageUrl = string.Format(url, pageNumber);
                var locationCookie = GetLocationCookie(locationName);
                var products = ProcessCitilinkPage(pageUrl, locationCookie);
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
                       .Where(x => x.Class() == "")
                       .ToArray();

            for (int i = 0; i < productRows.Length; i++)
            {
                var item = new ProductRecord();

                var row = productRows[i];

                var productNameTd = row.Child("td", "product_name");

                var titleNode = productNameTd.Descendants("a").First(x => x.Class() == "product_link__js");
                item.Name = titleNode.Attributes["title"].Value;
                item.SourceLink = "http://www.citilink.ru" + titleNode.Attributes["href"].Value;

                var descriptionNode = productNameTd.Descendants("p").First(x => x.Class() == "short_description");
                item.Description = descriptionNode.InnerText;

                var ratingDiv = row.DescendantFirstOrDefault("div", "ratings");
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

                var priceStr = Regex.Replace(priceNumNode.InnerText, "[^0-9]", "");
                var price = int.Parse(priceStr);
                item.Price = price;

                var dataParamsTd = row.DescendantFirstOrDefault("td", "actions product_data__js");
                var dataParamsStr = dataParamsTd.Attributes["data-params"].Value;
                var dataParams = JsonConvert.DeserializeObject<DataParams>(dataParamsStr);
                item.ExternalId = dataParams.Id;
                item.Brand = dataParams.BrandName;

                items.Add(item);
            }

            return items;
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

        protected override string GetUrl(string productType)
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
            }
            return url;
        }
    }
}
