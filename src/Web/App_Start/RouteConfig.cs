using System.Web.Mvc;
using System.Web.Routing;

namespace Kanban
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.Ignore("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
        }
    }
}
