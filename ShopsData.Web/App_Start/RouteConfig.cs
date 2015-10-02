using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShopsData.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "CurrentData",
                url: "CurrentData/{locationId}/{productTypeId}",
                defaults: new { controller = "Home", Action = "Index" }
            );

            routes.MapRoute(
                name: "ProductDetails",
                url: "ProductDetails/{locationId}/{productId}",
                defaults: new { controller = "Home", Action = "Index" }
            );

            routes.MapRoute(
                name: "CurrentDataPartial",
                url: "{controller}/CurrentData/{locationId}/{productTypeId}",
                defaults: new { controller = "Home", action = "CurrentData" }
            );

            routes.MapRoute(
                name: "ProductDetailsPartial",
                url: "{controller}/ProductDetails/{locationId}/{productId}",
                defaults: new { controller = "Home", action = "ProductDetails" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
