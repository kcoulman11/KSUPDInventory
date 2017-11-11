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
            Configuration config = new Configuration();
            SqlConnection conn = new SqlConnection(config.ConnectionString());
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
                status.Message = ex.Message;
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
            Configuration config = new Configuration();
            SqlConnection conn = new SqlConnection(config.ConnectionString());
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
                          int LocationID = (int)reader.GetInt64(reader.GetOrdinal("InventoryLocationID"));
                          Locations.Add(new Location(LocationName, LocationID));
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

        public JsonResult IssueItem(string issuedTo, int quantity, string itemName)
        {
            MethodStatus status = new MethodStatus();
            Configuration config = new Configuration();
            int QuantityUpdate = quantity;
            SqlConnection conn = new SqlConnection(config.ConnectionString());
            try
            {
                conn.Open();
                int inventoryId = -1;
                int issuedToId = -1;
                bool IssuedAlreadyExists = false;
                string query = "SELECT * FROM Inventory"; // get all items
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string ItemName = reader.GetString(reader.GetOrdinal("ItemName"));
                            int ItemID = (int)reader.GetInt64(reader.GetOrdinal("InventoryID"));
                            if(ItemName == itemName)
                            {
                                inventoryId = ItemID; // get item id for issued to
                            }
                        }
                    }
                }
                query = "SELECT * FROM IssuedTo"; // get all issuedtolocations
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string description = reader.GetString(reader.GetOrdinal("IssuedToDescription"));
                            int locationId = (int)reader.GetInt64(reader.GetOrdinal("IssuedToID"));
                            if (description == issuedTo)
                            {
                                issuedToId = locationId; // get location id for issued to
                            }
                        }
                    }
                }
                query = "SELECT * FROM Issued"; // check if row already exists that matches itemID & issuedToID
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int issuedId = (int)reader.GetInt64(reader.GetOrdinal("IssuedToID"));
                            int itemID = (int)reader.GetInt64(reader.GetOrdinal("InventoryID"));
                            if (itemID == inventoryId && issuedId == issuedToId)
                            {
                                IssuedAlreadyExists = true; // record with this item/location already exists
                            }
                        }
                    }
                }
                if (!IssuedAlreadyExists) // location/item combination do not exist in the DB, so add a new row with them
                {
                    string insertString = "INSERT INTO Issued (IssuedQuantity,IssuedDate,InventoryID,IssuedToID) VALUES (@val1, @val2, @val3, @val4)";
                    using (SqlCommand comm = new SqlCommand(insertString, conn))
                    {
                        comm.CommandText = insertString;
                        comm.Parameters.AddWithValue("@val1", quantity);
                        comm.Parameters.AddWithValue("@val2", DateTime.Now.ToShortDateString());
                        comm.Parameters.AddWithValue("@val3", inventoryId);
                        comm.Parameters.AddWithValue("@val4", issuedToId);
                        comm.ExecuteNonQuery();
                    }
                }
                else // find current quantity, add new issued quantity to it, update table accordingly
                {
                    int oldQuantity = -1;
                    int newQuantity = -1;
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Issued WHERE InventoryID =" + inventoryId + "AND IssuedToID =" + issuedToId, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int ItemID = (int)reader.GetInt64(reader.GetOrdinal("InventoryID"));
                                int locationId = (int)reader.GetInt64(reader.GetOrdinal("IssuedToID"));
                                if(ItemID == inventoryId && locationId == issuedToId)
                                {
                                    oldQuantity = (int)reader.GetInt32(reader.GetOrdinal("IssuedQuantity"));
                                    newQuantity = oldQuantity + quantity;
                                }
                            }
                        }
                    }
                    using (SqlCommand cmd = new SqlCommand("UPDATE Issued SET IssuedQuantity=@quantity" + " WHERE InventoryID=@ItemId AND IssuedToID=@LocationID", conn))
                    {
                        cmd.Parameters.AddWithValue("@quantity", newQuantity);
                        cmd.Parameters.AddWithValue("@ItemId", inventoryId);
                        cmd.Parameters.AddWithValue("@LocationID", issuedToId);

                        int rows = cmd.ExecuteNonQuery();
                    }
                }
                int originalQuantity = -1;
                int updatedQuantity = -1;
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Inventory WHERE InventoryID =" + inventoryId, conn)) // Get Old quantity and calculate new quantity
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int ItemID = (int)reader.GetInt64(reader.GetOrdinal("InventoryID"));
                            if (ItemID == inventoryId)
                            {
                                originalQuantity = (int)reader.GetInt32(reader.GetOrdinal("InventoryQuantity"));
                                updatedQuantity = originalQuantity - quantity;
                            }
                        }
                    }
                }
                using (SqlCommand cmd = new SqlCommand("UPDATE Inventory SET InventoryQuantity=@quantity" + " WHERE InventoryID=@ItemId", conn))
                {
                    cmd.Parameters.AddWithValue("@ItemId", inventoryId);
                    cmd.Parameters.AddWithValue("@quantity", updatedQuantity);
                    int rows = cmd.ExecuteNonQuery();
                }

            }
            catch(Exception ex)
            {
                status.Message = ex.Message; 
            }
            conn.Close();
            return Json(status, JsonRequestBehavior.DenyGet);
        }
    }
}
