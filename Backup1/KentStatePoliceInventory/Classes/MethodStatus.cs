using System;
using KentStatePoliceInventory.Classes;
using System.Collections.Generic;

namespace KentStatePoliceInventory.Classes
{
    public class MethodStatus
    {
        public string Message { get; set; } = String.Empty;
        public bool success { get; set; } = true;
    }

    public class InventoryStatus
    {
        public string Message { get; set; } = String.Empty;
        public bool success { get; set; } = true;
        public List<InventoryItem> items = new List<InventoryItem>();
    }
}
