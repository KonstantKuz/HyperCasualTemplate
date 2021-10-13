using System;
using Templates.Ads;
using Templates.ItemSystems.InventorySystem;
using Templates.UI;
using UnityEngine;
using UnityEngine.UI;
using GiftStatus = Templates.ItemSystems.GiftSystem.GiftItem.GiftStatus;

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

        private GiftsQueue _giftsQueue;
        private GiftItem _nextGift;

        private const float ShowProgressDelay = 2f;
        
        private void Start()
        {
            _panelContainer.SetActive(false);

            _giftsQueue = GiftsQueue.Instance;
            
            if (_giftsQueue.IsAllReceived())
            {
                return;
            }
            
            _nextGift = _giftsQueue.Next();
            _giftImage.sprite = _nextGift.Icon;
            _giftShadowImage.sprite = _nextGift.Icon;

            _nextGiftProgress.Initialize(_nextGift.ReceiveProgress);
        }

        public void ShowPanel(Action onPanelClosed)
        {
            _onPanelClosed = onPanelClosed;
            
            _panelContainer.SetActive(true);
            
            if (_giftsQueue.IsAllReceived())
            {
                Close();
                return;
            }
            
            _nextGift.IncreaseProgress();
            _nextGiftProgress.UpdateVisualProgress(_nextGift.ReceiveProgress);

            switch (_nextGift.CurrentStatus())
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
            _nextGift.BoostProgress();
            _nextGiftProgress.UpdateVisualProgress(_nextGift.ReceiveProgress);
        
            if (_nextGift.CurrentStatus() == GiftStatus.UnlockedToShopForOneVideo)
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
    }
}
