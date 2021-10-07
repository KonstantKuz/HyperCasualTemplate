﻿using Templates.ItemSystems.InventorySystem;
using UnityEngine;

namespace Templates.ItemSystems.GiftSystem
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

        public int RegularIncreaseValue;
        public bool CanBeBoosted;
        public int BoostIncreaseValue;
    }
}