using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HomeScreen : MonoBehaviour
{
    public Button btnShowInterstitial;
    public Button btnShowRewarded;
    public Button btnShowBanner;
    private bool isBannerShowing = true;
    public Text txtInterstitialStatus;
    public Text txtRewardedStatus;
    void Start()
    {
        btnShowInterstitial.onClick.AddListener(ShowInterstitial);
        btnShowRewarded.onClick.AddListener(ShowRewarded);
        btnShowBanner.onClick.AddListener(ShowBanner);
    }

    void ShowInterstitial()
    {
        if (AdManager.Instance.ShowInterstitialAd("HomeScene"))
        {
            txtInterstitialStatus.text = "Showing";
        }
        else
        {
            txtInterstitialStatus.text = "Ad not Ready";
        }
    }
    void ShowRewarded()
    {
        if (AdManager.Instance.ShowRewardedAds("HomeScene"))
        {
            txtRewardedStatus.text = "Showing";
        }
        else
        {
            txtRewardedStatus.text = "Ad not Ready";
        }
    }
    void ShowBanner()
    {
        if (!isBannerShowing)
        {
            AdManager.Instance.ShowBanner();
            btnShowBanner.GetComponentInChildren<Text>().text = "Hide Banner";
        }
        else
        {
            AdManager.Instance.HideBanner();
            btnShowBanner.GetComponentInChildren<Text>().text = "Show Banner";
        }
        isBannerShowing = !isBannerShowing;
    }
}
