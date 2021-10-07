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
        public UnlockType UnlockType;

        public int ProgressToReceive;
        public int RegularIncreaseValue;
        public bool CanBeBoosted;
        public int BoostIncreaseValue;
    }
}