using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Templates.ItemSystems.GiftSystem
{
    public class GiftsQueue : Singleton<GiftsQueue>
    {
        [SerializeField] private List<GiftItemData> _receivingQueue;

        private List<GiftItem> _items;
        private List<GiftItem> Items
        {
            get
            {
                if (_items == null)
                {
                    InitializeItems();
                }

                return _items;
            }
        }

        private void InitializeItems()
        {
            _items = _receivingQueue.Select(giftData => new GiftItem(giftData)).ToList();
        }

        public GiftItem Next()
        {
            return Items.First(gift => !gift.IsReceived);
        }

        public bool IsAllReceived()
        {
            return Items.All(gift => gift.IsReceived);
        }
    }
}
