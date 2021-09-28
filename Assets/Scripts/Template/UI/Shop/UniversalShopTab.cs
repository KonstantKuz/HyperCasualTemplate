using System;
using System.Linq;
using Template.ProgressiveItemsHandler;
using UnityEngine;
using UnityEngine.UI;
using Template.Ads;

namespace Template.UI.Shop
{
    public class UniversalShopTab : ShopTab
    {
        [SerializeField] private protected bool _isItemsEquipable;
        [SerializeField] private protected bool _equipOnSelect;
        [SerializeField] private protected Button _equipButton;
        [SerializeField] private protected BuyButton _buyButton;
        [SerializeField] private protected ItemsProgression _itemsProgression;

        private ShopItemButton _lastClickedButton;
    
        public virtual void Awake()
        {
            foreach (ShopItemButton button in itemButtons)
            {
                button.gameObject.SetActive(false);
            }
        
            for (int i = 0; i < _itemsProgression.itemsQueue.Count; i++)
            {
                ProgressiveItemData itemData = _itemsProgression.itemsQueue[i];
                itemButtons[i].Initialize(ProgressiveItemsHandler.ProgressiveItemsHandler.Instance.ItemsReadOnlyDictionary[itemData.itemName], OnItemClicked);
            }
        
            _buyButton.OnClicked += TryBuyItem;
            _equipButton.onClick.AddListener(OnEquipButtonClicked);
        }

        public override void UpdateItemsStatuses()
        {
            if (_lastClickedButton != null)
            {
                OnItemClicked(_lastClickedButton);
            }
        
            for (int i = 0; i < _itemsProgression.itemsQueue.Count; i++)
            {
                ShopItemButton itemButton = itemButtons[i];

                string progressionName = _itemsProgression.progressionName;
                int unlockLevel = ItemUnlockLevel(itemButton.Item, progressionName, i);
            
                itemButton.UpdateStatus(_isItemsEquipable, unlockLevel);
            }
        }

        private int ItemUnlockLevel(ProgressiveItemContainer item, string progressionName, int itemIndex)
        {
            int unlockLevel = 0;
            bool isProgressionUpdatesParallel = ProgressiveItemsHandler.ProgressiveItemsHandler.Instance.ProgressionsReadOnlyDictionary[progressionName].parallelUpdate;
            if (isProgressionUpdatesParallel)
            {
                int progressCorrection = item.CurrentUnlockProgress() - LevelManager.LevelManager.Instance.CurrentDisplayLevelNumber;
                unlockLevel = item.ProgressToUnlock() - progressCorrection;
            }
            else
            {
                unlockLevel = 1 + _itemsProgression.itemsQueue.Take(itemIndex + 1).
                                  Sum(itemData => (itemData.progressToUnlock - ProgressiveItemsHandler.ProgressiveItemsHandler.Instance.ItemsReadOnlyDictionary[itemData.itemName].ForcedProgress()));
            }

            return unlockLevel;
        }

        private void OnItemClicked(ShopItemButton itemButton)
        {
            _lastClickedButton = itemButton;
        
            _buyButton.gameObject.SetActive(false);
            _equipButton.gameObject.SetActive(false);

            OnItemSelected(itemButton.name);
        
            if (itemButton.Item.IsUnlockedToShop())
            {
                SetItemAsViewed(itemButton);
            
                if (itemButton.Item.IsUnlockedToUse())
                {
                    ShowEquipButtonOrEquip(itemButton.name);
                }
                else
                {
                    ShowBuyButton(itemButton.name);
                }
            }
        }

        private bool IsItemEquipped(string itemName)
        {
            return ProgressiveItemsHandler.ProgressiveItemsHandler.Instance.ItemsReadOnlyDictionary[itemName].IsEquipped();
        }

        public virtual void OnItemSelected(string itemName)
        {
            print($"OnItemSelected {itemName}");
            throw new NotImplementedException();
        }

        private void ShowEquipButtonOrEquip(string itemName)
        {
            if (IsItemEquipped(itemName))
            {
                return;
            }
        
            if (_equipOnSelect)
            {
                OnEquipItem(itemName);
            }
            else
            {
                _equipButton.gameObject.SetActive(true);
            }
        }

        private void OnEquipButtonClicked()
        {
            _equipButton.gameObject.SetActive(false);
            OnEquipItem(_lastClickedButton.name);
            UpdateItemsStatuses();
        }

        public virtual void OnEquipItem(string itemName)
        {
            print($"OnEquipItem {itemName}");
            throw new NotImplementedException();
        }

        private void ShowBuyButton(string itemName)
        {
            ProgressiveItemContainer item = ProgressiveItemsHandler.ProgressiveItemsHandler.Instance.ItemsReadOnlyDictionary[itemName];

            switch (item.PriceType())
            {
                case ItemPriceType.Video:
                    _buyButton.ShowButtonWithVideoCost();
                    break;
                case ItemPriceType.Coins:
                    _buyButton.ShowButtonWithCoinsCost(item.Price());
                    break;
            }
        }

        private void TryBuyItem()
        {
            switch (_lastClickedButton.Item.PriceType())
            {
                case ItemPriceType.Video:
                    TryBuyForVideo(_lastClickedButton.name);
                    break;
                case ItemPriceType.Coins:
                    TryBuyForCoins(_lastClickedButton.name);
                    break;
            }
        }

        private void TryBuyForVideo(string itemName)
        {
            AdsManager.Instance.onRewardedAdRewarded += delegate
            {
                ProgressiveItemsHandler.ProgressiveItemsHandler.Instance.ItemsReadOnlyDictionary[itemName].UnlockToUse();

                OnBuyItem(itemName);
                UpdateItemsStatuses();
            };
    
            AdsManager.Instance.ShowRewardedAd($"UnlockItem{itemName}");
        }
    
        private void TryBuyForCoins(string itemName)
        {
            int itemPrice = ProgressiveItemsHandler.ProgressiveItemsHandler.Instance.ItemsReadOnlyDictionary[itemName].Price();
            if (!PlayerWallet.Instance.HasMoney(itemPrice))
            {
                return;
            }
        
            PlayerWallet.Instance.DecreaseMoney(itemPrice);
            ProgressiveItemsHandler.ProgressiveItemsHandler.Instance.ItemsReadOnlyDictionary[itemName].UnlockToUse();
            
            OnBuyItem(itemName);
            UpdateItemsStatuses();
        }

        public virtual void OnBuyItem(string itemName)
        {
            print($"OnBuyItem {itemName}");
            throw new NotImplementedException();
        }

        private void SetItemAsViewed(ShopItemButton itemButton)
        {
            itemButton.SetAsNew(false);
            itemButton.Item.SetAsViewedInShop();
        }

        public override void OnShopClosed()
        {
            itemButtons.ForEach(itemButton => itemButton.SetSelected(false));
        }

        public override bool HasNewNotViewedInShopItems()
        {
            return _itemsProgression.itemsQueue.Any(itemData => 
                ProgressiveItemsHandler.ProgressiveItemsHandler.Instance.ItemsReadOnlyDictionary[itemData.itemName].IsNewNotViewedInShop());
        }
    }
}
