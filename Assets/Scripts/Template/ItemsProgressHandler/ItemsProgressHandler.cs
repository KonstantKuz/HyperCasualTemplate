using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsProgressHandler : Singleton<ItemsProgressHandler>
{
    [SerializeField] private ItemsProgression[] _progressions;

    private Dictionary<string, ItemsProgression> _progressionsDictionary;
    private Dictionary<string, ProgressiveItemData> _itemsDictionary;
    private Dictionary<string, PlayerPrefsProperty<int>> _itemProgressDictionary;
    private Dictionary<string, PlayerPrefsProperty<bool>> _itemStatusDictionary;

    private const string PrefsPrefixItemProgress = "Progress";
    private const string PrefsPrefixItemShopStatus = "AvailableToShop";
    private const string PrefsPrefixItemUseStatus = "AvailableToUse";
    
    private void Awake()
    {
        InitializeItemsDictionary();
        Observer.Instance.OnLoadNextScene += UpdateItemsProgress;
    }

    private void InitializeItemsDictionary()
    {
        _progressionsDictionary= new Dictionary<string, ItemsProgression>();
        _itemsDictionary = new Dictionary<string, ProgressiveItemData>();
        _itemProgressDictionary = new Dictionary<string, PlayerPrefsProperty<int>>();
        _itemStatusDictionary = new Dictionary<string, PlayerPrefsProperty<bool>>();
        
        for (int i = 0; i < _progressions.Length; i++)
        {
            _progressionsDictionary.Add(_progressions[i].progressionName, _progressions[i]);
            
            for (int j = 0; j < _progressions[i].itemsQueue.Length; j++)
            {
                string itemName = _progressions[i].itemsQueue[j]._itemName;
                
                _itemsDictionary.Add(itemName, _progressions[i].itemsQueue[j]);
                _itemProgressDictionary.Add(itemName, new PlayerPrefsProperty<int>(PrefsPrefixItemProgress + itemName, 0));

                string shopStatusKey = PrefsPrefixItemShopStatus + itemName;
                _itemStatusDictionary.Add(shopStatusKey, new PlayerPrefsProperty<bool>(shopStatusKey, false));

                string useStatusKey = PrefsPrefixItemUseStatus + itemName;
                _itemStatusDictionary.Add(useStatusKey, new PlayerPrefsProperty<bool>(useStatusKey, false));
            }
        }
    }

    private void UpdateItemsProgress()
    {
        foreach (ItemsProgression progression in _progressions)
        {
            foreach (ProgressiveItemData itemData in progression.itemsQueue)
            {
                string itemName = itemData._itemName;
                
                IncreaseItemProgress(itemName);
                if (IsItemProgressPassed(itemName))
                {
                    UnlockItemToShop(itemName);
                    if (UnlockOnlyForLevels(itemName))
                    {
                        UnlockItemToUse(itemName);
                    }
                }
            }
        }
    }

    public void IncreaseItemProgress(string itemName)
    {
        _itemProgressDictionary[itemName].Value++;
    }
    public bool IsItemProgressPassed(string itemName)
    {
        return _itemProgressDictionary[itemName].Value >= _itemsDictionary[itemName]._levelsCountToGet;
    }
    public bool UnlockOnlyForLevels(string itemName)
    {
        return _itemsDictionary[itemName]._unlockOnlyForLevels;
    }
    
    public void UnlockItemToShop(string itemName)
    {
        string shopStatusKey = PrefsPrefixItemShopStatus + itemName;
        _itemStatusDictionary[shopStatusKey].Value = true;
    }
    public bool IsItemUnlockedToShop(string itemName)
    {
        if (IsItemUnlockedByDefault(itemName))
        {
            return true;
        }
        
        string shopStatusKey = PrefsPrefixItemShopStatus + itemName;
        return _itemStatusDictionary[shopStatusKey].Value;
    }

    public bool IsItemUnlockedByDefault(string itemName)
    {
        ProgressiveItemData itemData = _itemsDictionary[itemName];
        if (itemData._unlockedByDefault)
        {
            return true;
        }
        return false;
    }
    
    public bool IsAllItemsUnlockedToShop(string progressionName)
    {
        ItemsProgression progression = _progressionsDictionary[progressionName];
        for (int i = 0; i < progression.itemsQueue.Length; i++)
        {
            if (!IsItemUnlockedToShop(progression.itemsQueue[i]._itemName))
            {
                return false;
            }
        }
        return true;
    }

    public void UnlockItemToUse(string itemName)
    {
        string useStatusKey = PrefsPrefixItemUseStatus + itemName;
        _itemStatusDictionary[useStatusKey].Value = true;
    }
    public bool IsItemUnlockedToUse(string itemName)
    {
        if (IsItemUnlockedByDefault(itemName))
        {
            return true;
        }

        string useStatusKey = PrefsPrefixItemUseStatus + itemName;
        return _itemStatusDictionary[useStatusKey].Value;
    }
    public bool IsAllItemsUnlockedToUse(string progressionName)
    {
        ItemsProgression progression = _progressionsDictionary[progressionName];
        for (int i = 0; i < progression.itemsQueue.Length; i++)
        {
            if (!IsItemUnlockedToUse(progression.itemsQueue[i]._itemName))
            {
                return false;
            }
        }
        return true;
    }

    public int ItemCost(string itemName)
    {
        return _itemsDictionary[itemName]._cost;
    }
    
    public int ItemLevelsCountToGet(string itemName)
    {
        return _itemsDictionary[itemName]._levelsCountToGet;
    }
    
    public Sprite ItemIcon(string itemName)
    {
        return _itemsDictionary[itemName]._icon;
    }

    public int ItemCurrentProgress(string itemName)
    {
        return _itemProgressDictionary[itemName].Value;
    }
}
