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
        private PlayerPrefsProperty<int> _receiveProgress;
        private PlayerPrefsProperty<bool> _isReceived;

        public GiftItem(GiftItemData giftData)
        {
            _giftData = giftData;
            _receiveProgress = new PlayerPrefsProperty<int>($"{Name}ReceiveProgress", 0);
            _isReceived = new PlayerPrefsProperty<bool>($"{Name}IsReceived", false);
        }

        public string Name => _giftData.InventoryData.Name;
        public Sprite Icon => _giftData.InventoryData.Icon;
        public UnlockType UnlockType => _giftData.UnlockType;
        public int ProgressToReceive => _giftData.ProgressToReceive;
        public int RegularIncreaseValue => _giftData.RegularIncreaseValue;
        public bool CanBeBoosted => _giftData.CanBeBoosted;
        public int BoostIncreaseValue => _giftData.BoostIncreaseValue;

        public bool IsReceived => _isReceived.Value;
        public int ReceiveProgress => _receiveProgress.Value;
        public void IncreaseReceiveProgress(int value) => _receiveProgress.Value += value;

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
        
        public bool IsReceiveProgressReached()
        {
            return ReceiveProgress >= ProgressToReceive;
        }
    }
}