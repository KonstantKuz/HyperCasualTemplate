using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Templates.ItemSystems.ShopSystem.UI
{
    public class ShopViewer : MonoBehaviour
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
            PlayerWallet.Instance.OnCurrencyChanged += UpdateTabsItemsStatuses;
            PlayerWallet.Instance.OnCurrencyChanged += UpdateNewItemsIndication;
        }

        private void OpenShop()
        {
            open.gameObject.SetActive(false);
            container.SetActive(true);

            UpdateTabsItemsStatuses();
        }

        private void CloseShop()
        {
            open.gameObject.SetActive(true);
            container.SetActive(false);
            tabs.ForEach(tab => tab.OnShopClosed());
        
            UpdateNewItemsIndication();
        }

        public void SwitchTabs(ShopTab clickedTab)
        {
            if (clickedTab == null)
            {
                throw new NullReferenceException("Reference to shop tab in tab button is null.");
            }
            tabs.ForEach(tab => tab.SetSelected(tab == clickedTab));
        }

        public void UpdateTabsItemsStatuses()
        {
            tabs.ForEach(tab => tab.UpdateItemsStatuses());
        }

        private void UpdateNewItemsIndication()
        {
            newItemIndicator.SetActive(tabs.Any(tab => tab.HasFreshItems()));
        }
    }
}
