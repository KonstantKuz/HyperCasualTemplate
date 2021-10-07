using Templates.Tools;
using UnityEngine;

namespace Templates.ItemSystems.ShopSystem
{
    public class ShopItem
    {
        private ShopItemData _shopData;
        private PlayerPrefsProperty<bool> _isUnlockedToShop;
        private PlayerPrefsProperty<bool> _isFreshInShop;
        private PlayerPrefsProperty<int> _priceAmount;
        
        public ShopItem(ShopItemData shopData)
        {
            _shopData = shopData;
            _isUnlockedToShop = new PlayerPrefsProperty<bool>($"{Name}IsUnlockedToShop", false);
            _isFreshInShop = new PlayerPrefsProperty<bool>($"{Name}IsFreshInShop", false);
            _priceAmount = new PlayerPrefsProperty<int>($"{Name}PriceAmount", _shopData.PriceAmount);
        }

        public string Name => _shopData.InventoryData.Name;
        public Sprite Icon => _shopData.InventoryData.Icon;
        public CurrencyType CurrencyType => _shopData.currencyType;
        public int PriceAmount => _priceAmount.Value;

        public void SetPriceAmount(int value) => _priceAmount.Value = value;
    
        public void UnlockToShop()
        {
            _isUnlockedToShop.Value = true;
            _isFreshInShop.Value = true;
        }
        
        public bool IsUnlockedToShop => IsUnlockedToShopByDefault || _isUnlockedToShop.Value;
        public bool IsUnlockedToShopByDefault => _shopData.UnlockedToShopByDefault;

        public bool IsPriceEqualsOneVideo => CurrencyType == CurrencyType.Video && PriceAmount == 1;
        

        public void MarkAsViewedInShop() => _isFreshInShop.Value = false;
        public bool IsFreshInShop => _isFreshInShop.Value;
    }
}
