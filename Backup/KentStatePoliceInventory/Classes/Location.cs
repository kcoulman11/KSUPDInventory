using System;
namespace KentStatePoliceInventory.Classes
{
    public class Location
    {
        public Location()
        {
            LocationName = String.Empty;
        }

        public Location(string name){
            LocationName = name;
        }

        public string LocationName { get; set; }
    }
}
