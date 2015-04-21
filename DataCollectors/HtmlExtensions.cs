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
    }
}
