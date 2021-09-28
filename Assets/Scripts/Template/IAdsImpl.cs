using System;

public interface IAdsImpl
{
    void DestroyBanner();
    void Init();

    void LoadInterstitial();
    void LoadBanner();

    void SubscribeToHandleInterstitialAd(Action action, 
        Action<string> onInterstitialAdLoadFailedEvent,
        Action<string> onInterstitialAdFailedToDisplayEvent, 
        Action onInterstitialAdClosedEvent);

    void SubscribeToHandleRewardedAd(Action<bool> onRewardedVideoAvailabilityChangedEvent,
        Action<string> onRewardedVideoAdShowFailedEvent, 
        Action onRewardedVideoAdClosedEvent,
        Action onRewardedVideoAdRewardedEvent);

    void SubscribeToHandleBannerAd();
    void HideBanner();
    bool IsInterstitialReady();
    void ShowInterstitial();
    void ShowRewardedVideo(string placementName);
    bool IsRewardedVideoAvailable { get; set; }
}