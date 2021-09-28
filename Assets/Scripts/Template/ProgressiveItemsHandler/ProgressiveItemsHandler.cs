using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Template.ProgressiveItemsHandler
{
    public class ProgressiveItemsHandler : Singleton<ProgressiveItemsHandler>
    {
        [SerializeField] private ItemsProgression[] _progressions;

        private Dictionary<string, ItemsProgression> _progressionsDictionary;
        private Dictionary<string, ProgressiveItemContainer> _itemsDictionary;

        public Dictionary<string, ProgressiveItemContainer> ItemsReadOnlyDictionary => _itemsDictionary;
        public Dictionary<string, ItemsProgression> ProgressionsReadOnlyDictionary => _progressionsDictionary;

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
                _progressionsDictionary.Add(progression.progressionName, progression);
                foreach (ProgressiveItemData itemData in progression.itemsQueue)
                {
                    ProgressiveItemContainer progressiveItemContainer = new ProgressiveItemContainer(progression.progressionName, itemData, ResetProgressionsEquipStatus);
                    _itemsDictionary.Add(itemData.itemName, progressiveItemContainer);
                }
            }
        }

        private void ResetProgressionsEquipStatus(EquippedItemData equippedItemData)
        {
            List<ProgressiveItemData> progression = _progressionsDictionary[equippedItemData.progressionName].itemsQueue;
            foreach (var itemData in progression.Where(itemData => itemData.itemName != equippedItemData.itemName))
            {
                _itemsDictionary[itemData.itemName].ResetEquipStatus();
            }
        }
    
        private void IncreaseAllItemsProgress()
        {
            foreach (var progression in _progressions)
            {
                if (progression.manualUpdate)
                {
                    continue;
                }

                IncreaseItemsProgress(progression, progression.parallelUpdate, false);
            }
        }

        public void IncreaseItemsProgress(ItemsProgression progression, bool parallel, bool forced)
        {
            foreach (var itemData in progression.itemsQueue)
            {
                ProgressiveItemContainer item = _itemsDictionary[itemData.itemName];
                if (item.IsUnlockedToShop())
                {
                    continue;
                }

                if (forced)
                {
                    item.IncreaseForcedProgress();
                }
            
                item.IncreaseProgress();
                item.TryUnlock();

                if (!parallel)
                {
                    break;
                }
            }
        }

        public bool IsAllItemsUnlockedToShop(string progressionName)
        {
            List<ProgressiveItemData> progression = _progressionsDictionary[progressionName].itemsQueue;
            return progression.All(itemData => ItemsReadOnlyDictionary[itemData.itemName].IsUnlockedToShop());
        }

        public bool IsAllItemsUnlockedToUse(string progressionName)
        {
            List<ProgressiveItemData> progression = _progressionsDictionary[progressionName].itemsQueue;
            return progression.All(itemData => ItemsReadOnlyDictionary[itemData.itemName].IsUnlockedToUse());
        }
    }
}
