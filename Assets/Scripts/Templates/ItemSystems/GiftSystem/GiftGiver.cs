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
                    InitializeGifts();
                }

                return _gifts;
            }
        }

        private void InitializeGifts()
        {
            _gifts = new List<GiftItem>();

            foreach (GiftItem giftItem in _items.Select(giftData => new GiftItem(giftData)))
            {
                _gifts.Add(giftItem);
            }
        }

        public void IncreaseNextGiftProgress()
        {
            if (AllGiftsReceived())
            {
                return;
            }

            GiftItem nextGift = NextGift();
            
            nextGift.IncreaseReceiveProgress(nextGift.RegularIncreaseValue);
            
            if (nextGift.IsReceiveProgressReached())
            {
                nextGift.Receive();
            }
        }

        public void BoostNextGiftProgress()
        {
            GiftItem nextGift = NextGift();
            
            nextGift.IncreaseReceiveProgress(nextGift.BoostIncreaseValue);
            
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
