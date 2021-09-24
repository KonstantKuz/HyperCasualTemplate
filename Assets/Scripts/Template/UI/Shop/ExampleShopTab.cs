using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ExampleShopTab : UniversalShopTab
{
    [SerializeField] private Image _itemView;

    public override void Awake()
    {
        base.Awake();

        List<ProgressiveItemContainer> itemContainers = ProgressiveItemsHandler.Instance.ItemsDictionary.Values.ToList();
        itemContainers[0].SetAsEquipped();
    }
    
    public override void OnItemSelected(string itemName)
    {
        _itemView.gameObject.SetActive(true);
        _itemView.sprite = ProgressiveItemsHandler.Instance.ItemsDictionary[itemName].Icon();
        print($"OnItemSelected {itemName}");
    }

    public override void OnEquipItem(string itemName)
    {
        ProgressiveItemsHandler.Instance.ItemsDictionary[itemName].SetAsEquipped();
        print($"OnEquipItem {itemName}");
    }

    public override void OnBuyItem(string itemName)
    {
        print($"OnBuyItem {itemName}");
    }
}
