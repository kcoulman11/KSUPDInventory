using System;
using System;
using System.Collections.Generic;
using KentStatePoliceInventory.Classes;
using System.Data.Entity.SqlServer.Utilities;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System.Data.Odbc;
using System.Data;
using MySql;
using System.Data.SqlClient;

namespace KentStatePoliceInventory.Models
{
    public class LocationModel
    {
        public LocationModel()
        {
            Locations = new List<Location>();
            Configuration config = new Configuration();
            SqlConnection conn = new SqlConnection(config.ConnectionString());
            try
            {
                conn.Open();
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
                IssuedToList = new List<Location>();
                query = "SELECT * FROM IssuedTo";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                          string DescriptionName = reader.GetString(reader.GetOrdinal("IssuedToDescription"));
                          IssuedToList.Add(new Location(DescriptionName));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Locations.Add(new Location("failed"));
            }
        }

        public LocationModel(int Id)
        {
            Configuration config = new Configuration();
            LocationID = Id;
            SqlConnection conn = new SqlConnection(config.ConnectionString());
            try
            {
                conn.Open();
                string query = @"SELECT Inventory.ItemName, Issued.IssuedQuantity, IssuedTo.IssuedToDescription FROM Issued 
	                            INNER JOIN Inventory ON Issued.InventoryID = Inventory.InventoryID 
	                            INNER JOIN IssuedTo  ON IssuedTo.IssuedToID = Issued.IssuedToID
		                        WHERE IssuedTo.IssuedToID = '" + LocationID + "';";

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string ItemName = reader.GetString(reader.GetOrdinal("ItemName"));
                            int ItemQuantity = reader.GetInt32(reader.GetOrdinal("IssuedQuantity"));
                            string LocationDescription = reader.GetString(reader.GetOrdinal("IssuedToDescription"));
                            InventoryItem item = new InventoryItem(ItemName, ItemQuantity, 0, LocationDescription);
                            Items.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Something
            }
        }

        public List<Location> Locations { get; set; }
        public List<Location> IssuedToList { get; set; }
        private int LocationID;
        public List<InventoryItem> Items = new List<InventoryItem>();

    }

}
