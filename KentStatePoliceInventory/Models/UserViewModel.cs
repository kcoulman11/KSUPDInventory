using System;
using System.Collections.Generic;
using KentStatePoliceInventory.Classes;

namespace KentStatePoliceInventory.Models
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            Users = new List<User>();
            User newUser = new User("Officer Steve", "Administrator", 123456);
            User newUser1 = new User("Officer Rick", "Administrator", 528248);
            User newUser2 = new User("Officer Tone", "Administrator", 113824);
            User newUser3 = new User("Officer Andreas", "Standard", 395717);
            Users.Add(newUser);
            Users.Add(newUser1);
            Users.Add(newUser2);
            Users.Add(newUser3);
        }  

        public List<User> Users { get; set; }
    }
}