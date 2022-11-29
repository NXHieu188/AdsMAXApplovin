using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Singleton("AdManager", true)]
public class AdManager : Singleton<AdManager>
{
    public MaxMediationController maxMediation;
    public bool ShowInterstitialAd()
    {
        return true;
    }
    public bool ShowRewardedAds()
    {
        return true;
    }
    public bool ShowBanner()
    {
        return true;
    }
    private void HideBanner()
    {
        
    }
}
