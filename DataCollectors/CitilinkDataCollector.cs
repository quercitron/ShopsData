﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

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

        protected override List<Product> GetProducts(string url)
        {
            var result = new List<Product>();
            for (int pageNumber = 1; ; pageNumber++)
            {
                var pageUrl = string.Format(url, pageNumber);
                var products = ProcessCitilinkPage(pageUrl);
                if (products != null)
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

        private List<Product> ProcessCitilinkPage(string url)
        {
            //var uriString = string.Format("http://www.citilink.ru/catalog/computers_and_notebooks/parts/motherboards/?p={0}", pageNumber);
            Uri target = new Uri(url);

            var request = WebRequest.CreateHttp(target);

            string source;
            var response = request.GetResponse();

            if (response.ResponseUri.Equals("http://www.citilink.ru/catalog/"))
            {
                return null;
            }

            using (var streamReader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(1251)))
            {
                source = streamReader.ReadToEnd();
            }

            source = WebUtility.HtmlDecode(source);
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(source);

            var products =
                document.DocumentNode.Descendants("table").Where(x => x.Class() == "item")
                    .Select(GetCitilinkItem).ToList();

            return products;
        }

        private Product GetCitilinkItem(HtmlNode node)
        {
            var item = new Product();

            var infotd = node.Descendants("td").First(x => x.Class() == "l");
            item.Name = infotd.ChildNodes.First(x => x.Name == "a").InnerText;
            item.Description = infotd.ChildNodes.First(x => x.Class() == "descr").InnerText;

            var priceNode = node.Descendants("div").FirstOrDefault(x => x.Class().Contains("club_price"));
            if (priceNode != null)
            {
                var priceString = Regex.Replace(priceNode.InnerText, "[^0-9]", "");
                item.Price = double.Parse(priceString);
            }

            return item;
        }

        protected override string GetUrl(string productType)
        {
            string url = null;
            switch (productType.ToLower())
            {
                case "motherboard":
                    url = "http://www.citilink.ru/catalog/computers_and_notebooks/parts/motherboards/?p={0}";
                    break;
                case "powersupply":
                    url = "http://www.citilink.ru/catalog/computers_and_notebooks/parts/powersupply/?p={0}";
                    break;
            }
            return url;
        }
    }
}
