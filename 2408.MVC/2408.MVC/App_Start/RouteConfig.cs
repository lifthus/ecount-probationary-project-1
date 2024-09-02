using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace _2408.MVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            /* View Controllers */
            routes.MapRoute(
                name: "IndexView",
                url: "",
                defaults: new { controller = "Index", action = "Index" }
             );
            routes.MapRoute(
                name: "ProductView",
                url: "product/{action}",
                defaults: new { controller = "Product", action = "Index" }
             );
            routes.MapRoute(
                name: "SaleView",
                url: "sale/{action}",
                defaults: new { controller = "Sale", action = "Index" }
             );

            /* API Controllers */
            routes.MapRoute(
                name: "ProductAPI",
                url: "api/product/{action}",
                defaults: new { controller = "ProductAPI" }
            );
            routes.MapRoute(
                name: "SaleAPI",
                url: "api/sale/{action}",
                defaults: new { controller = "SaleAPI" }
            );
        }
    }
}
