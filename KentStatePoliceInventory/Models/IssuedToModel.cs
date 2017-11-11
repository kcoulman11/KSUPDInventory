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
    public class IssuedToModel
    {
        public IssuedToModel()
        {

            Configuration config = new Configuration();
            SqlConnection conn = new SqlConnection(config.ConnectionString());
            IssuedToLocations = new List<Location>();
            try{
                conn.Open();
                string query = "SELECT * FROM IssuedTo";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string DescriptionName = reader.GetString(reader.GetOrdinal("IssuedToDescription"));
                            IssuedToLocations.Add(new Location(DescriptionName));
                        }
                    }
                }
            }
            catch (Exception ex){
                IssuedToLocations.Add(new Location("failed"));
            }
        }

        public List<InventoryItem> IssuedItems = new List<InventoryItem>();
        public List<Location> IssuedToLocations { get; set; }
    }
}
