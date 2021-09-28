using System;

namespace Template
{
    public class AdsDummyImpl: IAdsImpl
    {
        public void DestroyBanner()
        {
        }

        public void Init()
        {
        }

        public void LoadInterstitial()
        {
        }

        public void LoadBanner()
        {
        }

        public void SubscribeToHandleInterstitialAd(Action action, Action<string> onInterstitialAdLoadFailedEvent,
            Action<string> onInterstitialAdFailedToDisplayEvent, Action onInterstitialAdClosedEvent)
        {
        }

        public void SubscribeToHandleRewardedAd(Action<bool> onRewardedVideoAvailabilityChangedEvent,
            Action<string> onRewardedVideoAdShowFailedEvent, Action onRewardedVideoAdClosedEvent,
            Action onRewardedVideoAdRewardedEvent)
        {
        }

        public void SubscribeToHandleBannerAd()
        {
        }

        public void HideBanner()
        {
        }

        public bool IsInterstitialReady()
        {
            return true;
        }

        public void ShowInterstitial()
        {
        }

        public void ShowRewardedVideo(string placementName)
        {
        }

        public bool IsRewardedVideoAvailable { get; set; }
    }
}