using ceTe.DynamicPDF;
using ceTe.DynamicPDF.PageElements;
using KentStatePoliceInventory.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace KentStatePoliceInventory.Classes
{
    public class InventoryLocationReport
    {
        private Template template = new Template();
        private static PageDimensions pageDimensions = new PageDimensions(PageSize.Letter, PageOrientation.Portrait, 54.0f);
        private float currentY = 0;
        private bool alternateBG = false;
        private Page currentPage = null;
        private float bodyTop = 38;
        private float bodyBottom = pageDimensions.Body.Bottom - pageDimensions.Body.Top;

        public InventoryLocationReport()
        {
            // Run for all locations
        }

        public string RunReport()
        {
            string output = "";
            var document = new Document();
            document.Creator = "LocationReport.aspx";
            document.Author = "Capstone Team";
            document.Title = "Location Inventory Report";
            InventoryViewModel model = new InventoryViewModel();
            Configuration config = new Configuration();

            SetTemplate();
            document.Template = template;
            BuildDocument(document);

            try
            {
                var today = DateTime.Today.ToString("MMM-dd-yyyy");
                output = config.ReportSaveLocation + "LocationReport - " + today + ".pdf";
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

        private void BuildDocument(Document doc)
        {
            AddNewPage(doc);
            var locations = GetLocations();

            foreach (var location in locations)
            {
                var locatioModel = new LocationModel(location.LocationID);
                currentPage.Elements.Add(new Label(location.LocationName, 2, currentY + 5, 100, 11, ceTe.DynamicPDF.Font.TimesBold, 14, TextAlign.Left));
                template.Elements.Add(new Line(0, currentY + 20, 504, currentY + 20));
                template.Elements.Add(new Line(0, currentY + 23, 504, currentY + 23));
                currentY += 25;
                // Need to modify to filter for specific location
                foreach (var item in locatioModel.Items)
                {
                    AddRecord(doc, item);
                }
                currentY += 15;
            }
        }

        private void SetTemplate()
        {
            // Adds elements to the header template
            template.Elements.Add(new Label(DateTime.Now.ToString("dd MMM yyyy, H:mm:ss EST"), 0, 0, 504, 12, ceTe.DynamicPDF.Font.TimesRoman, 12));
            template.Elements.Add(new Label("KSU Police Department Locations", 0, 0, 504, 12, ceTe.DynamicPDF.Font.HelveticaBold, 12, TextAlign.Center));
            PageNumberingLabel pageNumLabel = new PageNumberingLabel("Page %%CP%% of %%TP%%", 0, 0, 504, 12, ceTe.DynamicPDF.Font.HelveticaBold, 12, TextAlign.Right);
            template.Elements.Add(pageNumLabel);
            template.Elements.Add(new Label("Product", 315, 23, 236, 11, ceTe.DynamicPDF.Font.TimesBold, 13));
            template.Elements.Add(new Label("Quantity", 345, 23, 156, 11, ceTe.DynamicPDF.Font.TimesBold, 13, TextAlign.Right));
            template.Elements.Add(new Line(315, 36, 504, 36));
        }

        private void AddRecord(Document document, InventoryItem item)
        {
            // Adds a new page to the document if needed
            if (currentY > bodyBottom) AddNewPage(document);


            if (item.ItemQuantity < item.ItemReorder)
            { // shows red for items that need reordering
                currentPage.Elements.Add(new ceTe.DynamicPDF.PageElements.Rectangle(0, currentY, 504, 18, RgbColor.Black, new WebColor("ff3333"), 0.0F));
            }
            else if (alternateBG)
            { // Adds alternating background to document
                currentPage.Elements.Add(new ceTe.DynamicPDF.PageElements.Rectangle(0, currentY, 504, 18, RgbColor.Black, new WebColor("E0E0FF"), 0.0F));
            }

            // Adds Labels to the document with data from current record
            currentPage.Elements.Add(new Label(item.ItemName, 315, currentY + 3, 236, 11, ceTe.DynamicPDF.Font.TimesRoman, 12));
            currentPage.Elements.Add(new Label(item.ItemQuantity.ToString(), 345, currentY + 3, 156, 11, ceTe.DynamicPDF.Font.TimesRoman, 12, TextAlign.Right));
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

        private List<Location> GetLocations()
        {
            var output = new List<Location>();
            Configuration config = new Configuration();
            SqlConnection conn = new SqlConnection(config.ConnectionString());
            try
            {
                conn.Open();
                string query = @"SELECT * FROM IssuedTo;";

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string ItemName = reader.GetString(reader.GetOrdinal("IssuedToDescription"));
                            long ItemRec = reader.GetInt64(reader.GetOrdinal("IssuedToID"));
                            output.Add(new Location(ItemName, Convert.ToInt32(ItemRec)));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new List<Location>();
            }
            return output;
        }
    }
}