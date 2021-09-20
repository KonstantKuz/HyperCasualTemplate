using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private Button open;
    [SerializeField] private Button close;
    [SerializeField] private GameObject newItemIndicator;
    [SerializeField] private ShopTab[] tabs;

    private void Start()
    {
        open.onClick.AddListener(OpenShop);
        close.onClick.AddListener(CloseShop);

        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].OpenTabButton.OnClickedOpenTab += SwitchTabs;
        }

        UpdateNewItemsIndication();
        PlayerWallet.Instance.OnMoneyChanged += UpdateTabsItems;
    }

    private void OpenShop()
    {
        open.gameObject.SetActive(false);
        container.SetActive(true);
    }

    private void CloseShop()
    {
        open.gameObject.SetActive(true);
        container.SetActive(false);
        
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].OnShopClosed();
        }
        
        UpdateNewItemsIndication();
    }

    public void SwitchTabs(ShopTab clickedTab)
    {
        foreach (ShopTab shopTab in tabs)
        {
            shopTab.OnSwitchTabs(clickedTab);
        }
    }

    public void UpdateTabsItems()
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].UpdateItemsStatuses();
        }
    }

    private void UpdateNewItemsIndication()
    {
        bool hasNewUncheckedItems = false;
        
        for (int i = 0; i < tabs.Length; i++)
        {
            if (tabs[i].HasNewUncheckedItem())
            {
                hasNewUncheckedItems = true;
                break;
            }
        }
        
        newItemIndicator.SetActive(hasNewUncheckedItems);
    }
}
