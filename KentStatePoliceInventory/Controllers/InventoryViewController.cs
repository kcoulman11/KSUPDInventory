using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KentStatePoliceInventory.Models;

namespace KentStatePoliceInventory.Controllers
{
    public class InventoryViewController : Controller
    {
        public ActionResult InventoryViewView()
        {
            InventoryViewModel model = new InventoryViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateQuantities(string valueINeed)
        {
            //do what is necessary with the valueINeed to save it back to db

            InventoryViewModel model = new InventoryViewModel();
            model.InventoryItems.First().ItemDescription = valueINeed;
            return View("/Views/InventoryView/InventoryViewView.cshtml", model);
        }       
    }
}
