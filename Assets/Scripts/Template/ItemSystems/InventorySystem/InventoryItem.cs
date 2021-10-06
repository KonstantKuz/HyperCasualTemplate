using System;
using Template.Tools;
using UnityEngine;

namespace Template.ItemSystems.InventorySystem
{
    public class InventoryItem
    {
        private InventoryItemData _itemData;
        private PlayerPrefsProperty<bool> _unlockedToUseStatus;
        private PlayerPrefsProperty<bool> _equipStatus;

        private EquippedItemData _equippedItemData;
    
        private Action<EquippedItemData> _onEquipped;
    
        public InventoryItem(string groupName, InventoryItemData itemData, Action<EquippedItemData> onEquipped)
        {
            _itemData = itemData;
            _unlockedToUseStatus = new PlayerPrefsProperty<bool>($"{Name}IsAvailableToUse", false);
            _equipStatus = new PlayerPrefsProperty<bool>($"{Name}IsEquipped", false);
            _equippedItemData = new EquippedItemData(groupName, Name);
            _onEquipped = onEquipped;
        }

        public string Name => _itemData.Name;
        public Sprite Icon => _itemData.Icon;
    
        public void UnlockToUse() => _unlockedToUseStatus.Value = true;
        public bool IsUnlockedToUse() => IsUnlockedToUseByDefault() || _unlockedToUseStatus.Value;
        public bool IsUnlockedToUseByDefault() => _itemData.UnlockedToUseByDefault;

        public void SetAsEquipped()
        {
            _equipStatus.Value = true;
            _onEquipped.Invoke(_equippedItemData);
        }
        public void ResetEquipStatus() => _equipStatus.Value = false;
        public bool IsEquipped() => _equipStatus.Value;
    }
}