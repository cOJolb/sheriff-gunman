using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class GoogleMobileAdTest : MonoBehaviour
{
    public Button interstitialButton;
    public Button rewardButton;
    public Text reward;

    public static readonly string interstitial1Id = "ca-app-pub-1195551850458243/2630472191";
    public static readonly string reward1Id = "ca-app-pub-1195551850458243/9004308858";

    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;
    public static GoogleMobileAdTest instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        //interstitialButton.interactable = false;
        //rewardButton.interactable = false;
    }


    public void OnClickInit()
    {
        List<string> deviceIds = new List<string>();
        

        deviceIds.Add("04C5EA7CAF59424C20D9A9B6EAE9241C");
        RequestConfiguration requestConfiguration = new RequestConfiguration
            .Builder()
            .SetTestDeviceIds(deviceIds)
            .build();
        MobileAds.SetRequestConfiguration(requestConfiguration);
        MobileAds.Initialize(initStatus => { });
    }

    public void OnClickRequestInterstitial()
    {
        if (interstitial != null)
        {
            interstitial.Destroy();
        }
        interstitial = new InterstitialAd(interstitial1Id);
        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        this.interstitial.OnAdOpening += HandleOnAdOpened;

        AdRequest request = new AdRequest.Builder().Build();
        interstitial.LoadAd(request);
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
        interstitialButton.interactable = true;
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
        interstitialButton.interactable = false;
    }

    public void OnClickInterstitial()
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }
    }
    public void OnClickRequestReward()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
        }
        this.rewardedAd = new RewardedAd(reward1Id);
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;

        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        AdRequest request = new AdRequest.Builder().Build();
        this.rewardedAd.LoadAd(request);
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
        rewardButton.interactable = true;
        reward.text = "리워드 광고 출력";
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
        rewardButton.interactable = false;
    }

    public void OnClickReward()
    {
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        reward.text = "received for " + amount.ToString() + " " + type;
    }
}