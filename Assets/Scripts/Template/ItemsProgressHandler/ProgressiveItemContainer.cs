using System;
using UnityEngine;

public class EquippedItemData
{
    public string progressionName;
    public string itemName;

    public EquippedItemData(string progressionName, string itemName)
    {
        this.progressionName = progressionName;
        this.itemName = itemName;
    }
}

public class ProgressiveItemContainer
{
    private string _progressionName;

    private ProgressiveItemData _itemData;
    public ProgressiveItemData ItemData => _itemData;
    
    private PlayerPrefsProperty<int> _itemProgress;
    private PlayerPrefsProperty<int> _itemForcedProgress;
    private PlayerPrefsProperty<bool> _unlockedToShopStatus;
    private PlayerPrefsProperty<bool> _unlockedToUseStatus;
    private PlayerPrefsProperty<bool> _viewedInShopStatus;
    private PlayerPrefsProperty<bool> _equipStatus;

    private EquippedItemData _equippedItemData;
    
    public Action<EquippedItemData> OnEquipped;
    
    public ProgressiveItemContainer(ProgressiveItemData itemData, string progressionName)
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
    }

    public void IncreaseProgress()
    {
        _itemProgress.Value++;
        Debug.Log($"{_itemData.itemName} progress == {_itemProgress.Value}. progress to unlock == {_itemData.progressToUnlock}. forced progress == {ForcedProgress()}");
    }
    public void IncreaseForcedProgress()
    {
        _itemForcedProgress.Value++;
    }
    
    public void UnlockToShop()
    {
        _unlockedToShopStatus.Value = true;
    }
    public bool IsUnlockedToShop()
    {
        if (IsUnlockedToShopByDefault() || IsUnlockedToUseByDefault())
        {
            return true;
        }

        return _unlockedToShopStatus.Value;
    }
    public bool IsUnlockedToShopByDefault()
    {
        return _itemData.unlockedToShopByDefault;
    }
    
    public void UnlockToUse()
    {
        _unlockedToUseStatus.Value = true;
    }
    public bool IsUnlockedToUse()
    {
        if (IsUnlockedToUseByDefault())
        {
            return true;
        }

        return _unlockedToUseStatus.Value;
    }
    public bool IsUnlockedToUseByDefault()
    {
        return _itemData.unlockedToUseByDefault;
    }

    public void TryUnlock()
    {
        if (!IsProgressPassed())
        {
            return;
        }
        
        UnlockToShop();
        
        if (!IsUnlockCompletely())
        {
            return;
        }
        
        UnlockToUse();
    }
    
    public bool IsProgressPassed()
    {
        return _itemProgress.Value >= _itemData.progressToUnlock;
    }
    
    public bool IsUnlockCompletely()
    {
        return _itemData.unlockCompletelyOnProgressPassed;
    }
    
    public int ProgressToUnlock()
    {
        return _itemData.progressToUnlock;
    }

    public int CurrentUnlockProgress()
    {
        return _itemProgress.Value;
    }
    public int ForcedProgress()
    {
        return _itemForcedProgress.Value;
    }

    public ItemPriceType PriceType()
    {
        return _itemData.priceType;
    }

    public int Price()
    {
        return _itemData.price;
    }
    
    public Sprite Icon()
    {
        return _itemData.icon;
    }

    public bool IsViewedInShop()
    {
        return _viewedInShopStatus.Value;
    }

    public void SetAsViewedInShop()
    {
        _viewedInShopStatus.Value = true;
    }

    public bool IsNewNotViewedInShop()
    {
        if (IsUnlockedToShopByDefault() || IsUnlockedToUseByDefault())
        {
            return false;
        }
        
        return IsUnlockedToShop() && !IsViewedInShop();
    }

    public void SetAsEquipped()
    {
        _equipStatus.Value = true;
        OnEquipped?.Invoke(_equippedItemData);
    }

    public void ResetEquipStatus()
    {
        _equipStatus.Value = false;
    }
    
    public bool IsEquipped()
    {
        return _equipStatus.Value;
    }
}