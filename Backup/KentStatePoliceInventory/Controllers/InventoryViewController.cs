using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using KentStatePoliceInventory.Models;
using KentStatePoliceInventory.Classes;

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
        public JsonResult UpdateQuantities(string ItemName, int ItemQuantity)
        {
            MethodStatus status = new MethodStatus();

            SqlConnection conn = new SqlConnection("SERVER=173.88.245.82,1433;Database=Inventory;USER ID=Capstone;PASSWORD=abc123");
            try
            {
                conn.Open();
                string query = "UPDATE Inventory SET InventoryQuantity=" + ItemQuantity + " WHERE ItemName=" + ItemName;
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandText = "UPDATE Inventory SET InventoryQuantity = @quantity WHERE ItemName = @name";
                    command.Parameters.AddWithValue("@quantity", ItemQuantity);
                    command.Parameters.AddWithValue("@name", ItemName);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                status.success = false;
                status.Message = ex.InnerException.ToString();
            }

            return Json(status, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult LoadLocationData(string locationName)
        {
            MethodStatus status = new MethodStatus();

            status.success = true;

            return Json(status, JsonRequestBehavior.DenyGet);
        }
    }
}
