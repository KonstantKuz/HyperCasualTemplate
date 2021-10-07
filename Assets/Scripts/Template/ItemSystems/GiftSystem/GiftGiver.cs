using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using Template.LevelManagement;
using Template.Tools;

namespace Template.ItemSystems.GiftSystem
{
    public class GiftGiver : Singleton<GiftGiver>
    {
        [SerializeField] private List<GiftItemData> _items;

        private PlayerPrefsProperty<string> _lastReceivedGiftName = new PlayerPrefsProperty<string>("LastReceivedGift", "");
       
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

        private void Awake()
        {
            TryGiveLastLockedGift();
        }

        private void TryGiveLastLockedGift()
        {
            GiftItem nextGift = NextGift();
            if (nextGift.IsReceiveLevelReached())
            {
                nextGift.Receive();
                _lastReceivedGiftName.Value = nextGift.Name;
            }
        }
        
        public GiftItem GetItem(string itemName)
        {
            return Gifts.First(gift => gift.Name == itemName);
        }

        public GiftItem LastReceivedGift()
        {
            return _lastReceivedGiftName.Value == "" ? null : GetItem(_lastReceivedGiftName.Value);
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
