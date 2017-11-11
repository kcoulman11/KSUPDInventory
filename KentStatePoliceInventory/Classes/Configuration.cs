using System;
using System.Collections.Generic;

namespace KentStatePoliceInventory.Classes
{
    public class Configuration
    {
        public string IPAddress = String.Empty;
        public string Database = String.Empty;
        public string User = String.Empty;
        public string Password = String.Empty;

        public string FromAddress = "bsteige1@kent.edu";
        public string MailUser = "bsteige1";
        public string MailPassword = "Benny303"; // My mom made it and I never changed it. Get over it
        public List<string> ToAddresses = new List<string> { "bsteige1@kent.edu" };

        public Configuration()
        {
            IPAddress = "173.88.245.82,1433";
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
