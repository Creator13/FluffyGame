using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fluffy
{
    public class Inventory : MonoBehaviour, IInventory
    {
        public event Action Updated;  
        
        [SerializeField] private List<string> items;

        public void AddItem(string item)
        {
            items.Add(item);
            Updated?.Invoke();
        }

        public void RemoveItem(string item)
        {
            items.Remove(item);
            Updated?.Invoke();
        }

        public int ItemCount(string item)
        {
            return items.Count(itemInList => itemInList == item);
        }

        public bool Has(string item)
        {
            return ItemCount(item) > 0;
        }
    }
}
