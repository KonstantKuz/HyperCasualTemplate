using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

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
            itemButtons[i].Initialize(_itemsProgression.itemsQueue[i]);
            itemButtons[i].OnClicked += UpdateBuyOrEquipButtonStatusFor;
        }
        
        _buyButton.OnClicked += TryBuyItem;
        _equipButton.onClick.AddListener(OnEquipButtonClicked);
    }

    public override void UpdateItemsStatuses()
    {
        if (_lastClickedButton != null)
        {
            UpdateBuyOrEquipButtonStatusFor(_lastClickedButton);
        }
        
        for (int i = 0; i < _itemsProgression.itemsQueue.Count; i++)
        {
            ShopItemButton itemButton = itemButtons[i];
            ProgressiveItemContainer item = ProgressiveItemsHandler.Instance.ItemsDictionary[itemButton.name];

            string progressionName = _itemsProgression.progressionName;
            int unlocksAtLevel = ItemUnlocksAtLevel(item, progressionName, i);
            
            UpdateItemButtonStatus(itemButton, item, unlocksAtLevel);
        }
    }

    private int ItemUnlocksAtLevel(ProgressiveItemContainer item, string progressionName, int itemIndex)
    {
        int unlocksAtLevel = 0;
        bool isProgressionUpdatesParallel = ProgressiveItemsHandler.Instance.ProgressionsDictionary[progressionName].parallelUpdate;
        if (isProgressionUpdatesParallel)
        {
            int progressCorrection = item.CurrentUnlockProgress() - LevelManager.Instance.CurrentDisplayLevelNumber;
            unlocksAtLevel = item.ProgressToUnlock() - progressCorrection;
        }
        else
        {
            unlocksAtLevel = 1 + _itemsProgression.itemsQueue.Take(itemIndex + 1).
                                 Sum(itemData => (itemData.progressToUnlock - ProgressiveItemsHandler.Instance.ItemsDictionary[itemData.itemName].ForcedProgress()));
        }

        return unlocksAtLevel;
    }

    private void UpdateBuyOrEquipButtonStatusFor(ShopItemButton itemButton)
    {
        _lastClickedButton = itemButton;
        
        _buyButton.gameObject.SetActive(false);
        _equipButton.gameObject.SetActive(false);

        OnItemSelected(itemButton.name);
        
        if (ProgressiveItemsHandler.Instance.ItemsDictionary[itemButton.name].IsUnlockedToShop())
        {
            SetItemAsViewed(itemButton);
            
            if (ProgressiveItemsHandler.Instance.ItemsDictionary[itemButton.name].IsUnlockedToUse())
            {
                ShowEquipButtonOrEquip(itemButton.name);
            }
            else
            {
                ShowBuyButton(itemButton.name);
            }
        }
    }

    private void UpdateItemButtonStatus(ShopItemButton itemButton, ProgressiveItemContainer item, int unlocksAtLevel)
    {
        if (_isItemsEquipable)
        {
            itemButton.SetEquipped(IsItemEquipped(itemButton.name));
        }

        if (!item.IsUnlockedToUseByDefault())
        {
            itemButton.SetAsNew(item.IsNewNotViewedInShop());
        }

        itemButton.UpdateItemAvailableStatus(item, unlocksAtLevel);
    }

    private bool IsItemEquipped(string itemName)
    {
        return ProgressiveItemsHandler.Instance.ItemsDictionary[itemName].IsEquipped();
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

    private void ShowBuyButton(string clickedItem)
    {
        ProgressiveItemContainer item = ProgressiveItemsHandler.Instance.ItemsDictionary[clickedItem];

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
        ProgressiveItemContainer item = ProgressiveItemsHandler.Instance.ItemsDictionary[_lastClickedButton.name];
        
        switch (item.PriceType())
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
            ProgressiveItemsHandler.Instance.ItemsDictionary[itemName].UnlockToUse();

            OnBuyItem(itemName);
            UpdateItemsStatuses();
        };
    
        AdsManager.Instance.ShowRewardedAd($"UnlockItem{itemName}");
    }
    
    private void TryBuyForCoins(string itemName)
    {
        int itemPrice = ProgressiveItemsHandler.Instance.ItemsDictionary[itemName].Price();
        if (!PlayerWallet.Instance.HasMoney(itemPrice))
        {
            return;
        }
        
        PlayerWallet.Instance.DecreaseMoney(itemPrice);
        ProgressiveItemsHandler.Instance.ItemsDictionary[itemName].UnlockToUse();
            
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
        ProgressiveItemsHandler.Instance.ItemsDictionary[itemButton.name].SetAsViewedInShop();
    }

    public override void OnShopClosed()
    {
        itemButtons.ForEach(itemButton => itemButton.SetSelected(false));
    }

    public override bool HasNewNotViewedInShopItems()
    {
        return _itemsProgression.itemsQueue.Any(itemData => 
            ProgressiveItemsHandler.Instance.ItemsDictionary[itemData.itemName].IsNewNotViewedInShop());
    }
}
