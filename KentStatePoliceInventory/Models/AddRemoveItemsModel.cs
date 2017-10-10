using System;
using System.Collections.Generic;
using KentStatePoliceInventory.Classes;

namespace KentStatePoliceInventory.Models
{
    public class AddRemoveItemsModel
    {
        public AddRemoveItemsModel()
        {
			InventoryItems = new List<InventoryItem>();
			InventoryItem NewInvenItem = new InventoryItem("Flare", 350, 100, "Its a flare yo");
			InventoryItems.Add(NewInvenItem);
			InventoryItem NewInvenItem1 = new InventoryItem("Shovel", 20, 10, "shovels r k00l");
			InventoryItems.Add(NewInvenItem1);
			InventoryItem NewInvenItem2 = new InventoryItem("Gun", 999999, 1000, "bang bang");
			InventoryItems.Add(NewInvenItem2);
			InventoryItem NewInvenItem3 = new InventoryItem("9mm Rounds", 58657, 10000, "Ammunition for 9mm");
			InventoryItems.Add(NewInvenItem3);
        }

        public List<InventoryItem> InventoryItems { get; set; }
    }
}
