using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        
        for (int i = 0; i < _itemsProgression._itemsQueue.Length; i++)
        {
            itemButtons[i].Initialize(_itemsProgression._itemsQueue[i]);
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

        Dictionary<string, ProgressiveItemContainer> itemsDictionary = ItemsProgressHandler.Instance.ItemsDictionary;
        
        int givenAtLevel = 0;
        for (int i = 0; i < _itemsProgression._itemsQueue.Length; i++)
        {
            string itemName = _itemsProgression._itemsQueue[i]._itemName;
            ProgressiveItemContainer item = itemsDictionary[itemName];
            ShopItemButton itemButton = itemButtons[i];

            if (ItemsProgressHandler.Instance.ProgressionsDictionary[_itemsProgression._progressionName]._parallelUpdate)
            {
                givenAtLevel = item.ProgressToUnlock() - 1;
            }
            else
            {
                // TODO : нужно обработать ситуацию когда прогресс повышен вне зависимости от прохождения уровней
                // например если прогресс повышен за просмотр ревард видео 
                givenAtLevel += item.ProgressToUnlock();  
            }
            
            if (_isItemsEquipable)
            {
                itemButton.SetEquipped(IsItemEquipped(itemName));
            }
            
            if (!item.IsUnlockedByDefault())
            {
                itemButton.SetAsNew(item.IsNewNotViewedInShop());
            }

            if (item.IsUnlockedToUse())
            {
                itemButton.SetItemAvailable();
                continue;
            }
            
            if (item.IsUnlockedToShop())
            {
                switch (_itemCostType)
                {
                    case ItemCostType.Video:
                        itemButton.SetItemAvailableForVideo();
                        break;
                    case ItemCostType.Coins:
                        itemButton.SetItemAvailableForCoins(item.Cost(), PlayerWallet.Instance.GetCurrentMoney());
                        break;
                }
                continue;
            }

            itemButton.SetItemAvailableForLevel(givenAtLevel + 1);
        }
    }

    private void UpdateBuyOrEquipButtonStatusFor(ShopItemButton itemButton)
    {
        _lastClickedButton = itemButton;
        
        _buyButton.gameObject.SetActive(false);
        _equipButton.gameObject.SetActive(false);

        OnItemSelected(itemButton.name);
        
        if (ItemsProgressHandler.Instance.ItemsDictionary[itemButton.name].IsUnlockedToShop())
        {
            SetItemAsViewed(itemButton);
            
            if (ItemsProgressHandler.Instance.ItemsDictionary[itemButton.name].IsUnlockedToUse())
            {
                ShowEquipButtonOrEquip(itemButton.name);
            }
            else
            {
                ShowBuyButton(itemButton.name);
            }
        }
    }

    public virtual bool IsItemEquipped(string itemName)
    {
        throw new NotImplementedException();
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
        _equipButton.gameObject.SetActive(true);

        // _equipButton.onClick.RemoveAllListeners();
        _equipButton.onClick.AddListener(delegate
        {
            OnEquipButtonClicked(itemName);
        });
    }

    private void OnEquipButtonClicked(string itemName)
    {
        _equipButton.gameObject.SetActive(false);
        OnEquipItem(itemName);
        UpdateItemsStatuses();      
        
        _equipButton.onClick.RemoveListener(delegate
        {
            OnEquipButtonClicked(itemName);
        });
    }

    public virtual void OnEquipItem(string itemName)
    {
        print($"OnEquipItem {itemName}");
        throw new NotImplementedException();
    }

    private void ShowBuyButton(string clickedItem)
    {
        int itemCost = ItemsProgressHandler.Instance.ItemsDictionary[clickedItem].Cost();
        bool canBeBought = PlayerWallet.Instance.HasMoney(itemCost);

        switch (_itemCostType)
        {
            case ItemCostType.Video:
                _buyButton.ShowButtonWithVideoCost();
                break;
            case ItemCostType.Coins:
                _buyButton.ShowButtonWithCoinsCost(itemCost, canBeBought);
                break;
        }

        _buyButton.SubscribeOnClick(delegate { TryBuyItem(clickedItem); });
    }

    private void TryBuyItem(string itemName)
    {
        _buyButton.UnsubscribeFromClick(delegate { TryBuyItem(itemName); });
        
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
        AdsManager.Instance.onRewardedAdRewarded += delegate
        {
            ItemsProgressHandler.Instance.ItemsDictionary[itemName].UnlockToUse();

            OnBuyItem(itemName);
            UpdateItemsStatuses();
        };
    
        AdsManager.Instance.ShowRewardedAd($"UnlockItem{itemName}");
    }
    
    private void TryBuyForCoins(string itemName)
    {
        int itemCost = ItemsProgressHandler.Instance.ItemsDictionary[itemName].Cost();
        if (!PlayerWallet.Instance.HasMoney(itemCost))
        {
            return;
        }
        
        PlayerWallet.Instance.DecreaseMoney(itemCost);
        ItemsProgressHandler.Instance.ItemsDictionary[itemName].UnlockToUse();
            
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
        ItemsProgressHandler.Instance.ItemsDictionary[itemButton.name].SetAsViewedInShop();
    }
    
    public override bool HasNewNotViewedInShopItems()
    {
        return _itemsProgression._itemsQueue.Any(itemData => 
            ItemsProgressHandler.Instance.ItemsDictionary[itemData._itemName].IsNewNotViewedInShop());
    }
}
