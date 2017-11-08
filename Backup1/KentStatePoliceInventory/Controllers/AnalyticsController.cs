using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KentStatePoliceInventory.Models;

namespace KentStatePoliceInventory.Controllers
{
    public class AnalyticsController : Controller
    {
        public ActionResult AnalyticsView()
        {
            InventoryViewModel model = new InventoryViewModel();
            return View(model);
        }
    }
}
