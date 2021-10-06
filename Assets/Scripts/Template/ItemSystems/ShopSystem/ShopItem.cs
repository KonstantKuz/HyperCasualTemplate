using Template.Tools;
using UnityEngine;

namespace Template.ItemSystems.ShopSystem
{
    public class ShopItem
    {
        private ShopItemData _shopData;
        private PlayerPrefsProperty<bool> _unlockedToShopStatus;
        private PlayerPrefsProperty<bool> _freshInShopStatus;
        private PlayerPrefsProperty<int> _priceAmount;
        
        public ShopItem(ShopItemData shopData)
        {
            _shopData = shopData;
            _unlockedToShopStatus = new PlayerPrefsProperty<bool>($"{Name}IsAvailableToShop", false);
            _freshInShopStatus = new PlayerPrefsProperty<bool>($"{Name}IsFreshInShop", false);
            _priceAmount = new PlayerPrefsProperty<int>($"{Name}PriceAmount", _shopData.PriceAmount);
        }

        public string Name => _shopData.InventoryData.Name;
        public Sprite Icon => _shopData.InventoryData.Icon;
        public CurrencyType CurrencyType => _shopData.currencyType;
        public int PriceAmount => _priceAmount.Value;

        public void SetPriceAmount(int value) => _priceAmount.Value = value;
    
        public void UnlockToShop()
        {
            _unlockedToShopStatus.Value = true;
            _freshInShopStatus.Value = true;
        }
        
        public bool IsUnlockedToShop() => IsUnlockedToShopByDefault() || _unlockedToShopStatus.Value;
        public bool IsUnlockedToShopByDefault() => _shopData.UnlockedToShopByDefault;

        public void MarkAsViewedInShop() => _freshInShopStatus.Value = false;
        public bool IsFreshInShop() => _freshInShopStatus.Value;
    }
}
