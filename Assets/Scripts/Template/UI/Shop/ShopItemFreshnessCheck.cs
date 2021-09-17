using UnityEngine;

public class ShopItemFreshnessCheck
{
    public static bool IsItemCheckedInShop(string itemName)
    {
        string checkPrefsKey = itemName + "IsCheckedInShop";
        int isChecked = PlayerPrefs.GetInt(checkPrefsKey);
        
        if (isChecked == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void SetItemCheckedInShop(string itemName)
    {
        string checkPrefsKey = itemName + "IsCheckedInShop";
        PlayerPrefs.SetInt(checkPrefsKey, 1);
    }
}