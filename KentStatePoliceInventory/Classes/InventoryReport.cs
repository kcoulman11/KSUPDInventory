using ceTe.DynamicPDF;
using ceTe.DynamicPDF.PageElements;
using KentStatePoliceInventory.Models;
using System;
using System.Data.SqlClient;
using System.IO;

namespace KentStatePoliceInventory.Classes
{
    public class InventoryReport
    {
        private Template template = new Template();
        private static PageDimensions pageDimensions = new PageDimensions(PageSize.Letter, PageOrientation.Portrait, 54.0f);
        private float currentY = 0;
        private bool alternateBG = false;
        private Page currentPage = null;
        private float bodyTop = 38;
        private float bodyBottom = pageDimensions.Body.Bottom - pageDimensions.Body.Top;

        public InventoryReport()
        {

        }

        public string RunReport()
        {
            string output = "";
            var document = new Document();
            document.Creator = "monthlyReport.aspx";
            document.Author = "Capstone Team";
            document.Title = "Monthly Inventory Report";
            InventoryViewModel model = new InventoryViewModel();
            Configuration config = new Configuration();

            SetTemplate();
            document.Template = template;
            BuildDocument(document, model);

            try
            {
                var today = DateTime.Today.ToString("MMM-dd-yyyy");
                output = config.ReportSaveLocation + "InventoryReport - " + today + ".pdf";
                var stream = new FileStream(output, FileMode.Create, FileAccess.Write);
                document.Draw(stream);
                stream.Close();
            }
            catch (Exception e)
            {
                return string.Empty;
            }
            return output;
        }

        private void BuildDocument(Document doc, InventoryViewModel model)
        {
            AddNewPage(doc);
            foreach (var item in model.InventoryItems)
            {
                AddRecord(doc, item);
            }
        }

        private void SetTemplate()
        {
            // Adds elements to the header template
            template.Elements.Add(new Label(DateTime.Now.ToString("dd MMM yyyy, H:mm:ss EST"), 0, 0, 504, 12, Font.TimesRoman, 12));
            template.Elements.Add(new Label("KSU Police Department Inventory", 0, 0, 504, 12, Font.HelveticaBold, 12, TextAlign.Center));
            PageNumberingLabel pageNumLabel = new PageNumberingLabel("Page %%CP%% of %%TP%%", 0, 0, 504, 12, Font.HelveticaBold, 12, TextAlign.Right);
            template.Elements.Add(pageNumLabel);
            template.Elements.Add(new Label("Product", 2, 23, 236, 11, Font.TimesBold, 13));
            template.Elements.Add(new Label("Qty Per Unit", 242, 23, 156, 11, Font.TimesBold, 13));
            template.Elements.Add(new Label("Reorder Point", 402, 23, 100, 11, Font.TimesBold, 13, TextAlign.Right));
            template.Elements.Add(new Line(0, 36, 504, 36));
        }

        private void AddRecord(Document document, InventoryItem item)
        {
            // Adds a new page to the document if needed
            if (currentY > bodyBottom) AddNewPage(document);


            if (item.ItemQuantity < item.ItemReorder)
            { // shows red for items that need reordering
                currentPage.Elements.Add(new Rectangle(0, currentY, 504, 18, RgbColor.Black, new WebColor("ff3333"), 0.0F));
            }
            else if (alternateBG)
            { // Adds alternating background to document
                currentPage.Elements.Add(new Rectangle(0, currentY, 504, 18, RgbColor.Black, new WebColor("E0E0FF"), 0.0F));
            }
            // Adds Labels to the document with data from current record
            currentPage.Elements.Add(new Label(item.ItemName, 2, currentY + 3, 236, 11, Font.TimesRoman, 11));
            currentPage.Elements.Add(new Label(item.ItemQuantity.ToString(), 242, currentY + 3, 156, 11, Font.TimesRoman, 11));
            currentPage.Elements.Add(new Label(item.ItemReorder.ToString(), 402, currentY + 3, 100, 11, Font.TimesRoman, 11, TextAlign.Right));
            // Toggles alternating background
            alternateBG = !alternateBG;
            // Increments the current Y position on the page
            currentY += 18;
        }

        private void AddNewPage(Document document)
        {
            // Adds a new page to the document
            currentPage = new Page(pageDimensions);
            currentY = bodyTop;
            alternateBG = false;
            document.Pages.Add(currentPage);
        }
    }
}