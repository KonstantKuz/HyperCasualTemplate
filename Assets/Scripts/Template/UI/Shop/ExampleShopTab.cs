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

public class ExampleShopTab : ShopTab
{
    [SerializeField] private protected bool _isItemsEquipable;
    [SerializeField] private protected bool _equipOnSelect;
    [SerializeField] private protected Button _equipButton;
    [SerializeField] private protected ItemCostType _itemCostType;
    [SerializeField] private protected BuyButton _buyButton;
    [SerializeField] private protected ItemsProgression _itemsProgression;

    private void Awake()
    {
        foreach (ShopItemButton button in itemButtons)
        {
            button.gameObject.SetActive(false);
        }
        
        for (int i = 0; i < _itemsProgression.itemsQueue.Length; i++)
        {
            itemButtons[i].gameObject.SetActive(true);
            
            itemButtons[i].SetItemSprite(_itemsProgression.itemsQueue[i].icon);
            itemButtons[i].OnClicked += TryShowBuyButton;
        }
        
        UpdateItemsStatuses();
    }

    public override void UpdateItemsStatuses()
    {
        int givenAtLevel = 0;

        for (int i = 0; i < _itemsProgression.itemsQueue.Length; i++)
        {
            string itemName = _itemsProgression.itemsQueue[i].itemName;
            givenAtLevel += ItemsProgressHandler.Instance.ItemLevelsCountToGet(itemName);
            
            if (_isItemsEquipable)
            {
                bool isItemEquiped = IsItemEquiped(itemName);
                itemButtons[i].SetEquiped(isItemEquiped);
            }
            
            if (ItemsProgressHandler.Instance.IsItemUnlockedToUse(itemName))
            {
                itemButtons[i].SetItemAvailable();
                continue;
            }

            if (ItemsProgressHandler.Instance.IsItemUnlockedToShop(itemName))
            {
                bool isItemNotChecked = !ShopItemFreshnessCheck.IsItemCheckedInShop(itemName);
                itemButtons[i].SetAsNewUncheckedItem(isItemNotChecked);

                switch (_itemCostType)
                {
                    case ItemCostType.Video:
                        itemButtons[i].SetItemAvailableForVideo();
                        break;
                    case ItemCostType.Coins:
                        int itemCost = ItemsProgressHandler.Instance.ItemCost(itemName);
                        bool canBeBought = PlayerWallet.Instance.HasMoney(itemCost);
                        itemButtons[i].SetItemAvailableForCoins(itemCost, canBeBought);
                        break;
                }
                continue;
            }
            
            itemButtons[i].SetItemAvailableForLevel(givenAtLevel + 1);
        }
    }

    private bool IsItemEquiped(string itemName)
    {
        throw new NotImplementedException();
    }

    private void EquipItem()
    {
        throw new NotImplementedException();
    }
    
    private void BuyItem()
    {
        throw new NotImplementedException();
    }

    private void ChangeItemView()
    {
        throw new NotImplementedException();
    }

    private void TryShowBuyButton(ShopItemButton itemButton)
    {
        _buyButton.gameObject.SetActive(false);
        string _clickedItem = "";
        
        for (int i = 0; i < itemButtons.Length; i++)
        {
            if (itemButtons[i] != itemButton)
            {
                continue;
            }
            
            _clickedItem = _itemsProgression.itemsQueue[i].itemName;
        }

        if (ItemsProgressHandler.Instance.IsItemUnlockedToUse(_clickedItem))
        {
            if (_equipOnSelect)
            {
                EquipItem();
            }
            else
            {
                _equipButton.onClick.RemoveAllListeners();
                _equipButton.onClick.AddListener(EquipItem); 
            }
            return;
        }
        
        ChangeItemView();
            
        if (ItemsProgressHandler.Instance.IsItemUnlockedToShop(_clickedItem))
        {
            SetItemAsChecked(itemButton, _clickedItem);

            switch (_itemCostType)
            {
                case ItemCostType.Video:
                    _buyButton.ShowButtonWithVideoCost(delegate { TryBuyItem(_clickedItem); });
                    break;
                case ItemCostType.Coins:
                    int itemCost = ItemsProgressHandler.Instance.ItemCost(_clickedItem);
                    bool canBeBought = PlayerWallet.Instance.HasMoney(itemCost);
                    _buyButton.ShowButtonWithCoinsCost(delegate { TryBuyItem(_clickedItem); }, itemCost, canBeBought);
                    break;
            }
        }
    }

    private void TryBuyItem(string item)
    {
        switch (_itemCostType)
        {
            case ItemCostType.Video:
                TryBuyForVideo(item);
                break;
            case ItemCostType.Coins:
                TryBuyForCoins(item);
                break;
        }
    }

    private void TryBuyForVideo(string item)
    {
        ADManager.Instance.onRewardedAdRewarded += delegate
        {
            ItemsProgressHandler.Instance.UnlockItemToUse(item);
            UpdateItemsStatuses();
        };
    
        // ADManager.Instance.ShowRewardedAd(RewardedVideoPlacement.Shop_Element);
    }
    
    private void TryBuyForCoins(string item)
    {
        int itemCost = ItemsProgressHandler.Instance.ItemCost(item);
        if (PlayerWallet.Instance.HasMoney(itemCost))
        {
            BuyItem();
        }
    }

    public override void OnShopClosed()
    {
    }
    
    private void SetItemAsChecked(ShopItemButton itemButton, string itemName)
    {
        itemButton.SetAsNewUncheckedItem(false);
        ShopItemFreshnessCheck.SetItemCheckedInShop(itemName);
    }
    
    public override bool HasNewUncheckedItem()
    {
        for (int i = 1; i < itemButtons.Length; i++)
        {
            string itemName = _itemsProgression.itemsQueue[i].itemName;
            if (ItemsProgressHandler.Instance.IsItemUnlockedToUse(itemName) &&
                !ShopItemFreshnessCheck.IsItemCheckedInShop(itemName))
            {
                return true;
            }
        }

        return false;
    }
}
