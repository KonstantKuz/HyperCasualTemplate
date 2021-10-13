using System;
using Templates.ItemSystems.InventorySystem;
using Templates.ItemSystems.ShopSystem;
using Templates.Tools;
using UnityEngine;

namespace Templates.ItemSystems.GiftSystem
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
        public int IncreaseValue => _giftData.IncreaseValue;
        public bool CanBeBoosted => _giftData.CanBeBoosted;
        public int BoostValue => _giftData.BoostValue;

        public int ReceiveProgress => _receiveProgress.Value;
        public bool IsReceived => _isReceived.Value;
        
        public void IncreaseProgress(int value)
        {
            _receiveProgress.Value += value;
            TryReceive();
        }
        
        public void IncreaseProgress()
        {
            _receiveProgress.Value += _giftData.IncreaseValue;
            TryReceive();
        }
        
        public void BoostProgress()
        {
            _receiveProgress.Value += _giftData.BoostValue;
            TryReceive();
        }
        
        private void TryReceive()
        {
            if (!IsReceiveProgressReached())
            {
                return;
            }
            
            _isReceived.Value = true;
            
            switch (UnlockType)
            {
                case UnlockType.UnlockToShop:
                    Shop.Instance.GetItem(Name).UnlockToShop();
                    break;
                case UnlockType.UnlockToUse:
                    Shop.Instance.GetItem(Name).UnlockToShop();
                    Inventory.Instance.GetItem(Name).UnlockToUse();
                    break;
            }
        }

        private bool IsReceiveProgressReached()
        {
            return ReceiveProgress >= 100;
        }
        
        public GiftStatus CurrentStatus()
        {
            if (IsReceived && UnlockType == UnlockType.UnlockToUse)
            {
                return GiftStatus.UnlockedToUse;
            }
            else if (IsReceived && UnlockType == UnlockType.UnlockToShop)
            {
                return Shop.Instance.GetItem(Name).IsPriceEqualsOneVideo ? 
                    GiftStatus.UnlockedToShopForOneVideo : GiftStatus.UnlockedToShop;
            }
            else if (!IsReceived && CanBeBoosted)
            {
                return GiftStatus.LockedCanBeBoosted;
            }
            else if (!IsReceived && !CanBeBoosted)
            {
                return GiftStatus.LockedCanNotBeBoosted;
            }
            
            throw new Exception("Not specified type.");
        }
        
        public enum GiftStatus
        {
            UnlockedToUse,
            UnlockedToShop,
            UnlockedToShopForOneVideo,
            LockedCanBeBoosted,
            LockedCanNotBeBoosted,
        }
    }
}