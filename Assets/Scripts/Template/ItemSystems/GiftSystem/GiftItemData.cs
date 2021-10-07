using Template.ItemSystems.InventorySystem;
using UnityEngine;

namespace Template.ItemSystems.GiftSystem
{
    public enum UnlockType
    {
        UnlockToShop,
        UnlockToUse,
    }
    
    [CreateAssetMenu(fileName = "GiftItemData", menuName = "Item Systems/GiftItemData", order = 0)]
    public class GiftItemData : ScriptableObject
    {
        public InventoryItemData InventoryData;
        public int ReceiveLevel;
        public UnlockType UnlockType;
    }
}