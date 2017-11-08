using System;
namespace KentStatePoliceInventory.Classes
{
    public class Configuration
    {
        public string IPAddress = String.Empty;
        public string Database = String.Empty;
        public string User = String.Empty;
        public string Password = String.Empty;

        public Configuration()
        {
            IPAddress = "no.88.245.82,1433";
            Database = "Inventory";
            User = "Capstone";
            Password = "abc123";
        }

        public string ConnectionString()
        {
            return "SERVER=" + IPAddress + ";Database=" + Database + 
                       ";USER ID=" + User + ";PASSWORD=" + Password;
        }
    }
}
