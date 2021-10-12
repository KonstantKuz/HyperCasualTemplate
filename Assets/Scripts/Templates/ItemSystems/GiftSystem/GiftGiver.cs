using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Templates.ItemSystems.GiftSystem
{
    public class GiftGiver : Singleton<GiftGiver>
    {
        [SerializeField] private List<GiftItemData> _items;

        private List<GiftItem> _gifts;

        private List<GiftItem> Gifts
        {
            get
            {
                if (_gifts == null)
                {
                    InitializeGiftItems();
                }

                return _gifts;
            }
        }

        private void InitializeGiftItems()
        {
            _gifts = _items.Select(giftData => new GiftItem(giftData)).ToList();
        }

        public void IncreaseNextGiftProgress()
        {
            if (AllGiftsReceived())
            {
                return;
            }

            NextGift().RegularIncreaseProgress();
            TryReceiveNextGift();
        }

        public void BoostNextGiftProgress()
        {
            NextGift().BoostIncreaseProgress();
            TryReceiveNextGift();
        }

        private void TryReceiveNextGift()
        {
            GiftItem nextGift = NextGift();
            
            if (nextGift.IsReceiveProgressReached())
            {
                nextGift.Receive();
            }
        }
        
        public GiftItem GetItem(string itemName)
        {
            return Gifts.First(gift => gift.Name == itemName);
        }

        public GiftItem NextGift()
        {
            return Gifts.First(gift => !gift.IsReceived);
        }

        public bool AllGiftsReceived()
        {
            return Gifts.All(gift => gift.IsReceived);
        }
    }
}
