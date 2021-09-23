using UnityEngine;

public class ProgressiveItemContainer
{
    private ProgressiveItemData _itemData;
    public ProgressiveItemData ItemData => _itemData;
    
    private PlayerPrefsProperty<int> _itemProgress;
    private PlayerPrefsProperty<bool> _unlockedToShopStatus;
    private PlayerPrefsProperty<bool> _unlockedToUseStatus;
    private PlayerPrefsProperty<bool> _viewedInShopStatus;

    public ProgressiveItemContainer(ProgressiveItemData itemData)
    {
        _itemData = itemData;
        _itemProgress = new PlayerPrefsProperty<int>($"{_itemData._itemName}Progress", 0);
        _unlockedToShopStatus = new PlayerPrefsProperty<bool>($"{_itemData._itemName}IsAvailableToShop", false);
        _unlockedToUseStatus = new PlayerPrefsProperty<bool>($"{_itemData._itemName}IsAvailableToUse", false);
        _viewedInShopStatus = new PlayerPrefsProperty<bool>($"{itemData._itemName}IsViewedInShop", false);
    }

    public void IncreaseProgress()
    {
        _itemProgress.Value++;
    }
    public void UnlockToShop()
    {
        _unlockedToShopStatus.Value = true;
    }
    public bool IsUnlockedToShop()
    {
        if (IsUnlockedByDefault())
        {
            return true;
        }

        return _unlockedToShopStatus.Value;
    }

    public bool IsUnlockedByDefault()
    {
        return _itemData._unlockedByDefault;
    }
    
    public void UnlockToUse()
    {
        _unlockedToUseStatus.Value = true;
    }
    public bool IsUnlockedToUse()
    {
        if (IsUnlockedByDefault())
        {
            return true;
        }

        return _unlockedToUseStatus.Value;
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
        return _itemProgress.Value >= _itemData._progressToUnlock;
    }
    
    public bool IsUnlockCompletely()
    {
        return _itemData._unlockCompletely;
    }
    
    public int ProgressToUnlock()
    {
        return _itemData._progressToUnlock;
    }

    public int CurrentUnlockProgress()
    {
        return _itemProgress.Value;
    }

    public int Cost()
    {
        return _itemData._cost;
    }
    
    public Sprite Icon()
    {
        return _itemData._icon;
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
        if (_itemData._unlockedByDefault)
        {
            return false;
        }
        
        return IsUnlockedToShop() && !IsViewedInShop();
    }
}