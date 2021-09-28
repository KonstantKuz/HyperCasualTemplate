using UnityEngine;

namespace Template.ProgressiveItemsHandler
{
    public enum ItemPriceType
    {
        Coins,
        Video,
    }

    [System.Serializable]
    public class ProgressiveItemData
    {
        public string itemName;
        public Sprite icon;
        public bool unlockedToShopByDefault;
        public bool unlockedToUseByDefault;
        public int progressToUnlock;
        public bool unlockCompletelyOnProgressPassed;
        public ItemPriceType priceType;
        public int price;
    }
}