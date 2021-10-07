using System;

namespace Templates.Ads
{
    public interface IAdsImpl
    {
        void Initialize();

        void LoadInterstitial();

        void LoadBanner();
        void ShowBanner();
        void HideBanner();
        void DestroyBanner();

        void SubscribeToHandleInterstitialAd(Action onInterstitialAdReadyEvent,
            Action<string> onInterstitialAdLoadFailedEvent,
            Action<string> onInterstitialAdFailedToDisplayEvent,
            Action onInterstitialAdClosedEvent);

        void SubscribeToHandleRewardedAd(Action<bool> onRewardedVideoAvailabilityChangedEvent,
            Action<string> onRewardedVideoAdShowFailedEvent,
            Action onRewardedVideoAdClosedEvent,
            Action onRewardedVideoAdRewardedEvent);

        void SubscribeToHandleBannerAd(Action<string> onBannerLoadFailedEvent);

        bool IsInterstitialAvailable();
        void ShowInterstitial();
        bool IsRewardedVideoAvailable();
        void ShowRewardedVideo(string placementName);
    }
}