using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;

namespace ShopsData.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "CurrentDataApi",
                routeTemplate: "api/CurrentData/{locationId}/{productTypeId}",
                defaults: new { controller = "CurrentData" }
            );

            config.Routes.MapHttpRoute(
                name: "CurrentDataGroupedApi",
                routeTemplate: "api/CurrentDataGrouped/{locationId}/{productTypeId}",
                defaults: new { controller = "CurrentDataGrouped" }
            );

            config.Routes.MapHttpRoute(
                name: "ProductDetailsApi",
                routeTemplate: "api/ProductDetails/{locationId}/{productId}",
                defaults: new { controller = "ProductDetails" }
            );

            // todo: is it a good method to set JSON formatter as default?
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }
    }
}
