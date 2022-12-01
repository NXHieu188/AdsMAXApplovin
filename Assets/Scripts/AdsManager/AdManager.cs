using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AdManager : MonoBehaviour
{
    public static AdManager Instance { get; set; }
    public MaxMediationController maxMediation;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
    private void Start()
    {
        maxMediation.Init();
    }
    public bool ShowInterstitialAd(string placement)
    {
        if (!maxMediation.IsInterstitialLoaded())
        {
            return false;
        }
        maxMediation.ShowInterstitial(placement);
        return true;
    }
    public bool ShowRewardedAds(string placement)
    {
        if (!maxMediation.IsRewardedAdLoaded())
        {
            return false;
        }
        maxMediation.ShowRewardedAd(placement);
        return true;
    }
    public bool ShowBanner()
    {
        if (!maxMediation.IsBannerAdLoaded())
        {
            return false;
        }
        maxMediation.ShowBanner();
        return true;
    }
    public void HideBanner()
    {
        maxMediation.HideBanner();
    }
    public void OnApplicationPause(bool paused)
    {
        Debug.Log("OnApplicationPause: " + paused);
        if (!paused)
        {
#if !UNITY_EDITOR
            maxMediation.ShowAdIfReady();
#endif
        }
    }
}
