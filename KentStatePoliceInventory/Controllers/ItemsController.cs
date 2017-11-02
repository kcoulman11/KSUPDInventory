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

        public JsonResult AddNewItem(string ItemName, int ItemQuantity, int ItemReorder, string ItemDescription, string ItemLocation, string ItemType)
        {
            MethodStatus status = new MethodStatus();
            int location = -1;
            List<Location> Locations = new List<Location>();
            SqlConnection conn1 = new SqlConnection("SERVER=IPADDRESS,1433;Database=Inventory;USER ID=Capstone;PASSWORD=abc123");
            try
            {
                conn1.Open();
                string query1 = "SELECT * FROM InventoryLocation";
                using (SqlCommand command = new SqlCommand(query1, conn1))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("InventoryLocationID"));
                        string description = reader.GetString(reader.GetOrdinal("InventoryLocationDescription"));
                        Locations.Add(new Location(description, id)); 
                    }
                }
                /*
                query1 = "SELECT * FROM InventoryType";
                using (SqlCommand command = new SqlCommand(query1, conn1))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("InventoryLocationID"));
                        string description = reader.GetString(reader.GetOrdinal("InventoryLocationDescription"));
                        Locations.Add(new Location(description, id));
                    }
                }
                */
            }
            catch(Exception ex)
            {
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
            SqlConnection conn = new SqlConnection("SERVER=IPADDRESS,1433;Database=Inventory;USER ID=Capstone;PASSWORD=abc123");
            try
            {
                conn.Open();
                string sql = "INSERT INTO Inventory(IsOnKSUInventory,ItemName,InventoryDescription,InventoryQuantity,InventoryReorderQuantity,InventoryTypeID,InventoryLocationID) VALUES(@param1,@param2,@param3,@param4,@param5,@param6,@param7)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@param1", SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@param2", SqlDbType.VarChar, 50).Value = ItemName;
                cmd.Parameters.Add("@param3", SqlDbType.VarChar, 50).Value = ItemDescription;
                cmd.Parameters.Add("@param4", SqlDbType.Int).Value = ItemQuantity;
                cmd.Parameters.Add("@param5", SqlDbType.Int).Value = ItemReorder;
                cmd.Parameters.Add("@param6", SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@param7", SqlDbType.Int).Value = 1;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
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
            SqlConnection conn = new SqlConnection("SERVER=IPADDRESS,1433;Database=Inventory;USER ID=Capstone;PASSWORD=abc123");
            try
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM Inventory WHERE ItemName='" + ItemName + "'", conn))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                status.success = false;
                status.Message = ex.InnerException.ToString();
            }

            return Json(status, JsonRequestBehavior.DenyGet);
        }
    }
}
