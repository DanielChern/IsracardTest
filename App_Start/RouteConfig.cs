using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Isracard
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Bookmarks", action = "Index" }
            );

            routes.MapRoute(
                name: "API",
                url: "api/{controller}/{action}",
                defaults: new { controller = "Bookmarks", action = "Index" }
            );

            routes.MapRoute(
                name: "NotFound",
                url: "{*url}",
                defaults: new { controller = "Error", action = "Handler" }
            );
        }
    }
}
