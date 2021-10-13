using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Templates.ItemSystems.GiftSystem
{
    public class GiftQueue : Singleton<GiftQueue>
    {
        [SerializeField] private List<GiftItemData> _receivingQueue;

        private List<GiftItem> _items;
        private List<GiftItem> Items
        {
            get
            {
                if (_items == null)
                {
                    InitializeGiftItems();
                }

                return _items;
            }
        }

        private void InitializeGiftItems()
        {
            _items = _receivingQueue.Select(giftData => new GiftItem(giftData)).ToList();
        }

        public GiftItem NextGift()
        {
            return Items.First(gift => !gift.IsReceived);
        }

        public bool AllGiftsReceived()
        {
            return Items.All(gift => gift.IsReceived);
        }
    }
}
