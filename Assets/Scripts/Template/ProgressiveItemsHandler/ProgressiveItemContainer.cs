using System;
using Template.Tools;
using UnityEngine;

namespace Template.ProgressiveItemsHandler
{
    public class ProgressiveItemContainer
    {
        private string _progressionName;

        private ProgressiveItemData _itemData;
        private PlayerPrefsProperty<int> _itemProgress;
        private PlayerPrefsProperty<int> _itemForcedProgress;
        private PlayerPrefsProperty<bool> _unlockedToShopStatus;
        private PlayerPrefsProperty<bool> _unlockedToUseStatus;
        private PlayerPrefsProperty<bool> _viewedInShopStatus;
        private PlayerPrefsProperty<bool> _equipStatus;

        private EquippedItemData _equippedItemData;
    
        private Action<EquippedItemData> _onEquipped;
    
        public ProgressiveItemContainer(string progressionName, ProgressiveItemData itemData, Action<EquippedItemData> onEquipped)
        {
            _progressionName = progressionName;
            _itemData = itemData;
            _itemProgress = new PlayerPrefsProperty<int>($"{_itemData.itemName}Progress", 0);
            _itemForcedProgress = new PlayerPrefsProperty<int>($"{_itemData.itemName}ForcedProgress", 0);
            _unlockedToShopStatus = new PlayerPrefsProperty<bool>($"{_itemData.itemName}IsAvailableToShop", false);
            _unlockedToUseStatus = new PlayerPrefsProperty<bool>($"{_itemData.itemName}IsAvailableToUse", false);
            _viewedInShopStatus = new PlayerPrefsProperty<bool>($"{itemData.itemName}IsViewedInShop", false);
            _equipStatus = new PlayerPrefsProperty<bool>($"{itemData.itemName}IsEquipped", false);
            _equippedItemData = new EquippedItemData(_progressionName, _itemData.itemName);
            _onEquipped = onEquipped;
        }

        public string Name() => _itemData.itemName;
        public void IncreaseProgress() => _itemProgress.Value++;
        public void IncreaseForcedProgress() => _itemForcedProgress.Value++;
    
        public void UnlockToShop() => _unlockedToShopStatus.Value = true;
        public bool IsUnlockedToShop() => IsUnlockedToShopByDefault() || IsUnlockedToUseByDefault() || _unlockedToShopStatus.Value;
        public bool IsUnlockedToShopByDefault() => _itemData.unlockedToShopByDefault;
    
        public void UnlockToUse() => _unlockedToUseStatus.Value = true;
        public bool IsUnlockedToUse() => IsUnlockedToUseByDefault() || _unlockedToUseStatus.Value;
        public bool IsUnlockedToUseByDefault() => _itemData.unlockedToUseByDefault;

        public void TryUnlock()
        {
            if (!IsProgressPassed())
            {
                return;
            }
        
            UnlockToShop();
        
            if (!IsUnlockCompletelyOnProgressPassed())
            {
                return;
            }
        
            UnlockToUse();
        }

        public bool IsProgressPassed() => CurrentUnlockProgress() >= ProgressToUnlock();
        public bool IsUnlockCompletelyOnProgressPassed() => _itemData.unlockCompletelyOnProgressPassed;
    
        public int ProgressToUnlock() => _itemData.progressToUnlock;
        public int CurrentUnlockProgress() => _itemProgress.Value;
        public int ForcedProgress() => _itemForcedProgress.Value;

        public void SetAsViewedInShop() => _viewedInShopStatus.Value = true;
        public bool IsNewNotViewedInShop()
        {
            if (IsUnlockedToShopByDefault() || IsUnlockedToUseByDefault())
            {
                return false;
            }
        
            return IsUnlockedToShop() && !IsViewedInShop();
        }
        public bool IsViewedInShop() => _viewedInShopStatus.Value;

        public void SetAsEquipped()
        {
            _equipStatus.Value = true;
            _onEquipped.Invoke(_equippedItemData);
        }
        public void ResetEquipStatus() => _equipStatus.Value = false;
        public bool IsEquipped() => _equipStatus.Value;
    
        public ItemPriceType PriceType() => _itemData.priceType;
        public int Price() => _itemData.price;
        public Sprite Icon() => _itemData.icon;
    }
}