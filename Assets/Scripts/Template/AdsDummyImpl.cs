using System;

public class AdsDummyImpl: IAdsImpl
{
    public void Initialize()
    {
    }

    public void LoadInterstitial()
    {
    }

    public void LoadBanner()
    {
    }

    public void ShowBanner()
    {
    }

    public void HideBanner()
    {
    }

    public void DestroyBanner()
    {
    }

    public void SubscribeToHandleInterstitialAd(Action onInterstitialAdReadyEvent, Action<string> onInterstitialAdLoadFailedEvent,
        Action<string> onInterstitialAdFailedToDisplayEvent, Action onInterstitialAdClosedEvent)
    {
    }

    public void SubscribeToHandleRewardedAd(Action<bool> onRewardedVideoAvailabilityChangedEvent,
        Action<string> onRewardedVideoAdShowFailedEvent, Action onRewardedVideoAdClosedEvent,
        Action onRewardedVideoAdRewardedEvent)
    {
    }

    public void SubscribeToHandleBannerAd(Action<string> onBannerLoadFailedEvent)
    {
    }

    public bool IsInterstitialAvailable()
    {
        return true;
    }

    public void ShowInterstitial()
    {
    }

    public bool IsRewardedVideoAvailable()
    {
        return true;
    }

    public void ShowRewardedVideo(string placementName)
    {
    }
}