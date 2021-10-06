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
        private PlayerPrefsProperty<int> _receiveLevel;
        private PlayerPrefsProperty<bool> _isReceived;

        public GiftItem(GiftItemData giftData)
        {
            _giftData = giftData;
            _receiveLevel = new PlayerPrefsProperty<int>($"{Name}ReceiveLevel", _giftData.ValueToReceive);
            _isReceived = new PlayerPrefsProperty<bool>($"{Name}IsReceived", false);
        }

        public string Name => _giftData.InventoryData.Name;
        public Sprite Icon => _giftData.InventoryData.Icon;
        public ConditionToReceive ConditionToReceive => _giftData.ConditionToReceive;
        public UnlockType UnlockType => _giftData.UnlockType;

        public bool IsReceived => _isReceived.Value;
 
        public int ReceiveLevel() => _receiveLevel.Value;
        public void DecreaseReceiveLevel() => _receiveLevel.Value--;
        
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
        
        public bool IsReadyToReceive()
        {
            switch (ConditionToReceive)
            {
                case ConditionToReceive.LevelReached:
                    return IsLevelReached();
            }
            
            throw new Exception($"ConditionToReceive type mismatch in {_giftData.name} gift item data.");
        }

        public bool WillBeReceivedOnNextLevel()
        {
            return LevelManager.Instance.CurrentDisplayLevelNumber + 1 >= ReceiveLevel();
        }
        private bool IsLevelReached() => LevelManager.Instance.CurrentDisplayLevelNumber >= ReceiveLevel();
    }
}