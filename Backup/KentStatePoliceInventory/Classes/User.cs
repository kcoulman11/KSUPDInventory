using System;
namespace KentStatePoliceInventory.Classes
{
    public class User
    {
        public User()
        {
            UserName = String.Empty;
            AccessLevel = String.Empty;
            BadgeId = 0;
        }

        public User(string name, string level, int badge){
            UserName = name;
            AccessLevel = level;
            BadgeId = badge;
        }
		
        public string UserName { get; set; }
        public string AccessLevel { get; set; }
        public int BadgeId { get; set; }
	}
}
