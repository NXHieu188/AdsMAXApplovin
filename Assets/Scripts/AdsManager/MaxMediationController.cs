using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class MaxMediationController : MonoBehaviour
{
    private const string MaxSdkKey = "ENTER_MAX_SDK_KEY_HERE";
    private string bannerAdUnitId = "ENTER_BANNER_AD_UNIT_ID_HERE";
    private string interstitialAdUnitId = "ENTER_INTERSTITIAL_AD_UNIT_ID_HERE";
    private string rewardedAdUnitId = "ENTER_REWARD_AD_UNIT_ID_HERE";
    private string AppOpenAdUnitId = "YOUR_AD_UNIT_ID";
    private int interstitialRetryAttempt;
    public Text txtInterstitialStatus;
    public Text txtRewardedStatus;
    public void Init()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
        {
            Debug.Log("MAX SDK Initialized");
#if !UNITY_EDITOR
            InitAOA();
#endif
            InitInterstitialAds();
            InitRewardedAds();
            InitBannerAds();
#if UNITY_EDITOR
            LoadInterstitial();
            LoadRewardedAd();
#endif
        };

        MaxSdk.SetSdkKey(MaxSdkKey);
        MaxSdk.InitializeSdk();
    }
    void InterstitialReady()
    {
        txtInterstitialStatus.text = "Load Done";
    }
    void RewardedReady()
    {
        txtRewardedStatus.text = "Load Done";
    }


    #region Interstitial Ad
    private void InitInterstitialAds()
    {
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent; //Successful show ads
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialFailedEvent; //
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialFailedToDisplayEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialDismissedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
    }
    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'
        // Reset retry attempt
        interstitialRetryAttempt = 0;
        InterstitialReady();
        Debug.Log("MAX > Interstitial Ad ready.");
    }
    private void OnInterstitialFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo error)
    {
        // Interstitial ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
        interstitialRetryAttempt++;
        var retryDelay = 2 * Math.Min(6, interstitialRetryAttempt);

        Invoke(nameof(LoadInterstitial), retryDelay);

        Debug.Log("MAX > Interstitial ad failed to load with error code: " + error.Code);
    }
    private void OnInterstitialFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. We recommend loading the next ad
        LoadInterstitial();
    }
    private void OnInterstitialDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is hidden. Pre-load the next ad
        LoadInterstitial();
        Debug.Log("MAX > Interstitial dismissed");
    }
    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        //When user click Interstitial
    }
    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        //When Interstitial display
    }
    public bool IsInterstitialLoaded()
    {
        return MaxSdk.IsInterstitialReady(interstitialAdUnitId);
    }
    public void LoadInterstitial()
    {
        txtInterstitialStatus.text = "Loading...";
        MaxSdk.LoadInterstitial(interstitialAdUnitId);
    }
    public void ShowInterstitial(string placement)
    {
        if (IsInterstitialLoaded())
        {
            MaxSdk.ShowInterstitial(interstitialAdUnitId, placement);
        }
    }
    #endregion

    #region Rewarded Ad
    private int rewardedRetryAttempt;
    private void InitRewardedAds()
    {
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
    }
    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready to be shown. MaxSdk.IsRewardedAdReady(rewardedAdUnitId) will now return 'true'
        // Reset retry attempt
        rewardedRetryAttempt = 0;
        RewardedReady();
        Debug.Log("MAX > Rewarded Ad ready.");
    }
    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
        rewardedRetryAttempt++;
        var retryDelay = 2 * Math.Min(6, rewardedRetryAttempt);

        Invoke(nameof(LoadRewardedAd), retryDelay);

        Debug.Log("MAX > Rewarded ad failed to load with error code: " + errorInfo.Code);
    }
    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. We recommend loading the next ad
        LoadRewardedAd();

        Debug.Log("MAX > Rewarded ad failed to display with error code: " + errorInfo.Code);
    }
    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        //When Rewarded display
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        //When user click Rewarded  
    }
    private void OnRewardedAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        LoadRewardedAd();
    }
    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdkBase.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad was displayed and user should receive the reward
        Debug.Log("Rewarded ad received reward");
    }
    public bool IsRewardedAdLoaded()
    {
        return MaxSdk.IsRewardedAdReady(rewardedAdUnitId);
    }
    public void LoadRewardedAd()
    {
        txtRewardedStatus.text = "Loading...";
        MaxSdk.LoadRewardedAd(rewardedAdUnitId);
    }
    public void ShowRewardedAd(string placement)
    {
        if (IsRewardedAdLoaded())
            MaxSdk.ShowRewardedAd(rewardedAdUnitId, placement);
    }
    #endregion
    #region Banner Ad
    private void InitBannerAds()
    {
        MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdFailedEvent;
        MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;

        // Banners are automatically sized to 320x50 on phones and 728x90 on tablets.
        // You may use the utility method `MaxSdkUtils.isTablet()` to help with view sizing adjustments.
        MaxSdk.CreateBanner(bannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);
        MaxSdk.SetBannerExtraParameter(bannerAdUnitId, "adaptive_banner", "true");
        MaxSdk.SetBannerPlacement(bannerAdUnitId, "Banner");
        // Set background or background color for banners to be fully functional.
        MaxSdk.SetBannerBackgroundColor(bannerAdUnitId, Color.clear);

        ShowBanner();
    }
    private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Banner ad is ready to be shown.
        // If you have already called MaxSdk.ShowBanner(BannerAdUnitId) it will automatically be shown on the next ad refresh.
        Debug.Log("Banner ad loaded");
    }

    private void OnBannerAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Banner ad failed to load. MAX will automatically try loading a new ad internally.
        Debug.Log("Banner ad failed to load with error code: " + errorInfo.Code);
    }

    private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Banner ad clicked");
    }
    private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Banner ad revenue paid. Use this callback to track user revenue.
        Debug.Log("Banner ad revenue paid");
    }
    public bool IsBannerAdLoaded()
    {
        return true;
    }
    public void ShowBanner()
    {
        MaxSdk.ShowBanner(bannerAdUnitId);
    }

    public void HideBanner()
    {
        MaxSdk.HideBanner(bannerAdUnitId);
    }
    #endregion

    #region AOA Ad
    private bool isFirstLoad = true;
    private void InitAOA()
    {
        MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += OnAppOpenDismissedEvent;
        MaxSdkCallbacks.AppOpen.OnAdLoadedEvent += OnAppOpenLoadedEvent;
        MaxSdkCallbacks.AppOpen.OnAdLoadFailedEvent += OnAppOpenLoadFailedEvent;
        MaxSdkCallbacks.AppOpen.OnAdDisplayFailedEvent += OnAppOpenFailedToDisplayEvent;
    }
    public void OnAppOpenDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        MaxSdk.LoadAppOpenAd(AppOpenAdUnitId);
    }
    public void OnAppOpenLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Aoa loaded successfully, ID: " + AppOpenAdUnitId);
        if (isFirstLoad)
        {
            isFirstLoad = false;
            ShowAdIfReady();
#if !UNITY_EDITOR
            LoadInterstitial();
            LoadRewardedAd();
#endif
        }
    }

    private void OnAppOpenLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        if (isFirstLoad)
        {
            isFirstLoad = false;
#if !UNITY_EDITOR
            LoadInterstitial();
            LoadRewardedAd();
#endif
        }
        Debug.Log("Load AOA Failed.");
    }
    private void OnAppOpenFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. We recommend loading the next ad
        Debug.Log("AOA ad failed to display with error code: " + errorInfo.Code);

        MaxSdk.LoadAppOpenAd(AppOpenAdUnitId);
    }

    public void ShowAdIfReady()
    {
        if (MaxSdk.IsAppOpenAdReady(AppOpenAdUnitId))
        {
            MaxSdk.ShowAppOpenAd(AppOpenAdUnitId);
        }
        else
        {
            Debug.Log("Load App Open Ad");

            MaxSdk.LoadAppOpenAd(AppOpenAdUnitId);
        }
    }

    #endregion
}
