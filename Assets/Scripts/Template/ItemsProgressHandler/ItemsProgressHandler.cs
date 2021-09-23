using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemsProgressHandler : Singleton<ItemsProgressHandler>
{
    [SerializeField] private ItemsProgression[] _progressions;

    private Dictionary<string, ItemsProgression> _progressionsDictionary;
    private Dictionary<string, ProgressiveItemContainer> _itemsDictionary;

    public Dictionary<string, ItemsProgression> ProgressionsDictionary => _progressionsDictionary;
    public Dictionary<string, ProgressiveItemContainer> ItemsDictionary => _itemsDictionary;
    
    private void Awake()
    {
        InitializeItemsDictionary();
        Observer.Instance.OnLoadNextScene += IncreaseAllItemsProgress;
    }

    private void InitializeItemsDictionary()
    {
        _progressionsDictionary= new Dictionary<string, ItemsProgression>();
        _itemsDictionary = new Dictionary<string, ProgressiveItemContainer>();
        
        foreach (ItemsProgression progression in _progressions)
        {
            _progressionsDictionary.Add(progression._progressionName, progression);
            foreach (ProgressiveItemData itemData in progression._itemsQueue)
            {
                ProgressiveItemContainer progressiveItemContainer = new ProgressiveItemContainer(itemData);
                _itemsDictionary.Add(itemData._itemName, progressiveItemContainer);
            }
        }
    }

    private void IncreaseAllItemsProgress()
    {
        foreach (var progression in _progressions)
        {
            if (progression._manualUpdate)
            {
                return;
            }

            IncreaseItemsProgress(progression);
        }
    }

    public void IncreaseItemsProgress(ItemsProgression progression)
    {
        foreach (var itemData in progression._itemsQueue)
        {
            string itemName = itemData._itemName;
            
            if (_itemsDictionary[itemName].IsUnlockedToShop())
            {
                continue;
            }

            _itemsDictionary[itemName].IncreaseProgress();
            _itemsDictionary[itemName].TryUnlock();

            if (!progression._parallelUpdate)
            {
                break;
            }
        }
    }
    
    public bool IsAllItemsUnlockedToShop(string progressionName)
    {
        ProgressiveItemData[] progression = _progressionsDictionary[progressionName]._itemsQueue;
        return progression.Any(itemData => !ItemsDictionary[itemData._itemName].IsUnlockedToShop());
    }

    public bool IsAllItemsUnlockedToUse(string progressionName)
    {
        ProgressiveItemData[] progression = _progressionsDictionary[progressionName]._itemsQueue;
        return progression.Any(itemData => !ItemsDictionary[itemData._itemName].IsUnlockedToUse());
    }
}
