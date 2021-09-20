using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleShopTab : UniversalShopTab
{
    public override bool IsItemEquipped(string itemName)
    {
        return PlayerPrefs.GetString("equipped item") == itemName;
    }

    public override void OnItemSelected(string itemName)
    {
        print($"OnItemSelected {itemName}");
    }

    public override void OnEquipItem(string itemName)
    {
        PlayerPrefs.SetString("equipped item", itemName);
        print($"OnEquipItem {itemName}");
    }

    public override void OnBuyItem(string itemName)
    {
        print($"OnBuyItem {itemName}");
    }
}
