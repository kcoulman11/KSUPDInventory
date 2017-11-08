using System;
using System.Collections.Generic;
using KentStatePoliceInventory.Classes;
using System.Data.SqlClient;

namespace KentStatePoliceInventory.Models
{
    public class AddRemoveItemsModel
    {
        public AddRemoveItemsModel()
        {
            InventoryItems = new List<InventoryItem>();
            Configuration config = new Configuration();
            SqlConnection conn = new SqlConnection(config.ConnectionString());
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
                InventoryItem NewInvenItem = new InventoryItem("failed to load form server", 0, 0, "fail");
                InventoryItems.Add(NewInvenItem);
          }

            Locations = new List<Location>();
            SqlConnection conn2 = new SqlConnection(config.ConnectionString());
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

            InventoryTypes = new List<string>();
            SqlConnection conn3 = new SqlConnection(config.ConnectionString());
            try
            {
                conn3.Open();
                string query = "SELECT * FROM InventoryType";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            InventoryTypes.Add(reader.GetString(reader.GetOrdinal("InventoryTypeDescription")));
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
        public List<string> InventoryTypes { get; set; }
    }
}
