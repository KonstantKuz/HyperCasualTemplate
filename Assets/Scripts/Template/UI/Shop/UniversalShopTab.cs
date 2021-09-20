using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public enum ItemCostType
{
    Video,
    Coins,
}

public class UniversalShopTab : ShopTab
{
    [SerializeField] private protected bool _isItemsEquipable;
    [SerializeField] private protected bool _equipOnSelect;
    [SerializeField] private protected Button _equipButton;
    [SerializeField] private protected ItemCostType _itemCostType;
    [SerializeField] private protected BuyButton _buyButton;
    [SerializeField] private protected ItemsProgression _itemsProgression;

    private ShopItemButton _lastClickedButton;
    
    private void Awake()
    {
        foreach (ShopItemButton button in itemButtons)
        {
            button.gameObject.SetActive(false);
        }
        
        for (int i = 0; i < _itemsProgression.itemsQueue.Length; i++)
        {
            itemButtons[i].gameObject.SetActive(true);
            itemButtons[i].gameObject.name = _itemsProgression.itemsQueue[i]._itemName;
            
            itemButtons[i].SetItemSprite(_itemsProgression.itemsQueue[i]._icon);
            itemButtons[i].OnClicked += UpdateBuyOrEquipButtonStatusFor;
        }
        
        UpdateItemsStatuses();
    }

    public override void UpdateItemsStatuses()
    {
        if (_lastClickedButton != null)
        {
            UpdateBuyOrEquipButtonStatusFor(_lastClickedButton);
        }
        
        int givenAtLevel = 0;

        for (int i = 0; i < _itemsProgression.itemsQueue.Length; i++)
        {
            ShopItemButton itemButton = itemButtons[i];
            string itemName = _itemsProgression.itemsQueue[i]._itemName;
            givenAtLevel += ItemsProgressHandler.Instance.ItemLevelsCountToGet(itemName);

            if (!_itemsProgression.itemsQueue[i]._unlockedByDefault)
            {
                bool isItemNewUnchecked = IsItemNewUnchecked(itemName);
                itemButton.SetAsNewUncheckedItem(isItemNewUnchecked);
            }
            
            if (_isItemsEquipable)
            {
                bool isItemEquipped = IsItemEquipped(itemName);
                itemButton.SetEquipped(isItemEquipped);
            }
            
            if (ItemsProgressHandler.Instance.IsItemUnlockedToUse(itemName))
            {
                itemButton.SetItemAvailable();
                continue;
            }
            if (ItemsProgressHandler.Instance.IsItemUnlockedToShop(itemName))
            {
                SetItemAvailable(itemButton);
                continue;
            }
            itemButton.SetItemAvailableForLevel(givenAtLevel + 1);
        }
    }

    private bool IsItemNewUnchecked(string itemName)
    {
        return ItemsProgressHandler.Instance.IsItemUnlockedToShop(itemName) &&
               !ShopItemFreshnessCheck.IsItemCheckedInShop(itemName);
    }

    public virtual bool IsItemEquipped(string itemName)
    {
        throw new NotImplementedException();
    }
    
    private void SetItemAvailable(ShopItemButton itemButton)
    {
        switch (_itemCostType)
        {
            case ItemCostType.Video:
                itemButton.SetItemAvailableForVideo();
                break;
            case ItemCostType.Coins:
                int itemCost = ItemsProgressHandler.Instance.ItemCost(itemButton.name);
                bool canBeBought = PlayerWallet.Instance.HasMoney(itemCost);
                itemButton.SetItemAvailableForCoins(itemCost, canBeBought);
                break;
        }
    }

    private void UpdateBuyOrEquipButtonStatusFor(ShopItemButton itemButton)
    {
        _lastClickedButton = itemButton;
        
        _buyButton.gameObject.SetActive(false);
        _equipButton.gameObject.SetActive(false);

        OnItemSelected(itemButton.name);
        
        if (ItemsProgressHandler.Instance.IsItemUnlockedToShop(itemButton.name))
        {
            SetItemAsChecked(itemButton);
            
            if (ItemsProgressHandler.Instance.IsItemUnlockedToUse(itemButton.name))
            {
                SetItemAsChecked(itemButton);
                ShowEquipButtonOrEquip(itemButton.name);
            }
            else
            {
                ShowBuyButton(itemButton.name);
            }
        }
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
            ShowEquipButton(itemName);
        }
    }

    private void ShowEquipButton(string itemName)
    {
        _equipButton.onClick.RemoveAllListeners();
        _equipButton.onClick.AddListener(delegate
        {
            _equipButton.gameObject.SetActive(false);
            OnEquipItem(itemName);
            UpdateItemsStatuses();
        });
        _equipButton.gameObject.SetActive(true);
    }

    public virtual void OnEquipItem(string itemName)
    {
        print($"OnEquipItem {itemName}");
        throw new NotImplementedException();
    }

    private void ShowBuyButton(string clickedItem)
    {
        switch (_itemCostType)
        {
            case ItemCostType.Video:
                _buyButton.ShowButtonWithVideoCost(delegate { TryBuyItem(clickedItem); });
                break;
            case ItemCostType.Coins:
                int itemCost = ItemsProgressHandler.Instance.ItemCost(clickedItem);
                bool canBeBought = PlayerWallet.Instance.HasMoney(itemCost);
                _buyButton.ShowButtonWithCoinsCost(delegate { TryBuyItem(clickedItem); }, itemCost, canBeBought);
                break;
        }
    }

    private void TryBuyItem(string itemName)
    {
        switch (_itemCostType)
        {
            case ItemCostType.Video:
                TryBuyForVideo(itemName);
                break;
            case ItemCostType.Coins:
                TryBuyForCoins(itemName);
                break;
        }
    }

    private void TryBuyForVideo(string itemName)
    {
        ADManager.Instance.onRewardedAdRewarded += delegate
        {
            ItemsProgressHandler.Instance.UnlockItemToUse(itemName);

            OnBuyItem(itemName);
            UpdateItemsStatuses();
        };
    
        ADManager.Instance.ShowRewardedAd($"UnlockItem{itemName}");
    }
    
    private void TryBuyForCoins(string itemName)
    {
        int itemCost = ItemsProgressHandler.Instance.ItemCost(itemName);
        if (PlayerWallet.Instance.HasMoney(itemCost))
        {
            PlayerWallet.Instance.DecreaseMoney(itemCost);
            ItemsProgressHandler.Instance.UnlockItemToUse(itemName);
            
            OnBuyItem(itemName);
            UpdateItemsStatuses();
        }
    }

    public virtual void OnBuyItem(string itemName)
    {
        print($"OnBuyItem {itemName}");
        throw new NotImplementedException();
    }

    private void SetItemAsChecked(ShopItemButton itemButton)
    {
        itemButton.SetAsNewUncheckedItem(false);
        ShopItemFreshnessCheck.SetItemCheckedInShop(itemButton.name);
    }
    
    public override bool HasNewUncheckedItem()
    {
        for (int i = 1; i < _itemsProgression.itemsQueue.Length; i++)
        {
            string itemName = _itemsProgression.itemsQueue[i]._itemName;
            
            if (IsItemNewUnchecked(itemName))
            {
                return true;
            }
        }

        return false;
    }
}
