using Template.ItemSystems.InventorySystem;
using UnityEngine;

namespace Template.ItemSystems.ShopSystem
{
    [CreateAssetMenu(fileName = "ShopItemData", menuName = "Item Systems/ShopItemData")]
    public class ShopItemData : ScriptableObject
    {
        public InventoryItemData InventoryData;
        public bool UnlockedToShopByDefault;
        public CurrencyType currencyType;
        public int PriceAmount;
    }
}
