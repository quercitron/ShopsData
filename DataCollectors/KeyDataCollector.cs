using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using DataCollectorCore;
using DataCollectorCore.DataObjects;

using HtmlAgilityPack;

namespace DataCollectors
{
    public class KeyDataCollector : DataCollectorBase
    {
        private static readonly string BaseUrl = "http://key.ru";

        public override string ShopName
        {
            get { return "Key"; }
        }

        protected override List<ProductRecord> GetProducts(string locationName, string url)
        {
            int page = 0;

            var productRecords = new List<ProductRecord>();

            while (true)
            {
                var targetUri = string.Format(url, page);

                Uri target = new Uri(targetUri);

                // todo: all location
                var request = WebRequest.CreateHttp(target);
                request.Accept = @"text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                // todo: how does Accept-Encoding affect response encoding?
                //request.Headers.Add(@"Accept-Encoding", @"gzip, deflate");
                request.Headers.Add(@"Accept-Language", @"ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3");
                request.UserAgent = @"Mozilla/5.0 (Windows NT 6.1; WOW64; rv:41.0) Gecko/20100101 Firefox/41.0";

                // cities
                // vrn -- city_id=1065
                // spb -- city_id=1063
                string locationCookie = null;
                switch (locationName)
                {
                    case "vrn":
                        locationCookie = "1065";
                        break;
                }
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(new Cookie("mode_view", "list") { Domain = target.Host });
                //request.CookieContainer.Add(new Cookie("lt-tl", "9poq") { Domain = target.Host });
                if (locationCookie != null)
                {
                    request.CookieContainer.Add(new Cookie("city_id", locationCookie) { Domain = target.Host });
                }

                var response = new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();

                //return new List<ProductRecord>();

                var source = WebUtility.HtmlDecode(response);
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(source);

                var products = document.DocumentNode
                    .Childs("div", "item")
                    .Select(GetKeyItem)
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

        private ProductRecord GetKeyItem(HtmlNode htmlNode)
        {
            var record = new ProductRecord();

            record.ExternalId = htmlNode.Attributes["data-item_id"].Value;

            var photoNode = htmlNode.Descendant("a", "item_photo");
            record.SourceLink = BaseUrl + photoNode.Attributes["href"].Value;

            var imageNode = photoNode.Descendant("img");
            record.Image = BaseUrl + imageNode.Attributes["src"].Value;

            var nameNode = htmlNode.Descendant("div", "cell_name").Descendant("span");
            record.Name = nameNode.InnerText;

            var descriptionNode = htmlNode.Descendant("div", "cell_anons");
            record.Description = descriptionNode.InnerText;

            var buyNode = htmlNode.Descendant("div", "cell_buy").Descendant("button");
            if (buyNode == null) return null;
            var priceAttribute = buyNode.Attributes["data-price"];
            if (priceAttribute == null) return null;
            record.Price = (int) double.Parse(priceAttribute.Value);

            return record;
        }

        protected override GetUrlResult GetUrl(string productType)
        {
            string baseUrl = "http://key.ru/catalog/ajaxLoadingGoods_v2/?p={0}&category_id={categoryId}&params=%3F&is_ajax=1";
            string categoryId = null;
            string resultUrl = null;
            switch (productType.ToLower())
            {
                case ProductTypeName.Monitor:
                    categoryId = "4099";
                    break;
                case ProductTypeName.Motherboard:
                    categoryId = "9023";
                    break;
                case ProductTypeName.PowerSupply:
                    categoryId = "9679";
                    break;
                case ProductTypeName.Screwdriver:
                    // key don't sell screwdrivers
                    return new GetUrlResult { NotSell = true };
                case ProductTypeName.SSD:
                    resultUrl = "http://key.ru/catalog/ajaxLoadingGoods_v2/" +
                                "?p={0}&category_id=93410&params=%3Ff%255Bname%255D%3Dssd%26search_string%3Dssd%26&is_ajax=1";
                    break;
                case ProductTypeName.Headset:
                    resultUrl = "http://key.ru/catalog/ajaxLoadingGoods_v2/" +
                                "?p={0}&category_id=15854&params=%3Fcategory_id%3D15854%26collection%3D%26disc%3D%26ncitems" +
                                "%3D%26f%255Bname%255D%3D%26f%255Bprice%255D%255B0%255D%3D140%26f%255Bprice%255D%255B1%255D" +
                                "%3D32990%26f%255Bdlina_shnura%255D%255B0%255D%3D0.8%26f%255Bdlina_shnura%255D%255B1%255D" +
                                "%3D6%26f%255Bradius_dejstviya_besprovodnoj_garnituri%255D%255B0%255D%3D1.2%26f" +
                                "%255Bradius_dejstviya_besprovodnoj_garnituri%255D%255B1%255D%3D100%26f%255Bmaks_mownost" +
                                "%255D%255B0%255D%3D10%26f%255Bmaks_mownost%255D%255B1%255D%3D3500%26f%255Btip_mikrofona_garnituri" +
                                "%255D%255B0%255D%3D3847890%26f%255Btip_mikrofona_garnituri%255D%255B1%255D%3D6190552%26f" +
                                "%255Bvremya_raboty_v_rezhime_razgovora_garnitury%255D%255B0%255D%3D2%26f" +
                                "%255Bvremya_raboty_v_rezhime_razgovora_garnitury%255D%255B1%255D%3D250%26category_main_id" +
                                "%3D0%26maker_id%3D0%26model_id%3D0%26category_acs_id%3D0%26&is_ajax=1";
                    break;
                case ProductTypeName.CPU:
                    categoryId = "1117";
                    break;
                case ProductTypeName.Jigsaw:
                    // key don't sell jigsaws
                    return new GetUrlResult { NotSell = true };
                case ProductTypeName.Headphones:
                    resultUrl = "http://key.ru/catalog/ajaxLoadingGoods_v2/" +
                                "?p={0}&category_id=15854&params=%3Ff%255Bname%255D%3D%25D0%25BD%25D0%25B0%25D1%2583" +
                                "%25D1%2588%25D0%25BD%25D0%25B8%25D0%25BA%25D0%25B8%26search_string%3D%25D0%25BD" +
                                "%25D0%25B0%25D1%2583%25D1%2588%25D0%25BD%25D0%25B8%25D0%25BA%25D0%25B8%26&is_ajax=1";
                    break;
            }
            if (resultUrl != null)
            {
                return new GetUrlResult { Url = resultUrl };
            }
            if (categoryId != null)
            {
                var url = baseUrl.Replace("{categoryId}", categoryId);
                return new GetUrlResult { Url = url };
            }
            return new GetUrlResult();
        }
    }
}
