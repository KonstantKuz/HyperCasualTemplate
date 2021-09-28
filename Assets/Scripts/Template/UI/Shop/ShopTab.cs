using System;
using System.Collections;
using System.Collections.Generic;
using Template.UI.Shop;
using UnityEngine;
using UnityEngine.UI;

public class ShopTab : MonoBehaviour
{
    [SerializeField] private Template.UI.Shop.TabButton openTabButton;
    public Template.UI.Shop.TabButton OpenTabButton => openTabButton;
    
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] protected List<ShopItemButton> itemButtons;

    public virtual void OnEnable()
    {
        for (int i = 0; i < itemButtons.Count; i++)
        {
            itemButtons[i].OnClicked += UpdateItemButtonsSelection;
        }
    }

    private void UpdateItemButtonsSelection(ShopItemButton selectedItem)
    {
        itemButtons.ForEach(button => button.SetSelected(button == selectedItem));
    }

    public void SetSelected(bool value)
    {
        openTabButton.SetSelected(value);
        if (value)
        {
            rectTransform.SetAsLastSibling();
        }
    }

    public virtual void UpdateItemsStatuses()
    {
        
    }

    public virtual void OnShopClosed()
    {
    }

    public virtual bool HasNewNotViewedInShopItems()
    {
        return false;
    }
}
