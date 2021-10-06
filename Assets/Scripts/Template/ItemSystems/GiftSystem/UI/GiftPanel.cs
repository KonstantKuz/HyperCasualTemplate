using System;
using Template.Ads;
using Template.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Template.ItemSystems.GiftSystem.UI
{
    public class GiftPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _panelContainer;
        [SerializeField] private Image _giftImage;
        [SerializeField] private Image _giftShadowImage;
        [SerializeField] private NextGiftProgressBar _nextGiftProgress;
        [SerializeField] private RewardedPanel _boostPanel;
        // [SerializeField] private RewardedPanel _unlockPanel;

        private Action _onPanelHandled;
        private GiftItem _lastLockedGift;
    
        private void Start()
        {
            if (!GiftGiver.Instance.AllGiftsReceived())
            {
                _lastLockedGift = GiftGiver.Instance.LastLockedGift();
                _giftImage.sprite = _lastLockedGift.Icon;
                _giftShadowImage.sprite = _lastLockedGift.Icon;

                _nextGiftProgress.Initialize();
            }
        
            _panelContainer.SetActive(false);
        }

        public void ShowPanel(Action onPanelHandled)
        {
            _onPanelHandled = onPanelHandled;

            _panelContainer.SetActive(true);
        
            DelayHandler.Instance.DelayedCallCoroutine(0.1f, delegate
            {
                _nextGiftProgress.UpdateProgress();
            });
        }

        public void ShowBoostOrUnlockButton()
        {
            if (_lastLockedGift.WillBeReceivedOnNextLevel() && _lastLockedGift.UnlockType == UnlockType.UnlockToShop)
            {
                // _unlockPanel.ShowPanel(TryUnlock, Close);
                Close();
            }
            else
            {
                _boostPanel.ShowPanel(TryBoostProgress, Close);
            }
        }

        private void TryBoostProgress()
        {
            _boostPanel.HidePanel();
        
            AdsManager.Instance.onRewardedAdFailedOrDiscarded += Close;
            AdsManager.Instance.onRewardedAdRewarded += BoostProgress;
        
            AdsManager.Instance.ShowRewardedAd("BoostGiftProgress");
        }

        private void BoostProgress()
        {
            _lastLockedGift.DecreaseReceiveLevel();
            _nextGiftProgress.UpdateProgress();
        
            if (_lastLockedGift.WillBeReceivedOnNextLevel() && _lastLockedGift.UnlockType == UnlockType.UnlockToShop)
            {
                // _unlockPanel.ShowPanel(TryUnlock, Close);
                Close();
            }
            else
            {
                DelayHandler.Instance.DelayedCallCoroutine(2, Close);
            }
        }

        // private void TryUnlock()
        // {
        //     _unlockPanel.HidePanel();
        //
        //     AdsManager.Instance.onRewardedAdFailedOrDiscarded += Close;
        //     AdsManager.Instance.onRewardedAdRewarded += Unlock;
        //
        //     AdsManager.Instance.ShowRewardedAd("UnlockGiftToUse");
        // }
        //
        // private void Unlock()
        // {
        //     _lastLockedGift.DecreaseReceiveLevel();
        //     _nextGiftProgress.UpdateProgress();
        //     
        //     DelayHandler.Instance.DelayedCallCoroutine(1, Close);
        // }

        private void Close()
        {
            _onPanelHandled?.Invoke();
        }
    }
}
