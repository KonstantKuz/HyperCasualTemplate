using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopTab : MonoBehaviour
{
    [SerializeField] private TabButton openTabButton;
    public TabButton OpenTabButton => openTabButton;
    
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] protected ShopItemButton[] itemButtons;

    public virtual void OnEnable()
    {
        for (int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].OnClicked += UpdateItemsSelection;
        }
    }

    public virtual void UpdateItemsStatuses()
    {
        
    }

    private void UpdateItemsSelection(ShopItemButton selectedItem)
    {
        selectedItem.SetSelected(true);
        
        for (int i = 0; i < itemButtons.Length; i++)
        {
            if (itemButtons[i] != selectedItem)
            {
                itemButtons[i].SetSelected(false);
            }
        }
    }

    public void OnSwitchTabs(ShopTab switchedTab)
    {
        if (this == switchedTab)
        {
            openTabButton.SetSelected(true);
            rectTransform.SetAsLastSibling();
        }
        else
        {
            openTabButton.SetSelected(false);
        }
    }

    public virtual void OnShopClosed()
    {
    }

    public virtual bool HasNewUncheckedItem()
    {
        return false;
    }
}
