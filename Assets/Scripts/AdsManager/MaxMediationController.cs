using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MaxMediationController : MonoBehaviour
{
    private int interstitialRetryAttempt;
    public void Init()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
        {
#if !UNITY_EDITOR
            InitializeAOA();
#endif
            InitInterstitialAds();
            //InitRewardedAds();
            //InitBannerAds();
        };
    }
    #region Interstitial Ad
    private void InitInterstitialAds()
    {
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent; //Successful show ads
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialFailedEvent; //
        //MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialFailedToDisplayEvent;
        //MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialDismissedEvent;
        //MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        //MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
    }
    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready for you to show
        interstitialRetryAttempt = 0;
        Debug.Log("Interstitial ad is ready for you to show.");
    }
    private void OnInterstitialFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo error)
    {
        // Interstitial ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
        interstitialRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, interstitialRetryAttempt));

        Invoke("LoadInterstitial", (float)retryDelay);

        Debug.Log("MAX > Interstitial ad failed to load with error code: " + error.Code);
    }
    //private void OnInterstitialFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    //{
    //    // Interstitial ad failed to display. We recommend loading the next ad
    //    LoadInterstitial();
    //    Debug.Log("MAX > Interstitial failed to display with error code: " + errorInfo.Message);
    //}
    //private void OnInterstitialDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    //{
    //    // Interstitial ad is hidden. Pre-load the next ad
    //    LoadInterstitial();
    //    OnFinishWatchFullscreenAds();
    //}
    //private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    //{
    //    AnalyticManager.AppflyerLogAdsClicked(AdsType.Interstitial, adInfo.Placement);
    //}
    //private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    //{
    //    AnalyticManager.AppflyerLogAdsImpression(AdsType.Interstitial, adInfo.Placement);
    //}
    //#endregion
    //private void InitRewardedAds()
    //{
    //    MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
    //    MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
    //    MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
    //    MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
    //    MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
    //    MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
    //    MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
    //}
    //private void InitBannerAds()
    //{

    //}



}
