using System;
namespace KentStatePoliceInventory.Classes
{
    public class InventoryItem
    {
        public InventoryItem()
        {
            ItemName = String.Empty;
            ItemDescription = String.Empty;
            ItemQuantity = 10;
        }
        public InventoryItem(string name, int quantity, int reorder, string description = ""){
            ItemName = name;
            ItemQuantity = quantity;
            ItemDescription = description;
            ItemReorder = reorder;
        }

        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public int ItemQuantity { get; set; }
        public int ItemReorder { get; set; }
    }
}
