using System;
using System.Collections.Generic;
using KentStatePoliceInventory.Classes;
using System.Data.Entity.SqlServer.Utilities;
using System.Data.SqlClient;
using MySql.Data.Types;
using System.Data.Odbc;
using System.Data;
using MySql;

namespace KentStatePoliceInventory.Models
{
    public class InventoryViewModel
    {
        public InventoryViewModel()
        {
            InventoryItems = new List<InventoryItem>();
            SqlConnection conn = new SqlConnection("SERVER=173.88.245.82,1433;Database=Inventory;USER ID=Capstone;PASSWORD=abc123");
            try
            {
                conn.Open();
                string query = "SELECT * FROM Inventory";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string ItemName = reader.GetString(reader.GetOrdinal("ItemName"));
                            int ItemQuantity = reader.GetInt32(reader.GetOrdinal("InventoryQuantity"));
                            int ItemReorder = reader.GetInt32(reader.GetOrdinal("InventoryReorderQuantity"));
                            string ItemDescription = reader.GetString(reader.GetOrdinal("InventoryDescription"));
                            InventoryItem item = new InventoryItem(ItemName, ItemQuantity, ItemReorder, ItemDescription);
                            InventoryItems.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
              InventoryItem NewInvenItem = new InventoryItem("Failed to connect to sql server", 0, 0, "contact administrator");
              InventoryItems.Add(NewInvenItem);
            }
            Locations = new List<Location>();
            SqlConnection conn2 = new SqlConnection("SERVER=173.88.245.82,1433;Database=Inventory;USER ID=Capstone;PASSWORD=abc123");
            try
            {
                conn2.Open();
                string query = "SELECT * FROM InventoryLocation";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string LocationName = reader.GetString(reader.GetOrdinal("InventoryLocationDescription"));
                            Locations.Add(new Location(LocationName));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Locations.Add(new Location("failed"));
            }


        }
            public List<InventoryItem> InventoryItems { get; set; }
            public List<Location> Locations { get; set; }
        }

    }
//https://stackoverflow.com/questions/12024226/how-to-generate-liststring-from-sql-query
