using System.Web.Mvc;
using System.Web.Routing;

namespace KentStatePoliceInventory
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
	            "Analytics",
	            "Analytics",
	        new
	        {
		    controller = "Analytics",
		    action = "AnalyticsView"
	        });

			routes.MapRoute(
				"InventoryView",
				"InventoryView",
			new
			{
				controller = "InventoryView",
				action = "InventoryViewView"
			});

			routes.MapRoute(
				"ItemsView",
				"ItemsView",
			new
			{
				controller = "Items",
				action = "ItemsView"
			});

			routes.MapRoute(
				"UserView",
				"UserView",
			new
			{
				controller = "Users",
				action = "UserView"
			});

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
