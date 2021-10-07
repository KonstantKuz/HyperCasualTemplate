using System;
using System.Linq;
using Templates.Ads;
using Templates.ItemSystems.InventorySystem;
using UnityEngine;
using UnityEngine.UI;

namespace Templates.ItemSystems.ShopSystem.UI
{
    public class UniversalShopTab : ShopTab
    {
        [SerializeField] private protected bool _isItemsEquipable;
        [SerializeField] private protected bool _equipOnSelect;
        [SerializeField] private protected Button _equipButton;
        [SerializeField] private protected BuyButton _buyButton;
        [SerializeField] private protected ShopItemsGroup _shopItemsGroup;

        private ShopItemButton _lastClickedButton;
    
        public override void Initialize()
        {
            base.Initialize();

            foreach (ShopItemButton button in itemButtons)
            {
                button.gameObject.SetActive(false);
            }
        
            for (int i = 0; i < _shopItemsGroup.Items.Count; i++)
            {
                ShopItemData itemData = _shopItemsGroup.Items[i];
                itemButtons[i].Initialize(itemData, OnItemClicked);
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
        
            for (int i = 0; i < _shopItemsGroup.Items.Count; i++)
            {
                ShopItemButton itemButton = itemButtons[i];
                itemButton.UpdateStatus(_isItemsEquipable);
            }
        }

        private void OnItemClicked(ShopItemButton itemButton)
        {
            _lastClickedButton = itemButton;
        
            _buyButton.gameObject.SetActive(false);
            _equipButton.gameObject.SetActive(false);

            OnItemSelected(itemButton.name);

            if (!itemButton.IsUnlockedToShop)
            {
                return;
            }

            Shop.Instance.GetItem(itemButton.name).MarkAsViewedInShop();

            if (itemButton.IsUnlockedToUse)
            {
                ShowEquipButtonOrEquip(itemButton.name);
            }
            else
            {
                ShowBuyButton(itemButton.name);
            }
            
            itemButton.UpdateStatus(_isItemsEquipable);
        }

        private bool IsItemEquipped(string itemName)
        {
            return Inventory.Instance.GetItem(itemName).IsEquipped;
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
            ShopItem shopItem = Shop.Instance.GetItem(itemName);
            _buyButton.Show(shopItem.CurrencyType, shopItem.PriceAmount);
        }

        private void TryBuyItem()
        {
            ShopItem shopItem = Shop.Instance.GetItem(_lastClickedButton.name);

            switch (shopItem.CurrencyType)
            {
                case CurrencyType.Coin:
                    TryBuyForCoins(_lastClickedButton.name);
                    break;
                case CurrencyType.Video:
                    TryBuyForVideo(_lastClickedButton.name);
                    break;
            }
        }

        private void TryBuyForCoins(string itemName)
        {
            ShopItem shopItem = Shop.Instance.GetItem(itemName);
            if (!PlayerWallet.Instance.HasCurrency(shopItem.CurrencyType.ToString(), shopItem.PriceAmount))
            {
                return;
            }
        
            PlayerWallet.Instance.DecreaseCurrency(shopItem.CurrencyType.ToString(), shopItem.PriceAmount);
            Inventory.Instance.GetItem(itemName).UnlockToUse();
            
            OnBuyItem(itemName);
            UpdateItemsStatuses();
        }

        private void TryBuyForVideo(string itemName)
        {
            AdsManager.Instance.onRewardedAdRewarded += delegate
            {
                TryBuyItem(itemName);
                UpdateItemsStatuses();
            };
    
            AdsManager.Instance.ShowRewardedAd($"UnlockItem{itemName}");
        }

        private void TryBuyItem(string itemName)
        {
            ShopItem shopItem = Shop.Instance.GetItem(itemName);
            shopItem.SetPriceAmount(shopItem.PriceAmount - 1);
            
            if (shopItem.PriceAmount <= 0)
            {
                Inventory.Instance.GetItem(itemName).UnlockToUse();
                OnBuyItem(itemName);
            }
        }

        public virtual void OnBuyItem(string itemName)
        {
            print($"OnBuyItem {itemName}");
            throw new NotImplementedException();
        }

        public override void OnShopClosed()
        {
            itemButtons.ForEach(itemButton => itemButton.SetSelected(false));
        }

        public override bool HasFreshItems()
        {
            return _shopItemsGroup.Items.Any(itemData => 
                Shop.Instance.GetItem(itemData.InventoryData.Name).IsFreshInShop);
        }
    }
}
