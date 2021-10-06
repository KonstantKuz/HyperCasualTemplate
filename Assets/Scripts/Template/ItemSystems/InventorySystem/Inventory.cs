using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Template.ItemSystems.InventorySystem
{
    public class Inventory : Singleton<Inventory>
    {
        [SerializeField] private InventoryItemsGroup[] _itemsGroups;

        private Dictionary<string, InventoryItem> _itemsDictionary;

        private Dictionary<string, InventoryItem> ItemsDictionary
        {
            get
            {
                if (_itemsDictionary == null)
                {
                    InitializeItemsDictionary();
                }

                return _itemsDictionary;
            }
        }

        private void InitializeItemsDictionary()
        {
            _itemsDictionary = new Dictionary<string, InventoryItem>();
        
            foreach (InventoryItemsGroup group in _itemsGroups)
            {
                foreach (InventoryItemData inventoryData in group.Items)
                {
                    InventoryItem inventoryItem = new InventoryItem(group.Name, inventoryData, ResetGroupsEquipStatus);
                    _itemsDictionary.Add(inventoryData.Name, inventoryItem);
                }
            }
        }

        private void ResetGroupsEquipStatus(EquippedItemData equippedItemData)
        {
            InventoryItemsGroup group = GetItemsGroup(equippedItemData.GroupName);
            foreach (InventoryItemData inventoryData in group.Items.Where(inventoryData => inventoryData.Name != equippedItemData.Name))
            {
                ItemsDictionary[inventoryData.Name].ResetEquipStatus();
            }
        }
        
        private InventoryItemsGroup GetItemsGroup(string groupName)
        {
            return _itemsGroups.First(group => group.Name == groupName);
        }

        public InventoryItem GetItem(string itemName)
        {
            return ItemsDictionary[itemName];
        }
        
        public bool IsAllItemsUnlockedToUse(string groupName)
        {
            InventoryItemsGroup group = GetItemsGroup(groupName);
            return group.Items.All(inventoryData => ItemsDictionary[inventoryData.Name].IsUnlockedToUse());
        }
    }
}
