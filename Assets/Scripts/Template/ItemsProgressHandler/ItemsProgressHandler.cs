using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsProgressHandler : Singleton<ItemsProgressHandler>
{
    [SerializeField] private ItemsProgression[] progressions;

    private Dictionary<string, ItemsProgression> progressionsDictionary;
    private Dictionary<string, ProgressiveItemData> itemsDictionary;
    private Dictionary<string, PlayerPrefsProperty<int>> itemProgressDictionary;
    private Dictionary<string, PlayerPrefsProperty<bool>> itemStatusDictionary;

    private const string PrefsPrefixItemProgress = "Progress";
    private const string PrefsPrefixItemShopStatus = "AvailableToShop";
    private const string PrefsPrefixItemUseStatus = "AvailableToUse";
    
    private void Awake()
    {
        InitializeItemsDictionary();
    }

    private void InitializeItemsDictionary()
    {
        progressionsDictionary= new Dictionary<string, ItemsProgression>();
        itemsDictionary = new Dictionary<string, ProgressiveItemData>();
        itemProgressDictionary = new Dictionary<string, PlayerPrefsProperty<int>>();
        itemStatusDictionary = new Dictionary<string, PlayerPrefsProperty<bool>>();
        
        for (int i = 0; i < progressions.Length; i++)
        {
            progressionsDictionary.Add(progressions[i].progressionName, progressions[i]);
            
            for (int j = 0; j < progressions[i].itemsQueue.Length; j++)
            {
                string itemName = progressions[i].itemsQueue[j].itemName;
                
                itemsDictionary.Add(itemName, progressions[i].itemsQueue[j]);
                itemProgressDictionary.Add(itemName, new PlayerPrefsProperty<int>(PrefsPrefixItemProgress + itemName, 0));

                string shopStatusKey = PrefsPrefixItemShopStatus + itemName;
                itemStatusDictionary.Add(shopStatusKey, new PlayerPrefsProperty<bool>(shopStatusKey, false));

                string useStatusKey = PrefsPrefixItemUseStatus + itemName;
                itemStatusDictionary.Add(useStatusKey, new PlayerPrefsProperty<bool>(useStatusKey, false));
            }
        }
    }
    
    public void UpdateItemProgress(string itemName)
    {
        itemProgressDictionary[itemName].Value++;
    }
    public bool IsItemProgressPassed(string itemName)
    {
        return itemProgressDictionary[itemName].Value >= itemsDictionary[itemName].levelsCountToGet;
    }
    
    public void UnlockItemToShop(string itemName)
    {
        string shopStatusKey = PrefsPrefixItemShopStatus + itemName;
        itemStatusDictionary[shopStatusKey].Value = true;
    }
    public bool IsItemUnlockedToShop(string itemName)
    {
        string shopStatusKey = PrefsPrefixItemShopStatus + itemName;
        return itemStatusDictionary[shopStatusKey].Value;
    }
    public bool IsAllItemsUnlockedToShop(string progressionName)
    {
        ItemsProgression progression = progressionsDictionary[progressionName];
        for (int i = 0; i < progression.itemsQueue.Length; i++)
        {
            if (!IsItemUnlockedToShop(progression.itemsQueue[i].itemName))
            {
                return false;
            }
        }
        return true;
    }

    public void UnlockItemToUse(string itemName)
    {
        string useStatusKey = PrefsPrefixItemUseStatus + itemName;
        itemStatusDictionary[useStatusKey].Value = true;
    }
    public bool IsItemUnlockedToUse(string itemName)
    {
        string useStatusKey = PrefsPrefixItemUseStatus + itemName;
        return itemStatusDictionary[useStatusKey].Value;
    }
    public bool IsAllItemsUnlockedToUse(string progressionName)
    {
        ItemsProgression progression = progressionsDictionary[progressionName];
        for (int i = 0; i < progression.itemsQueue.Length; i++)
        {
            if (!IsItemUnlockedToUse(progression.itemsQueue[i].itemName))
            {
                return false;
            }
        }
        return true;
    }

    public int ItemCost(string itemName)
    {
        return itemsDictionary[itemName].cost;
    }
    
    public int ItemLevelsCountToGet(string itemName)
    {
        return itemsDictionary[itemName].levelsCountToGet;
    }
    
    public Sprite ItemIcon(string itemName)
    {
        return itemsDictionary[itemName].icon;
    }

    public int ItemCurrentProgress(string itemName)
    {
        return itemProgressDictionary[itemName].Value;
    }
}
