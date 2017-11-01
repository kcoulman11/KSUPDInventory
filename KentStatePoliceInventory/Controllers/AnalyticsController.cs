using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Windows.Forms;
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
        public ActionResult Button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("SERVER=173.88.245.82,1433;Database=Inventory;USER ID=Capstone;PASSWORD=abc123");
            try
            {
                conn.Open();

                return View("worked");

            }
            catch (Exception ex)
            {
                ViewData["exception"] = ex;
                return View("notWorked");
            }

        }
    }
}
