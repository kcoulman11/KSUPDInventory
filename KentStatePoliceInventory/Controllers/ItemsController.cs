using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KentStatePoliceInventory.Models;
using KentStatePoliceInventory.Classes;
using System.Data.SqlClient;
using System.Data;

namespace KentStatePoliceInventory.Controllers
{
    public class ItemsController : Controller
    {
        public ActionResult ItemsView()
        {
            AddRemoveItemsModel model = new AddRemoveItemsModel();
            return View (model);
        }

        public JsonResult AddNewItem(string ItemName, int ItemQuantity, int ItemReorder, string ItemDescription, string ItemLocation, string ItemType, string GunSerial = "")
        {
            MethodStatus status = new MethodStatus();
            int location = -1;
            List<Location> Locations = new List<Location>();
            Configuration config = new Configuration();
            SqlConnection conn1 = new SqlConnection(config.ConnectionString());
            try
            {
                conn1.Open();
                string query1 = "SELECT InventoryLocationID FROM InventoryLocation WHERE InventoryLocationDescription=@location";
                using (SqlCommand command = new SqlCommand(query1, conn1))
                {
                    
                    command.CommandText = "SELECT InventoryLocationID FROM InventoryLocation WHERE InventoryLocationDescription=@location";
                    command.Parameters.AddWithValue("@location", ItemLocation);
                    string str = Convert.ToString(command.ExecuteScalar());
                    location = Int32.Parse(str);
                    
                    Locations.Add(new Location(ItemLocation, location));
   
                }
   
            }
            catch(Exception ex)
            {
                status.success = false;
                status.Message = ex.Message;
            }
            foreach(var LOCATION in Locations)
            {
                if(LOCATION.LocationName == ItemLocation)
                {
                    location = LOCATION.LocationID;
                }
            }

            List<InventoryItem> InventoryItems = new List<InventoryItem>();
            SqlConnection conn = new SqlConnection(config.ConnectionString());
            try
            {
                conn.Open();
                if (ItemType == "Guns ")
                {
                    string sql = "INSERT INTO Inventory(IsOnKSUInventory,ItemName,InventoryDescription,InventoryQuantity,InventoryReorderQuantity,InventoryTypeID,InventoryLocationID,GunSerial) VALUES(@param1,@param2,@param3,@param4,@param5,@param6,@param7,@param8)";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add("@param1", SqlDbType.Int).Value = 1;
                    cmd.Parameters.Add("@param2", SqlDbType.VarChar, 50).Value = ItemName;
                    cmd.Parameters.Add("@param3", SqlDbType.VarChar, 50).Value = ItemDescription;
                    cmd.Parameters.Add("@param4", SqlDbType.Int).Value = ItemQuantity;
                    cmd.Parameters.Add("@param5", SqlDbType.Int).Value = ItemReorder;
                    cmd.Parameters.Add("@param6", SqlDbType.Int).Value = 3;
                    cmd.Parameters.Add("@param7", SqlDbType.Int).Value = location;
                    cmd.Parameters.Add("@param8", SqlDbType.VarChar, 50).Value = GunSerial;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    string sql = "INSERT INTO Inventory(IsOnKSUInventory,ItemName,InventoryDescription,InventoryQuantity,InventoryReorderQuantity,InventoryTypeID,InventoryLocationID) VALUES(@param1,@param2,@param3,@param4,@param5,@param6,@param7)";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add("@param1", SqlDbType.Int).Value = 1;
                    cmd.Parameters.Add("@param2", SqlDbType.VarChar, 50).Value = ItemName;
                    cmd.Parameters.Add("@param3", SqlDbType.VarChar, 50).Value = ItemDescription;
                    cmd.Parameters.Add("@param4", SqlDbType.Int).Value = ItemQuantity;
                    cmd.Parameters.Add("@param5", SqlDbType.Int).Value = ItemReorder;
                    cmd.Parameters.Add("@param6", SqlDbType.Int).Value = 1;
                    cmd.Parameters.Add("@param7", SqlDbType.Int).Value = location;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
              status.success = false;
            //status.Message = ex.InnerException.ToString();
            }

            return Json(status, JsonRequestBehavior.DenyGet);
        }

        public JsonResult RemoveItem(string ItemName)
        {
            MethodStatus status = new MethodStatus();
            Configuration config = new Configuration();
            SqlConnection conn = new SqlConnection(config.ConnectionString());
            try
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT InventoryID FROM Inventory Where ItemName = @itemname";
                    cmd.Parameters.AddWithValue("@itemname", ItemName);
                    string str = Convert.ToString(cmd.ExecuteScalar());
                    int locationid = Int32.Parse(str);
                    int hasinventory = 0;
                    cmd.CommandText = "SELECT COUNT (InventoryID) FROM Issued WHERE InventoryID = @param1";
                    cmd.Parameters.AddWithValue("@param1", locationid);
                    string str2 = Convert.ToString(cmd.ExecuteScalar());
                    hasinventory = Int32.Parse(str2);

                    if (hasinventory == 0)
                    {

                        cmd.CommandText = "DELETE FROM Inventory WHERE ItemName = @word";
                        cmd.Parameters.AddWithValue("@word", ItemName);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        status.success = false;
                        status.Message = "Cannot remove items that have been issued";
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
