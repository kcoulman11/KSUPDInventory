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
        public InventoryItem(string name, int quantity, int reorder, string description, int typeid, int locationid, int itemid)
        {
          ItemName = name;
          ItemQuantity = quantity;
          ItemDescription = description;
          ItemReorder = reorder;
          TypeId = typeid;
          LocationId = locationid;
          ItemId = itemid;
        }

        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public int ItemId { get; set; }
        public int ItemQuantity { get; set; }
        public int ItemReorder { get; set; }
        public int TypeId { get; set;}
        public int LocationId { get; set; }
    }
}
