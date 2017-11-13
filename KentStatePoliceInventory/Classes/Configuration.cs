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

        public string FromAddress = "test@kent.edu";
        public string MailUser = "test";
        public string MailPassword = "password"; // My mom made it and I never changed it. Get over it
        public List<string> ToAddresses = new List<string> { "kcoulman889@gmail.com" };

        public string ReportSaveLocation = "C:\\test\\";

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
