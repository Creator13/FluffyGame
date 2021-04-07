using UnityEngine;

namespace Fluffy
{
    public interface IInventory
    {
        void AddItem(string itemName);
    }
    
    public class InventoryLinker : MonoBehaviour, IInventory
    {
        [SerializeField] private Inventory inventory;

        public void AddItem(string item)
        {
            inventory.AddItem(item);
        }
    }
}
