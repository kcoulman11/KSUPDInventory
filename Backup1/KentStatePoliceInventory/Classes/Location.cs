using System;
namespace KentStatePoliceInventory.Classes
{
    public class Location
    {
        public Location()
        {
            LocationName = String.Empty;
        }

        public Location(string name)
        {
            LocationName = name;
        }

        public Location(string name, int id)
        {
            LocationName = name;
            LocationID = id;
        }

        public string LocationName { get; set; }
        public int LocationID { get; set; }
    }
}
