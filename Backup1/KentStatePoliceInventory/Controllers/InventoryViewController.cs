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
        public JsonResult LoadNewInventoryData(string locationName)
        {
            InventoryStatus status = new InventoryStatus();
            status.success = true;
            List<Location> Locations = new List<Location>();
            List<InventoryItem> Items = new List<InventoryItem>();
            List<InventoryItem> FinalItems = new List<InventoryItem>();
            SqlConnection conn = new SqlConnection("SERVER=173.88.245.82,1433;Database=Inventory;USER ID=Capstone;PASSWORD=abc123");
            try
            {
              conn.Open();
              string query = "SELECT * FROM InventoryLocation"; // Get All Locations
              using (SqlCommand command = new SqlCommand(query, conn))
              {
                  using (SqlDataReader reader = command.ExecuteReader())
                  {
                      while (reader.Read())
                      {
                          string LocationName = reader.GetString(reader.GetOrdinal("InventoryLocationDescription"));
                          //int LocationID = reader.GetInt32(reader.GetOrdinal("InventoryLocationID"));
                          Locations.Add(new Location(LocationName));
                      }
                  }
              }
              query = "SELECT * FROM Inventory"; // Get all items
              using (SqlCommand command = new SqlCommand(query, conn))
              {
                  using (SqlDataReader reader = command.ExecuteReader())
                  {
                      while (reader.Read())
                      {
                        string ItemName = reader.GetString(reader.GetOrdinal("ItemName"));
                        int ItemQuantity = reader.GetInt32(reader.GetOrdinal("InventoryQuantity"));
                        int ItemReorder = reader.GetInt32(reader.GetOrdinal("InventoryReorderQuantity"));
                        string ItemDescription = reader.GetString(reader.GetOrdinal("InventoryDescription"));
                        int InventoryTypeID = (int)reader.GetInt64(reader.GetOrdinal("InventoryTypeID"));
                        int InventoryLocationID = (int)reader.GetInt64(reader.GetOrdinal("InventoryLocationID"));
                        int ItemID = (int)reader.GetInt64(reader.GetOrdinal("InventoryID"));
                        InventoryItem item = new InventoryItem(ItemName, ItemQuantity, ItemReorder, ItemDescription, InventoryTypeID, InventoryLocationID, ItemID);
                        Items.Add(item);
                      }
                  }
              }

              foreach(var item in Items)
              {
                foreach(var location in Locations)
                {
                  if(location.LocationName == locationName)
                  {
                    if(location.LocationID == item.LocationId)
                    {
                       status.items.Add(item);
                    }
                  }
                }
              }

            }
            catch(Exception ex)
            {
                status.success = false;
                status.Message = ex.Message;
            }
            
            return Json(status, JsonRequestBehavior.DenyGet);
        }
    }
}
