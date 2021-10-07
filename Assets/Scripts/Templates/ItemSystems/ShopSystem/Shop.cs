using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Templates.ItemSystems.ShopSystem
{
    [Serializable]
    public class CurrencySprite
    {
        public CurrencyType Currency;
        public Sprite Sprite;
    }
    
    public class Shop : Singleton<Shop>
    {
        [SerializeField] private ShopItemsGroup[] _itemsGroups;
        [SerializeField] private List<CurrencySprite> _currencySprites;
            
        private Dictionary<string, ShopItem> _itemsDictionary;

        private Dictionary<string, ShopItem> ItemsDictionary
        {
            get
            {
                if (_itemsDictionary == null)
                {
                    InitializeItemsDictionary();
                }

                return _itemsDictionary;
            }
        }

        private void InitializeItemsDictionary()
        {
            _itemsDictionary = new Dictionary<string, ShopItem>();
        
            foreach (ShopItemsGroup group in _itemsGroups)
            {
                foreach (ShopItemData shopData in group.Items)
                {
                    ShopItem shopItem = new ShopItem(shopData);
                    _itemsDictionary.Add(shopData.InventoryData.Name, shopItem);
                }
            }
        }

        private ShopItemsGroup GetItemsGroup(string groupName)
        {
            return _itemsGroups.First(group => group.Name == groupName);
        }

        public ShopItem GetItem(string itemName)
        {
            return ItemsDictionary[itemName];
        }
        
        public bool IsAllItemsUnlockedToShop(string groupName)
        {
            ShopItemsGroup group = GetItemsGroup(groupName);
            return group.Items.All(shopData => ItemsDictionary[shopData.InventoryData.Name].IsUnlockedToShop);
        }

        public Sprite GetCurrencySprite(CurrencyType currencyType)
        {
            return _currencySprites.First(currency => currency.Currency == currencyType).Sprite;
        }
    }
}
