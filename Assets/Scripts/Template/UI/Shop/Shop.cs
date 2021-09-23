using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private Button open;
    [SerializeField] private Button close;
    [SerializeField] private GameObject newItemIndicator;
    [SerializeField] private List<ShopTab> tabs;

    private void Start()
    {
        open.onClick.AddListener(OpenShop);
        close.onClick.AddListener(CloseShop);

        foreach (var tab in tabs)
        {
            tab.OpenTabButton.OnClickedOpenTab += SwitchTabs;
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
        
        foreach (var tab in tabs)
        {
            tab.OnShopClosed();
        }
        
        UpdateNewItemsIndication();
    }

    public void SwitchTabs(ShopTab clickedTab)
    {
        tabs.ForEach(tab => tab.SetSelected(tab == clickedTab));
    }

    public void UpdateTabsItems()
    {
        foreach (var tab in tabs)
        {
            tab.UpdateItemsStatuses();
        }
    }

    private void UpdateNewItemsIndication()
    {
        newItemIndicator.SetActive(tabs.Any(tab => tab.HasNewNotViewedInShopItems()));
    }
}
