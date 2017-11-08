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
            SqlConnection conn = new SqlConnection("SERVER=173.88.245.82,1433;Database=Inventory;USER ID=Capstone;PASSWORD=abc123");
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

        public List<Location> Locations { get; set; }
        public List<Location> IssuedToList { get; set; }
    }

}
