using System.Web.Mvc;
using System.Web.Routing;

namespace MusicSearch
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{author}",
                defaults: new { controller = "Home", action = "Index", author = "" }
            );

            routes.MapRoute(
                name:"test",
                url: "{controller}/Albums/{author}/{action}/{album}",
                defaults: new { controller = "Home", action = "Index", author = "", album = "" }
                );

           
        }
    }
}
