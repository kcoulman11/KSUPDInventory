using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KentStatePoliceInventory.Models;

namespace KentStatePoliceInventory.Controllers
{
    public class UsersController : Controller
    {
        public ActionResult UserView()
        {
            UserViewModel model = new UserViewModel();
            return View (model);
        }
    }
}
