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

        public static HtmlNode Child(this HtmlNode htmlNode, string name, string className)
        {
            return htmlNode.ChildNodes
                .FirstOrDefault(n => n.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) &&
                                     n.Class().Equals(className, StringComparison.InvariantCultureIgnoreCase));
        }

        public static IEnumerable<HtmlNode> Descendants(this HtmlNode htmlNode, string name, string className)
        {
            return htmlNode.Descendants(name).Where(x => x.Class().Equals(className, StringComparison.InvariantCultureIgnoreCase));
        }

        public static HtmlNode DescendantFirstOrDefault(this HtmlNode htmlNode, string name, string className)
        {
            return htmlNode.Descendants(name, className).FirstOrDefault();
        }
    }
}
