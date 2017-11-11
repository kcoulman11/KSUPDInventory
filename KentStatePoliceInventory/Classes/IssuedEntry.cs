using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KentStatePoliceInventory.Classes
{
    public class IssuedEntry
    {
        public IssuedEntry()
        {
            IssuedID = -1;
            IssuedQuantity = -1;
            IssuedDate = DateTime.Now;
            ReturnedDate = null;
            UsedOrLostDate = null;
            InventoryID = -1;
            IssuedToID = -1;
        }

        public IssuedEntry(int issuedid, int quantity, DateTime issueddate, int itemid, int issuedtoid, DateTime? returneddate = null, DateTime? usedorlostdate = null)
        {
            IssuedID = issuedid;
            IssuedQuantity = quantity;
            IssuedDate = issueddate;
            IssuedToID = issuedtoid;
            InventoryID = itemid;
            ReturnedDate = returneddate;
            UsedOrLostDate = usedorlostdate;
        }

        public int IssuedID = -1; // id of row in Issued table
        public int IssuedQuantity = -1; // How many of this item are currently issued to this issuedto location
        public DateTime IssuedDate = DateTime.Now; // when was this item issued
        public DateTime? ReturnedDate = null; // when was this item returned
        public DateTime? UsedOrLostDate = null; // when was this item used/lost
        public int InventoryID = -1; // id of the item issued
        public int IssuedToID = -1; // id of the location issued to
        public string ItemName = String.Empty;
    }

}