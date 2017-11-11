using System.Web.Mvc;
using KentStatePoliceInventory.Classes;

namespace KentStatePoliceInventory.Controllers
{
    public class ReportsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult InventoryReport()
        {
            var newReport = new InventoryReport();
            var status = new ReportStatus();
            status.ReportLocation = newReport.RunReport();
            if (string.IsNullOrWhiteSpace(status.ReportLocation)) status.success = false;
            else status.success = true;

            return Json(status, JsonRequestBehavior.DenyGet);
        }

        public JsonResult SendMail(string reportLocation)
        {
            var mail = new MailReport();
            MethodStatus status = new MethodStatus();
            if (string.IsNullOrWhiteSpace(reportLocation)) status.success = false;
            else status.success = mail.SendReport(reportLocation);

            return Json(status, JsonRequestBehavior.DenyGet);
        }

        public JsonResult LocationReport()
        {
            var locationReport = new InventoryLocationReport();
            var status = new ReportStatus();
            status.ReportLocation = locationReport.RunReport();
            if (string.IsNullOrWhiteSpace(status.ReportLocation)) status.success = false;
            else status.success = true;

            return Json(status, JsonRequestBehavior.DenyGet);
        }

        public JsonResult GunReport()
        {
            var GunReport = new GunReport();
            var status = new ReportStatus();
            status.ReportLocation = GunReport.RunReport();
            if (string.IsNullOrWhiteSpace(status.ReportLocation)) status.success = false;
            else status.success = true;

            return Json(status, JsonRequestBehavior.DenyGet);
        }
    }
}
