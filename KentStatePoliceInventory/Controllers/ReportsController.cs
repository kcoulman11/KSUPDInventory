using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ceTe.DynamicPDF;
using ceTe.DynamicPDF.PageElements;
using System.IO;
using KentStatePoliceInventory.Classes;
using KentStatePoliceInventory.Models;

namespace KentStatePoliceInventory.Controllers
{
    public class ReportsController : Controller
    {
        // Testing the pdfGenerator DLL
        private Template template = new Template();
        private static PageDimensions pageDimensions = new PageDimensions(PageSize.Letter, PageOrientation.Portrait, 54.0f);
        public Document document;
        private float currentY = 0;
        private bool alternateBG = false;        private Page currentPage = null;
        private float bodyTop = 38;
        private float bodyBottom = pageDimensions.Body.Bottom - pageDimensions.Body.Top;

        public ActionResult Index()
        {
            return View ();
        }

        // Put all C# code in here that will be called from the view
        public object MonthlyReport()
        {
            var newReport = new InventoryReport();
            if (!newReport.RunReport()) return null;
            else return true;
        }   
        
        public object GunReport()
        {
            return new object();
        }    
    }
}
