using System.Collections.Generic;
using UnityEngine;

namespace Templates.ItemSystems.InventorySystem
{
    [CreateAssetMenu(fileName = "InventoryItemsGroup", menuName = "Item Systems/InventoryItemsGroup")]
    public class InventoryItemsGroup : ScriptableObject
    {
        public string Name;
        public List<InventoryItemData> Items;
    }
}