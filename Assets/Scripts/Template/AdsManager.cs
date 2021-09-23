using System.Collections;
using System.Collections.Generic;
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
    [Header("Editor only")] 
    [SerializeField] private bool rewardedAvailable;
    [SerializeField] private float rewardedFreezeTime = 5;
    #endif
    
    public Action onInterAdShowedOrFailed = delegate { Debug.Log("onInterAdShowedOrFailed"); };
    public Action onRewardedAdRewarded = delegate { Debug.Log("onRewardedAdRewarded"); };
    public Action onRewardedAdFailedOrDiscarded = delegate { Debug.Log("onRewardedAdFailedOrDiscarded"); };

    private bool isInterFreezed = false;
    private Coroutine freezeInterShow;

    private int interRetryAttempt;
    private int bannerRetryAttempt;

    private const string PrefsIsAdsEnabled = "IsAdsEnabled";
     
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        SubscribeToHandleRewardedAd();

        if (AdsEnabled())
        {
            SubscribeToHandleBannerAd();
            SubscribeToHandleInterstitialAd();
        }
        
        InitializeAdsSDK();
    }

    public void DisableInterAndBannerAds()
    {
        PlayerPrefs.SetString(PrefsIsAdsEnabled, "ads is disabled");

        // IronSource.Agent.destroyBanner();
    }

    public bool AdsEnabled()
    {
        return !PlayerPrefs.HasKey(PrefsIsAdsEnabled);
    }

    private void InitializeAdsSDK()
    {
        // IronSource.Agent.shouldTrackNetworkState(true);
        // IronSource.Agent.setAdaptersDebug(true);
        //
        // IronSource.Agent.validateIntegration();

        if (AdsEnabled())
        {
            LoadBanner();
            LoadInterstitial();
        }
    }

    private void LoadInterstitial()
    {
        // IronSource.Agent.loadInterstitial();
    }

    private void LoadBanner()
    {
        // IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
    }
    
    private void SubscribeToHandleInterstitialAd()
    {
        // IronSourceEvents.onInterstitialAdReadyEvent += delegate { interRetryAttempt = 0; };
        // IronSourceEvents.onInterstitialAdLoadFailedEvent += OnInterstitialAdLoadFailedEvent;
        // IronSourceEvents.onInterstitialAdShowFailedEvent += OnInterstitialAdFailedToDisplayEvent;
        // IronSourceEvents.onInterstitialAdClosedEvent += OnInterstitialAdClosedEvent;
    }
    // private void OnInterstitialAdLoadFailedEvent(IronSourceError ironSourceError)
    // {
    //     DebugAdError("Interstitial LOAD failed." +
    //                  $"Description = {ironSourceError.getDescription()}. ErrorCode = {ironSourceError.getErrorCode()}. Code = {ironSourceError.getCode()} ");
    //
    //     interRetryAttempt++;
    //     float retryDelay = (float)Math.Pow(2, Math.Min(6, interRetryAttempt));
    //     DelayHandler.Instance.DelayedCallAsync(retryDelay, LoadInterstitial);
    // }
    // private void OnInterstitialAdFailedToDisplayEvent(IronSourceError ironSourceError)
    // {
    //     DebugAdError("Interstitial SHOW failed." +
    //                  $"Description = {ironSourceError.getDescription()}. ErrorCode = {ironSourceError.getErrorCode()}. Code = {ironSourceError.getCode()} ");
    //     CallInterShownOrFailedAndClearEvents();
    //     LoadInterstitial();
    // }
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
        // IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += OnRewardedVideoAvailabilityChangedEvent;
        // IronSourceEvents.onRewardedVideoAdShowFailedEvent += OnRewardedVideoAdShowFailedEvent;
        // IronSourceEvents.onRewardedVideoAdClosedEvent += OnRewardedVideoAdClosedEvent;
        // IronSourceEvents.onRewardedVideoAdRewardedEvent += OnRewardedVideoAdRewardedEvent;
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
    // private void OnRewardedVideoAdShowFailedEvent(IronSourceError ironSourceError)
    // {
    //     DebugAdError("Rewarded video SHOW failed." +
    //                  $"Description = {ironSourceError.getDescription()}. ErrorCode = {ironSourceError.getErrorCode()}. Code = {ironSourceError.getCode()} ");
    //     
    //     onRewardedAdFailedOrDiscarded?.Invoke();
    //     ClearAdEvents();
    // }
    private void OnRewardedVideoAdClosedEvent()
    {
        onRewardedAdFailedOrDiscarded?.Invoke();
        ClearAdEvents();
    }
    private void OnRewardedVideoAdRewardedEvent()//IronSourcePlacement ironSourcePlacement)
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
        // IronSourceEvents.onBannerAdLoadedEvent += delegate
        // {
        //     IronSource.Agent.displayBanner();
        // };
        // IronSourceEvents.onBannerAdLoadFailedEvent += OnBannerAdLoadFailed;
    }
    // private void OnBannerAdLoadFailed(IronSourceError ironSourceError)
    // {
    //     DebugAdError($"Banner LOAD failed." +
    //                  $"Description = {ironSourceError.getDescription()}. ErrorCode = {ironSourceError.getErrorCode()}. Code = {ironSourceError.getCode()} ");
    //     
    //     IronSource.Agent.hideBanner();
    //     
    //     bannerRetryAttempt++;
    //     float retryDelay = (float)Math.Pow(2, Math.Min(6, bannerRetryAttempt));
    //     DelayHandler.Instance.DelayedCallAsync(retryDelay, LoadBanner);
    // }
    
    private void DebugAdError(string additiveMessage)
    {
        Debug.Log($"{additiveMessage}");
    }

    private void DebugAdSuccess(string additiveMessage)
    {
        Debug.Log($"{additiveMessage}");
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
        
        // if(!IronSource.Agent.isInterstitialReady())
        {
            Debug.Log("INTERSTITIAL AD NOT READY");
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
            // IronSource.Agent.showInterstitial();
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
        // OnRewardedVideoAdRewardedEvent(new IronSourcePlacement("","",0));
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
        // IronSource.Agent.showRewardedVideo(placementName);
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
        return true;
        // return IronSource.Agent.isRewardedVideoAvailable();
    }
}