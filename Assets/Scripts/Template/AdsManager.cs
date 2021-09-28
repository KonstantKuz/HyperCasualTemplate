using System.Collections;
using UnityEngine;
using System;

public enum RewardedVideoPlacement
{
    LevelFinish,
    Shop_Coins,
}

public class AdsManager : Singleton<AdsManager>
{
    [SerializeField] private float interFreezeTime;
    [SerializeField] private int minLevelToShowInter;

#if UNITY_EDITOR
    [Header("Editor only")] [SerializeField]
    private bool rewardedAvailable;

    [SerializeField] private float rewardedFreezeTime = 5;
#endif

    public Action onInterAdShowedOrFailed = delegate { Debug.Log("onInterAdShowedOrFailed"); };

    public Action onRewardedAdFailedOrDiscarded = delegate { Debug.Log("onRewardedAdFailedOrDiscarded"); };
    public Action onRewardedAdRewarded = delegate { Debug.Log("onRewardedAdRewarded"); };

    private bool isInterFreezed = false;
    private Coroutine freezeInterShow;

    private int interRetryAttempt;
    private int bannerRetryAttempt;

    private const string PrefsIsAdsEnabled = "IsAdsEnabled";

    private readonly IAdsImpl _impl = new AdsDummyImpl();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        SubscribeToHandleRewardedAd();

        if (AdsEnabled())
        {
            SubscribeToHandleBannerAd();
            SubscribeToHandleInterstitialAd();
        }

        InitializeAds();
    }

    public bool AdsEnabled()
    {
        return !PlayerPrefs.HasKey(PrefsIsAdsEnabled);
    }

    public void DisableInterAndBannerAds()
    {
        PlayerPrefs.SetString(PrefsIsAdsEnabled, "ads is disabled");
        _impl.DestroyBanner();
    }

    private void InitializeAds()
    {
        _impl.Initialize();

        if (AdsEnabled())
        {
            LoadBanner();
            LoadInterstitial();
        }
    }

    private void LoadBanner()
    {
        _impl.LoadBanner();
    }

    private void LoadInterstitial()
    {
        _impl.LoadInterstitial();
    }

    private void SubscribeToHandleInterstitialAd()
    {
        _impl.SubscribeToHandleInterstitialAd(delegate { interRetryAttempt = 0; },
            OnInterstitialAdLoadFailedEvent,
            OnInterstitialAdFailedToDisplayEvent,
            OnInterstitialAdClosedEvent);
    }

    private void OnInterstitialAdLoadFailedEvent(string error)
    {
        DebugAdError("Interstitial LOAD failed." +
                     $"Description = {error} ");

        interRetryAttempt++;
        float retryDelay = (float) Math.Pow(2, Math.Min(6, interRetryAttempt));
        DelayHandler.Instance.DelayedCallAsync(retryDelay, LoadInterstitial);
    }

    private void OnInterstitialAdFailedToDisplayEvent(string error)
    {
        DebugAdError("Interstitial SHOW failed." +
                     $"Description = {error} ");
        CallInterShownOrFailedAndClearEvents();
        LoadInterstitial();
    }

    private void OnInterstitialAdClosedEvent()
    {
        interRetryAttempt = 0;
        DebugAdSuccess("Interstitial ad showed.");
        CallInterShownOrFailedAndClearEvents();
        LoadInterstitial();
        FreezeInterShow();
    }

    private void CallInterShownOrFailedAndClearEvents()
    {
        onInterAdShowedOrFailed?.Invoke();
        ClearAdEvents();
    }

    private void SubscribeToHandleRewardedAd()
    {
        _impl.SubscribeToHandleRewardedAd(
            OnRewardedVideoAvailabilityChangedEvent,
            OnRewardedVideoAdShowFailedEvent,
            OnRewardedVideoAdClosedEvent,
            OnRewardedVideoAdRewardedEvent);
    }

    private void OnRewardedVideoAvailabilityChangedEvent(bool available)
    {
        if (available)
        {
            DebugAdSuccess("Rewarded ad available.");
        }
        else
        {
            DebugAdError("Rewarded ad not available");
        }
    }

    private void OnRewardedVideoAdShowFailedEvent(string error)
    {
        DebugAdError("Rewarded video SHOW failed." +
                     $"Description = {error} ");

        onRewardedAdFailedOrDiscarded?.Invoke();
        ClearAdEvents();
    }

    private void OnRewardedVideoAdClosedEvent()
    {
        onRewardedAdFailedOrDiscarded?.Invoke();
        ClearAdEvents();
    }

    private void OnRewardedVideoAdRewardedEvent()
    {
        DebugAdSuccess("Rewarded ad rewarded.");
        onRewardedAdRewarded?.Invoke();
        ClearAdEvents();
        FreezeInterShow();
    }

    private void FreezeInterShow()
    {
        isInterFreezed = true;
        if (freezeInterShow != null)
        {
            StopCoroutine(freezeInterShow);
        }

        freezeInterShow = StartCoroutine(FreezeInterShow());

        IEnumerator FreezeInterShow()
        {
            yield return new WaitForSecondsRealtime(interFreezeTime);
            isInterFreezed = false;
            freezeInterShow = null;
        }
    }

    private void SubscribeToHandleBannerAd()
    {
        _impl.SubscribeToHandleBannerAd(OnBannerAdLoadFailed);
    }

    private void OnBannerAdLoadFailed(string error)
    {
        DebugAdError($"Banner LOAD failed." +
                     $"Description = {error} ");

        bannerRetryAttempt++;
        float retryDelay = (float) Math.Pow(2, Math.Min(6, bannerRetryAttempt));
        DelayHandler.Instance.DelayedCallAsync(retryDelay, LoadBanner);
    }

    private void DebugAdError(string message)
    {
        Debug.Log($"{message}");
    }

    private void DebugAdSuccess(string message)
    {
        Debug.Log($"{message}");
    }

    public void ShowInterstitialAd()
    {
#if UNITY_EDITOR
        if (AdsEnabled() && !isInterFreezed)
        {
            Debug.Log("SHOW INTERSTITIAL AD");
            OnInterstitialAdClosedEvent();
        }
        else
        {
            if (isInterFreezed)
            {
                Debug.Log($"INTERSTITIAL AD FREEZED (freeze time == {interFreezeTime})");
            }

            if (!AdsEnabled())
            {
                Debug.Log($"INTERSTITIAL AD DISABLED");
            }

            CallInterShownOrFailedAndClearEvents();
        }

        return;
#endif

        if (!_impl.IsInterstitialAvailable())
        {
            Debug.Log("INTERSTITIAL AD NOT AVAILABLE");
            CallInterShownOrFailedAndClearEvents();
            LoadInterstitial();
            return;
        }

        if (LevelManager.Instance.CurrentDisplayLevelNumber < minLevelToShowInter)
        {
            Debug.Log("CAN NOT SHOW INTERSTITIAL AD");
            CallInterShownOrFailedAndClearEvents();
            LoadInterstitial();
            return;
        }

        if (AdsEnabled() && !isInterFreezed)
        {
            Debug.Log("SHOW INTERSTITIAL AD");
            _impl.ShowInterstitial();
        }
        else
        {
            if (isInterFreezed)
            {
                Debug.Log($"INTERSTITIAL AD FREEZED (freeze time == {interFreezeTime})");
            }

            if (!AdsEnabled())
            {
                Debug.Log($"INTERSTITIAL AD DISABLED");
            }

            CallInterShownOrFailedAndClearEvents();
        }
    }

    public void ShowRewardedAd(string placementName)
    {
#if UNITY_EDITOR
        Debug.Log("SHOW REWARDED VIDEO AD");
        OnRewardedVideoAdRewardedEvent();
        FreezeRewardedShow();
        return;
#endif

        if (!IsRewardedReady())
        {
            Debug.Log("REWARDED VIDEO AD NOT READY");
            onRewardedAdFailedOrDiscarded?.Invoke();
            ClearAdEvents();
            return;
        }

        Debug.Log("SHOW REWARDED VIDEO AD");
        _impl.ShowRewardedVideo(placementName);
    }

#if UNITY_EDITOR
    private void FreezeRewardedShow()
    {
        if (!rewardedAvailable)
        {
            return;
        }

        StartCoroutine(FreezeRewardedShow());

        IEnumerator FreezeRewardedShow()
        {
            rewardedAvailable = false;
            yield return new WaitForSecondsRealtime(rewardedFreezeTime);
            rewardedAvailable = true;
        }
    }
#endif

    public void ClearAdEvents()
    {
        onRewardedAdRewarded = null;
        onRewardedAdFailedOrDiscarded = null;
        onInterAdShowedOrFailed = null;
    }

    public bool IsRewardedReady()
    {
#if UNITY_EDITOR
        return rewardedAvailable;
#endif
        return _impl.IsRewardedVideoAvailable();
    }
}