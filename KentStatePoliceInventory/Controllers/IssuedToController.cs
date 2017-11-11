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
    public class IssuedToController : Controller
    {
        // GET: IssuedTo
        public ActionResult Index()
        {
            IssuedToModel model = new IssuedToModel();
            return View(model);
        }

        [HttpPost]
        public JsonResult LoadNewInventoryData(string locationName)
        {
            IssuedStatus status = new IssuedStatus();
            status.success = true;
            List<Location> Locations = new List<Location>();
            List<InventoryItem> Items = new List<InventoryItem>();
            List<InventoryItem> FinalItems = new List<InventoryItem>();
            List<IssuedEntry> IssuedEntries = new List<IssuedEntry>();
            List<IssuedEntry> FinalEntries = new List<IssuedEntry>();
            Configuration config = new Configuration();
            SqlConnection conn = new SqlConnection(config.ConnectionString());
            try
            {
                conn.Open();
                string query = "SELECT * FROM IssuedTo"; // Get All Locations
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string LocationName = reader.GetString(reader.GetOrdinal("IssuedToDescription"));
                            int LocationID = (int)reader.GetInt64(reader.GetOrdinal("IssuedToID"));
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
                query = "SELECT * FROM Issued"; // Get all issued entries
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int issuedid = (int)reader.GetInt64(reader.GetOrdinal("IssuedID"));
                            int issuedquantity = reader.GetInt32(reader.GetOrdinal("IssuedQuantity"));
                            DateTime issueddate = DateTime.Today;
                            int itemid = (int)reader.GetInt64(reader.GetOrdinal("InventoryID"));
                            int issuedtoid = (int)reader.GetInt64(reader.GetOrdinal("IssuedToID"));
                            IssuedEntry entry = new IssuedEntry(issuedid, issuedquantity, issueddate, itemid, issuedtoid);
                            IssuedEntries.Add(entry);
                        }
                    }
                }
                foreach(var item in Items) // set the name of each item in issued entry
                {
                    foreach(var entry in IssuedEntries)
                    {
                        if(item.ItemId == entry.InventoryID)
                        {
                            entry.ItemName = item.ItemName;
                        }
                    }
                }

                foreach(var location in Locations) // build the final list of items issued to the selected location
                {
                    if (location.LocationName == locationName)
                    {
                        foreach(var entry in IssuedEntries)
                        {
                            if(entry.IssuedToID == location.LocationID)
                            {
                                FinalEntries.Add(entry);
                            }
                        }
                    }
                }
                status.items = FinalEntries;
            }
            catch (Exception ex)
            {
                status.success = false;
                status.Message = "Failed to load data";
            }
            return Json(status, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult UpdateIssuedQuantity(string itemName, int oldIssuedQuantity, int numberTaken, string location)
        {
            MethodStatus status = new MethodStatus();
            Configuration config = new Configuration();
            SqlConnection conn = new SqlConnection(config.ConnectionString());
            int ItemQuantity = 0;
            int oldQuantity = 0;
            int itemId = -1;
            int issuedToID = -1;
            //get old quantity
            try
            {
                conn.Open();
                string query = "SELECT * FROM Inventory WHERE ItemName=[" + itemName + "]"; // Get All Locations
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandText = "SELECT * FROM Inventory WHERE ItemName=@itemName";
                    command.Parameters.AddWithValue("@itemName", itemName);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            oldQuantity = reader.GetInt32(reader.GetOrdinal("InventoryQuantity"));
                            itemId = (int)reader.GetInt64(reader.GetOrdinal("InventoryID"));
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                status.success = false;
                status.Message = "FAIL";
                return Json(status, JsonRequestBehavior.DenyGet);
            }

            ItemQuantity = oldQuantity - numberTaken; // check if update will take inventory under 0
            if(ItemQuantity < 0)
            {
                status.success = false;
                status.Message = "Cannot take more than currently in inventory";
                return Json(status, JsonRequestBehavior.DenyGet);
            }

            //set inventory table quantity to new quantity
            try
            {
                string query = "UPDATE Inventory SET InventoryQuantity=" + ItemQuantity + " WHERE ItemName=" + itemName;
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandText = "UPDATE Inventory SET InventoryQuantity = @quantity WHERE ItemName = @name";
                    command.Parameters.AddWithValue("@quantity", ItemQuantity);
                    command.Parameters.AddWithValue("@name", itemName);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                status.success = false;
                status.Message = ex.InnerException.ToString();
            }
            ItemQuantity = oldIssuedQuantity + numberTaken;
            //increment issued table quantity by numberTaken
            try // get IssuedToID
            {
                string query = "SELECT IssuedToID FROM IssuedTo WHERE IssuedToDescription=" + location;
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandText = "SELECT IssuedToID FROM IssuedTo WHERE IssuedToDescription=@location";
                    command.Parameters.AddWithValue("@location", location);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            issuedToID = (int)reader.GetInt64(reader.GetOrdinal("IssuedToID"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                status.success = false;
                status.Message = "FAIL";
                return Json(status, JsonRequestBehavior.DenyGet);
            }

            
            try // set new inventory quantity
            {
                string query = "UPDATE Issued SET IssuedQuantity=" + ItemQuantity + " WHERE InventoryID=" + itemId + " AND IssuedToID=" + issuedToID;
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandText = "UPDATE Issued SET IssuedQuantity = @quantity WHERE InventoryID = @item AND IssuedToID= @location";
                    command.Parameters.AddWithValue("@quantity", ItemQuantity);
                    command.Parameters.AddWithValue("@item", itemId);
                    command.Parameters.AddWithValue("@location", issuedToID);
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
        public JsonResult ReturnItem(string itemName, int numberToReturn, int oldIssuedQuantity, string location)
        {
            //decrement issued table quanttity by numberToReturn
            //increment iventory table quantity by numberToReturn
            MethodStatus status = new MethodStatus();
            Configuration config = new Configuration();
            SqlConnection conn = new SqlConnection(config.ConnectionString());
            int ItemQuantity = 0;
            int oldQuantity = 0;
            int itemId = -1;
            int issuedToID = -1;
            //get old quantity
            try
            {
                conn.Open();
                string query = "SELECT * FROM Inventory WHERE ItemName=[" + itemName + "]"; // Get All Locations
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandText = "SELECT * FROM Inventory WHERE ItemName=@itemName";
                    command.Parameters.AddWithValue("@itemName", itemName);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            oldQuantity = reader.GetInt32(reader.GetOrdinal("InventoryQuantity"));
                            itemId = (int)reader.GetInt64(reader.GetOrdinal("InventoryID"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                status.success = false;
                status.Message = "FAIL";
                return Json(status, JsonRequestBehavior.DenyGet);
            }
            ItemQuantity = oldQuantity + numberToReturn;
            //set inventory table quantity to new quantity
            try
            {
                string query = "UPDATE Inventory SET InventoryQuantity=" + ItemQuantity + " WHERE ItemName=" + itemName;
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandText = "UPDATE Inventory SET InventoryQuantity = @quantity WHERE ItemName = @name";
                    command.Parameters.AddWithValue("@quantity", ItemQuantity);
                    command.Parameters.AddWithValue("@name", itemName);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                status.success = false;
                status.Message = ex.InnerException.ToString();
            }
            ItemQuantity = oldIssuedQuantity - numberToReturn;
            try // get IssuedToID
            {
                string query = "SELECT IssuedToID FROM IssuedTo WHERE IssuedToDescription=" + location;
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandText = "SELECT IssuedToID FROM IssuedTo WHERE IssuedToDescription=@location";
                    command.Parameters.AddWithValue("@location", location);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            issuedToID = (int)reader.GetInt64(reader.GetOrdinal("IssuedToID"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                status.success = false;
                status.Message = "FAIL";
                return Json(status, JsonRequestBehavior.DenyGet);
            }


            try // set new inventory quantity
            {
                string query = "UPDATE Issued SET IssuedQuantity=" + ItemQuantity + " WHERE InventoryID=" + itemId + " AND IssuedToID=" + issuedToID;
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandText = "UPDATE Issued SET IssuedQuantity = @quantity WHERE InventoryID = @item AND IssuedToID= @location";
                    command.Parameters.AddWithValue("@quantity", ItemQuantity);
                    command.Parameters.AddWithValue("@item", itemId);
                    command.Parameters.AddWithValue("@location", issuedToID);
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
        public JsonResult UsedItem(string itemName, int numberToReturn, int oldIssuedQuantity, string location)
        {
            MethodStatus status = new MethodStatus();
            Configuration config = new Configuration();
            SqlConnection conn = new SqlConnection(config.ConnectionString());
            int ItemQuantity = oldIssuedQuantity - numberToReturn;
            int oldQuantity = 0;
            int itemId = -1;
            int issuedToID = -1;
            //decrement issued table quantity by numberToReturn
            ItemQuantity = oldIssuedQuantity - numberToReturn;
            conn.Open();
            //get old quantity
            try
            {
                string query = "SELECT * FROM Inventory WHERE ItemName=[" + itemName + "]"; // Get All Locations
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandText = "SELECT * FROM Inventory WHERE ItemName=@itemName";
                    command.Parameters.AddWithValue("@itemName", itemName);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            oldQuantity = reader.GetInt32(reader.GetOrdinal("InventoryQuantity"));
                            itemId = (int)reader.GetInt64(reader.GetOrdinal("InventoryID"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                status.success = false;
                status.Message = "FAIL";
                return Json(status, JsonRequestBehavior.DenyGet);
            }
            try // get IssuedToID
            {
                string query = "SELECT IssuedToID FROM IssuedTo WHERE IssuedToDescription=" + location;
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandText = "SELECT IssuedToID FROM IssuedTo WHERE IssuedToDescription=@location";
                    command.Parameters.AddWithValue("@location", location);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            issuedToID = (int)reader.GetInt64(reader.GetOrdinal("IssuedToID"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                status.success = false;
                status.Message = "FAIL";
                return Json(status, JsonRequestBehavior.DenyGet);
            }


            try // set new inventory quantity
            {
                string query = "UPDATE Issued SET IssuedQuantity=" + ItemQuantity + " WHERE InventoryID=" + itemId + " AND IssuedToID=" + issuedToID;
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandText = "UPDATE Issued SET IssuedQuantity = @quantity WHERE InventoryID = @item AND IssuedToID= @location";
                    command.Parameters.AddWithValue("@quantity", ItemQuantity);
                    command.Parameters.AddWithValue("@item", itemId);
                    command.Parameters.AddWithValue("@location", issuedToID);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                status.success = false;
                status.Message = "failed yo";
            }
            return Json(status, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult DeissueItem(string itemName, int oldIssuedQuantity, string locationName)
        {
            MethodStatus status = new MethodStatus();
            // increment inventory table quantity by quantity of item to deissue
            Configuration config = new Configuration();
            SqlConnection conn = new SqlConnection(config.ConnectionString());
            int ItemQuantity = 0;
            int oldQuantity = 0;
            int itemId = -1;
            int issuedToID = -1;
            //decrement issued table quantity by numberToReturn
            conn.Open();
            //get old quantity
            try
            {
                string query = "SELECT * FROM Inventory WHERE ItemName=[" + itemName + "]"; // Get All Locations
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandText = "SELECT * FROM Inventory WHERE ItemName=@itemName";
                    command.Parameters.AddWithValue("@itemName", itemName);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            oldQuantity = reader.GetInt32(reader.GetOrdinal("InventoryQuantity"));
                            itemId = (int)reader.GetInt64(reader.GetOrdinal("InventoryID"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                status.success = false;
                status.Message = "FAIL";
                return Json(status, JsonRequestBehavior.DenyGet);
            }
            ItemQuantity = oldQuantity + oldIssuedQuantity;
            //set inventory table quantity to new quantity
            try
            {
                string query = "UPDATE Inventory SET InventoryQuantity=" + ItemQuantity + " WHERE ItemName=" + itemName;
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandText = "UPDATE Inventory SET InventoryQuantity = @quantity WHERE ItemName = @name";
                    command.Parameters.AddWithValue("@quantity", ItemQuantity);
                    command.Parameters.AddWithValue("@name", itemName);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                status.success = false;
                status.Message = ex.InnerException.ToString();
            }

            // remove row from issued table
            try
            {
                int IssuedToID = 0;
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT IssuedToID FROM IssuedTo WHERE IssuedToDescription = @description";
                    cmd.Parameters.AddWithValue("@description", locationName);
                    string str = Convert.ToString(cmd.ExecuteScalar());
                    IssuedToID = Int32.Parse(str);
                }
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Issued WHERE IssuedToId = @location AND InventoryID=@item";
                    cmd.Parameters.AddWithValue("@location", IssuedToID);
                    cmd.Parameters.AddWithValue("@item", itemId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                status.success = false;
                status.Message = "failed to remove row from issued table.";
            }
            return Json(status, JsonRequestBehavior.DenyGet);
        }
    }
}