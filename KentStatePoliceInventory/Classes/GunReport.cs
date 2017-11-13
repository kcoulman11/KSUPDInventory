using ceTe.DynamicPDF;
using ceTe.DynamicPDF.PageElements;
using KentStatePoliceInventory.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace KentStatePoliceInventory.Classes
{

    public class GunReport
    {
        private Template template = new Template();
        private static PageDimensions pageDimensions = new PageDimensions(PageSize.Letter, PageOrientation.Portrait, 54.0f);
        private float currentY = 0;
        private bool alternateBG = false;
        private Page currentPage = null;
        private float bodyTop = 38;
        private float bodyBottom = pageDimensions.Body.Bottom - pageDimensions.Body.Top;

        public string RunReport()
        {
            Document document = new Document();
            string output = String.Empty;
            document.Creator = "gunReport.aspx";
            document.Author = "Capstone Team";
            document.Title = "Gun Report";
            Configuration config = new Configuration();

            SetTemplate();
            document.Template = template;
            BuildDocument(document);

            try
            {
                var today = DateTime.Today.ToString("MMM-dd-yyyy");
                output = config.ReportSaveLocation + "Gun Report-" + today + ".pdf";
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
            var GunInventory = GetGuns();
            foreach (var item in GunInventory)
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
            template.Elements.Add(new Label("Product", 4, 23, 236, 11, Font.TimesBold, 11));
            template.Elements.Add(new Label("Inventory Location", 165, 23, 156, 11, Font.TimesBold, 11, TextAlign.Left));
            template.Elements.Add(new Label("Serial Number", 402, 23, 100, 11, Font.TimesBold, 11, TextAlign.Right));
            template.Elements.Add(new Line(0, 36, 504, 36));
        }

        private void AddRecord(Document document, InventoryItem item)
        {
            // Adds a new page to the document if needed
            if (currentY > bodyBottom) AddNewPage(document);

            // Adds alternating background to document
            if (alternateBG)
            {
                currentPage.Elements.Add(new ceTe.DynamicPDF.PageElements.Rectangle(0, currentY, 504, 18, RgbColor.Black, new WebColor("E0E0FF"), 0.0F));
            }

            // Adds Labels to the document with data from current record
            currentPage.Elements.Add(new Label(item.ItemName, 4, currentY + 3, 236, 11, Font.TimesRoman, 11));
            currentPage.Elements.Add(new Label(item.ItemDescription.ToString(), 165, currentY + 3, 156, 11, Font.TimesRoman, 11, TextAlign.Left));
            currentPage.Elements.Add(new Label(item.SerialNumber.ToString(), 402, currentY + 3, 100, 11, Font.TimesRoman, 11, TextAlign.Right));
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

        private List<InventoryItem> GetGuns()
        {
            var output = new List<InventoryItem>();

            SqlConnection conn = new SqlConnection("SERVER=173.88.245.82,1433;Database=Inventory;USER ID=Capstone;PASSWORD=abc123");
            try
            {
                conn.Open();
                string query = @"SELECT ItemName, GunSerial, InventoryLocationDescription FROM Inventory 
                                JOIN InventoryLocation ON Inventory.InventoryLocationID = InventoryLocation.InventoryLocationID
                                WHERE GunSerial IS NOT NULL;";

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string ItemName = reader.GetString(reader.GetOrdinal("ItemName"));
                            string SerialNumber = reader.GetString(reader.GetOrdinal("GunSerial"));
                            string LocationDescription = reader.GetString(reader.GetOrdinal("InventoryLocationDescription"));

                            output.Add(new InventoryItem(ItemName, 0, 0, LocationDescription, SerialNumber));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new List<InventoryItem>();
            }
            return output;
        }
    }
}

