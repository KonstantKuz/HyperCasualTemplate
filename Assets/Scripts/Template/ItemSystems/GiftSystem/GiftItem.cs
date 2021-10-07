using System;
using UnityEngine;
using Template.Tools;
using Template.LevelManagement;
using Template.ItemSystems.ShopSystem;
using Template.ItemSystems.InventorySystem;

namespace Template.ItemSystems.GiftSystem
{
    public class GiftItem
    {
        private GiftItemData _giftData;
        private PlayerPrefsProperty<int> _receiveLevelOffset;
        private PlayerPrefsProperty<bool> _isReceived;

        public GiftItem(GiftItemData giftData)
        {
            _giftData = giftData;
            _receiveLevelOffset = new PlayerPrefsProperty<int>($"{Name}ReceiveLevel", 0);
            _isReceived = new PlayerPrefsProperty<bool>($"{Name}IsReceived", false);
        }

        public string Name => _giftData.InventoryData.Name;
        public Sprite Icon => _giftData.InventoryData.Icon;
        public UnlockType UnlockType => _giftData.UnlockType;
        public int DefaultReceiveLevel => _giftData.ReceiveLevel;
        public bool IsReceived => _isReceived.Value;
        public int ReceiveLevelOffset => _receiveLevelOffset.Value;
        public void IncreaseReceiveLevelOffset() => _receiveLevelOffset.Value++;
        public int ActualReceiveLevel => DefaultReceiveLevel - ReceiveLevelOffset;
        
        public void Receive()
        {
            _isReceived.Value = true;
            
            switch (UnlockType)
            {
                case UnlockType.UnlockToShop:
                    Shop.Instance.GetItem(Name).UnlockToShop();
                    break;
                case UnlockType.UnlockToUse:
                    Inventory.Instance.GetItem(Name).UnlockToUse();
                    break;
            }
        }
        
        public bool IsReceiveLevelReached()
        {
            return LevelManager.Instance.CurrentDisplayLevelNumber >= ActualReceiveLevel;
        }

        public bool WillBeReceivedOnNextLevel()
        {
            return LevelManager.Instance.CurrentDisplayLevelNumber + 1 >= ActualReceiveLevel;
        }
    }
}