using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ceTe.DynamicPDF;
using ceTe.DynamicPDF.PageElements;
using System.IO;

namespace KentStatePoliceInventory.Controllers
{
    public class ReportsController : Controller
    {
        // Testing the pdfGenerator DLL
        private Template template = new Template();
        public Document document;



        public ActionResult Index()
        {
            
            return View ();
        }

        // Put all C# code in here that will be called from the view
        public object MonthlyReport()
        {
            // Doing proof of concept
            // Create a document and set it's properties
            ceTe.DynamicPDF.Document document = new ceTe.DynamicPDF.Document();
            document.Creator = "HelloWorld.aspx";
            document.Author = "Ben";
            document.Title = "Working!";
            
            // Create a page to add to the document
            ceTe.DynamicPDF.Page page = new ceTe.DynamicPDF.Page(PageSize.Letter, PageOrientation.Portrait, 54.0f);
            // Create a Label to add to the page
            string text = "Hello ASPX C# World...\nFrom DynamicPDF Generator for .NET\nDynamicPDF.com";
            Label label = new Label(text, 0, 0, 504, 100, Font.Helvetica, 18, TextAlign.Center);
            // Add label to page
            page.Elements.Add(label);
            // Add page to document
            document.Pages.Add(page);
            // Outputs the document to the current web page
            //document.DrawToWeb("HelloWorld.pdf");

            var stream = new FileStream("C:\\test\\testing.pdf", FileMode.Create, FileAccess.Write);

            document.Draw("C:\\test\\testing2.pdf");
            document.Draw(stream);

            stream.Close();

            //// Create a document and set it's properties
            // document = new Document();
            // document.Creator = "SimpleReport.aspx";
            // document.Author = "Ben";
            // document.Title = "Simple Report";

            // // Adds elements to the header template
            // SetTemplate();
            // document.Template = template;
            //// Builds the report
            //var currentPageDimensions = new PageDimensions(PageSize.Letter, PageOrientation.Portrait, 54.0f);
            //var currentPage = new Page(currentPageDimensions);
            //document.Pages.Add(currentPage);
            return new JsonResult();
        }

        private void SetTemplate()
        {
        // Adds elements to the header template
        template.Elements.Add( new Label(DateTime.Now.ToString("dd MMM yyyy, H:mm:ss EST"), 0, 0, 504, 12,
        Font.HelveticaBold, 12 ) );
        template.Elements.Add( new Label( "Northwind Product List", 0, 0, 504, 12, Font.HelveticaBold, 12, TextAlign.Center
        ) );
        PageNumberingLabel pageNumLabel = new PageNumberingLabel("Page %%CP%% of %%TP%%", 0, 0, 504, 12,
        Font.HelveticaBold, 12, TextAlign.Right);
        template.Elements.Add( pageNumLabel );
        template.Elements.Add( new Label( "Product", 2, 23, 236, 11, Font.TimesBold, 11 ) );
        template.Elements.Add( new Label( "Qty Per Unit", 242, 23, 156, 11, Font.TimesBold, 11 ) );
        template.Elements.Add( new Label( "Unit Price", 402, 23, 100, 11, Font.TimesBold, 11, TextAlign.Right ) );
        template.Elements.Add( new Line( 0, 36, 504, 36 ) );

         // Uncomment the line below to add a layout grid to each page
         //template.Elements.Add( new LayoutGrid() );
        }



}
}
