using System;
using Templates.Ads;
using Templates.ItemSystems.InventorySystem;
using Templates.ItemSystems.ShopSystem;
using Templates.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Templates.ItemSystems.GiftSystem.UI
{
    public class GiftProgressUpdatePanel : MonoBehaviour
    {
        [SerializeField] private GameObject _panelContainer;
        [SerializeField] private Image _giftImage;
        [SerializeField] private Image _giftShadowImage;
        [SerializeField] private NextGiftProgressBar _nextGiftProgress;
        [SerializeField] private RewardedPanel _boostPanel;
        [SerializeField] private RewardedPanel _unlockPanel;

        private Action _onPanelClosed;

        private GiftGiver _giftGiver;
        private GiftItem _nextGift;

        private const float ShowProgressDelay = 2f;
        
        private void Start()
        {
            _panelContainer.SetActive(false);

            _giftGiver = GiftGiver.Instance;
            
            if (_giftGiver.AllGiftsReceived())
            {
                return;
            }
            
            _nextGift = _giftGiver.NextGift();
            _giftImage.sprite = _nextGift.Icon;
            _giftShadowImage.sprite = _nextGift.Icon;

            _nextGiftProgress.Initialize(_nextGift.ReceiveProgress);
        }

        public void ShowPanel(Action onPanelClosed)
        {
            _onPanelClosed = onPanelClosed;
            
            _panelContainer.SetActive(true);
            
            if (_giftGiver.AllGiftsReceived())
            {
                Close();
                return;
            }
            
            _giftGiver.IncreaseNextGiftProgress();
            _nextGiftProgress.UpdateVisualProgress(_nextGift.ReceiveProgress);

            switch (CurrentGiftStatus())
            {
                case GiftStatus.UnlockedToShopForOneVideo:
                    _unlockPanel.ShowPanel(TryUnlockItemToUse, Close);
                    break;
                case GiftStatus.LockedCanBeBoosted:
                    _boostPanel.ShowPanel(TryBoostProgress, Close);
                    break;
                default:
                    DelayHandler.Instance.DelayedCallCoroutine(ShowProgressDelay, Close);
                    break;
            }
        }
        
        private void TryBoostProgress()
        {
            _boostPanel.HidePanel();
        
            AdsManager.Instance.onRewardedAdFailedOrDiscarded += Close;
            AdsManager.Instance.onRewardedAdRewarded += BoostProgress;
        
            AdsManager.Instance.ShowRewardedAd($"Boost {_nextGift.Name} Gift Progress");
        }

        private void BoostProgress()
        {
            _giftGiver.BoostNextGiftProgress();
            _nextGiftProgress.UpdateVisualProgress(_nextGift.ReceiveProgress);
        
            if (CurrentGiftStatus() == GiftStatus.UnlockedToShopForOneVideo)
            {
                _unlockPanel.ShowPanel(TryUnlockItemToUse, Close);
            }
            else
            {
                DelayHandler.Instance.DelayedCallCoroutine(ShowProgressDelay, Close);
            }
        }

        private void TryUnlockItemToUse()
        {
            _unlockPanel.HidePanel();
        
            AdsManager.Instance.onRewardedAdFailedOrDiscarded += Close;
            AdsManager.Instance.onRewardedAdRewarded += UnlockItemToUse;
        
            AdsManager.Instance.ShowRewardedAd($"Unlock {_nextGift.Name} Item To Use");
        }
        
        private void UnlockItemToUse()
        {
            Inventory.Instance.GetItem(_nextGift.Name).UnlockToUse();
            
            Close();
        }

        private void Close()
        {
            _panelContainer.SetActive(false);
            _onPanelClosed?.Invoke();
        }

        private GiftStatus CurrentGiftStatus()
        {
            if (_nextGift.IsReceived && _nextGift.UnlockType == UnlockType.UnlockToUse)
            {
                return GiftStatus.UnlockedToUse;
            }
            else if (_nextGift.IsReceived && _nextGift.UnlockType == UnlockType.UnlockToShop)
            {
                return Shop.Instance.GetItem(_nextGift.Name).IsPriceEqualsOneVideo ? 
                    GiftStatus.UnlockedToShopForOneVideo : GiftStatus.UnlockedToShop;
            }
            else if (!_nextGift.IsReceived && _nextGift.CanBeBoosted)
            {
                return GiftStatus.LockedCanBeBoosted;
            }
            else if (!_nextGift.IsReceived && !_nextGift.CanBeBoosted)
            {
                return GiftStatus.LockedCanNotBeBoosted;
            }
            
            throw new Exception("Not specified type.");
        }
        
        private enum GiftStatus
        {
            UnlockedToUse,
            UnlockedToShop,
            UnlockedToShopForOneVideo,
            LockedCanBeBoosted,
            LockedCanNotBeBoosted,
        }
    }
}
