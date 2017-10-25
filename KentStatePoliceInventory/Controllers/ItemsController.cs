using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KentStatePoliceInventory.Models;
using KentStatePoliceInventory.Classes;

namespace KentStatePoliceInventory.Controllers
{
    public class ItemsController : Controller
    {
        public ActionResult ItemsView()
        {
            AddRemoveItemsModel model = new AddRemoveItemsModel();
            return View (model);
        }

        public JsonResult AddNewItem(string ItemName, int ItemQuantity, int ItemReorder, string ItemDescription){
            MethodStatus status = new MethodStatus();

            //using (SqlConnection connection = new SqlConnection("ConnectionStringHere"))
            //{
            //    using (SqlCommand command = new SqlCommand())
            //    {
            //        command.Connection = connection;            // <== lacking
            //        command.CommandType = CommandType.Text;
            //        command.CommandText = "INSERT into tbl_staff (staffName, userID, idDepartment) VALUES (@staffName, @userID, @idDepart)";
            //        command.Parameters.AddWithValue("@staffName", name);
            //        command.Parameters.AddWithValue("@userID", userId);
            //        command.Parameters.AddWithValue("@idDepart", idDepart);

            //        try
            //        {
            //            connection.Open();
            //            int recordsAffected = command.ExecuteNonQuery();
            //        }
            //        catch (SqlException)
            //        {
            //            // error here
            //        }
            //        finally
            //        {
            //            connection.Close();
            //        }
            //    }
            //}

            return Json(status, JsonRequestBehavior.DenyGet);
        }

        public JsonResult RemoveItem(string ItemName){
            MethodStatus status = new MethodStatus();

            //using (SqlConnection connection = new SqlConnection("ConnectionStringHere"))
            //{
            //    using (SqlCommand command = new SqlCommand())
            //    {
            //        command.Connection = connection;            // <== lacking
            //        command.CommandType = CommandType.Text;
            //        command.CommandText = "INSERT into tbl_staff (staffName, userID, idDepartment) VALUES (@staffName, @userID, @idDepart)";
            //        command.Parameters.AddWithValue("@staffName", name);
            //        command.Parameters.AddWithValue("@userID", userId);
            //        command.Parameters.AddWithValue("@idDepart", idDepart);

            //        try
            //        {
            //            connection.Open();
            //            int recordsAffected = command.ExecuteNonQuery();
            //        }
            //        catch (SqlException)
            //        {
            //            // error here
            //        }
            //        finally
            //        {
            //            connection.Close();
            //        }
            //    }
            //}

            return Json(status, JsonRequestBehavior.DenyGet);
        }
    }
}
