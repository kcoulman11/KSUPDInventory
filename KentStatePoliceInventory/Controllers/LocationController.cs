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
    public class LocationController : Controller
    {
        public ActionResult Index()
        {
            LocationModel model = new LocationModel();
            return View(model);
        }

        [HttpPost]
        public JsonResult AddNewLocation(string locationName)
        {
            MethodStatus status = new MethodStatus();
            Random random = new Random();
            int randomNumber = random.Next(0, 100);
            Configuration config = new Configuration();
            List<InventoryItem> InventoryItems = new List<InventoryItem>();
            SqlConnection conn = new SqlConnection(config.ConnectionString());
            try
            {
                conn.Open();
                string sql = "INSERT INTO InventoryLocation(InventoryLocationDescription) VALUES(@param1)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@param1", SqlDbType.VarChar, 50).Value = locationName;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                status.success = false;
                status.Message = ex.InnerException.ToString();
            }

            return Json(status, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult AddIssue(string locationName, int issuedType)
        {
            MethodStatus status = new MethodStatus();
            Random random = new Random();
            int randomNumber = random.Next(0, 100);
            Configuration config = new Configuration();
            List<InventoryItem> InventoryItems = new List<InventoryItem>();
            SqlConnection conn = new SqlConnection(config.ConnectionString());
            try
            {
                conn.Open();
                string sql = "INSERT INTO Issuedto(IssuedToDescription,IssuedToTypeID) VALUES(@param1,@param2)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@param1", SqlDbType.VarChar, 50).Value = locationName;
                cmd.Parameters.Add("@param2", SqlDbType.Int).Value = issuedType;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                status.success = false;
                status.Message = ex.InnerException.ToString();
            }

            return Json(status, JsonRequestBehavior.DenyGet);
        }


        [HttpPost]
        public JsonResult RemoveLocation(string locationName)
        {
            MethodStatus status = new MethodStatus();
            Configuration config = new Configuration();
            SqlConnection conn = new SqlConnection(config.ConnectionString());
            try
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT InventoryLocationID FROM InventoryLocation Where InventoryLocationDescription = @location";
                    cmd.Parameters.AddWithValue("location", locationName);
                    string str = Convert.ToString(cmd.ExecuteScalar());
                    int locationid = Int32.Parse(str);
                    int hasinventory = 0;
                    cmd.CommandText = "SELECT COUNT (InventoryLocationID) FROM Inventory WHERE InventoryLocationID = @param1";
                    cmd.Parameters.AddWithValue("@param1", locationid);
                    string str2 = Convert.ToString(cmd.ExecuteScalar());
                    hasinventory = Int32.Parse(str2);

                    if (hasinventory == 0)
                    {

                        cmd.CommandText = "DELETE FROM InventoryLocation WHERE InventoryLocationDescription = @word";
                        cmd.Parameters.AddWithValue("@word", locationName);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        status.success = false;
                        status.Message = "Cannot remove location with items in it";
                    }

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
        public JsonResult RemoveIssue(string locationName)
        {
            MethodStatus status = new MethodStatus();
            Configuration config = new Configuration();
            SqlConnection conn = new SqlConnection(config.ConnectionString());
            try
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT IssuedToID FROM IssuedTo Where IssuedtoDescription = @issuedlocation";
                    cmd.Parameters.AddWithValue("issuedlocation", locationName);
                    string str = Convert.ToString(cmd.ExecuteScalar());
                    int locationid = Int32.Parse(str);
                    int hasinventory = 0;
                    cmd.CommandText = "SELECT COUNT (IssuedToID) FROM Issued WHERE IssuedtoID = @param1";
                    cmd.Parameters.AddWithValue("@param1", locationid);
                    string str2 = Convert.ToString(cmd.ExecuteScalar());
                    hasinventory = Int32.Parse(str2);

                    if (hasinventory == 0)
                    {
                        cmd.CommandText = "DELETE FROM IssuedTo WHERE IssuedToDescription = @word";
                        cmd.Parameters.AddWithValue("@word", locationName);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        status.success = false;
                        status.Message = "Cannot remove location with items issued to it";
                    }
                }
            }
            catch (Exception ex)
            {
                status.success = false;
                status.Message = "Items issued to location must be removed";

            }

            return Json(status, JsonRequestBehavior.DenyGet);
        }

    }
}
