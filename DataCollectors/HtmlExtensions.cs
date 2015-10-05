using System;
using System.Collections.Generic;
using System.Linq;

using HtmlAgilityPack;

namespace DataCollectors
{
    public static class HtmlExtensions
    {
        public static string Class(this HtmlNode htmlNode)
        {
            var classAttribute = htmlNode.Attributes["class"];
            return classAttribute != null ? classAttribute.Value : string.Empty;
        }

        public static IEnumerable<HtmlNode> Childs(this HtmlNode htmlNode, string name)
        {
            return htmlNode.ChildNodes.Where(node => node.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<HtmlNode> Childs(this HtmlNode htmlNode, string name, string className)
        {
            return htmlNode.Childs(name).Where(node => node.Class().ContainsIgnoreCase(className));
        }

        public static HtmlNode Child(this HtmlNode htmlNode, string name)
        {
            return htmlNode.Childs(name).FirstOrDefault();
        }

        public static HtmlNode Child(this HtmlNode htmlNode, string name, string className)
        {
            return htmlNode.Childs(name, className).FirstOrDefault();
        }

        public static IEnumerable<HtmlNode> Descendants(this HtmlNode htmlNode, string name, string className)
        {
            return htmlNode.Descendants(name).Where(x => x.Class().ContainsIgnoreCase(className));
        }

        public static HtmlNode Descendant(this HtmlNode htmlNode, string name)
        {
            return htmlNode.Descendants(name).FirstOrDefault();
        }

        public static HtmlNode Descendant(this HtmlNode htmlNode, string name, string className)
        {
            return htmlNode.Descendants(name, className).FirstOrDefault();
        }

        private static bool ContainsIgnoreCase(this string source, string value)
        {
            return source.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
