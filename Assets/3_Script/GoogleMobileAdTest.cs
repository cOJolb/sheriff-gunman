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

    private static InterstitialAd interstitial;
    private static RewardedAd retryAd;
    private static RewardedAd goodsAd;
    public static GoogleMobileAdTest instance;
    public static bool isClosed;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        //interstitialButton.interactable = false;
        //rewardButton.interactable = false;
    }
    public static void Init()
    {
        List<string> deviceIds = new List<string>();

        deviceIds.Add("04C5EA7CAF59424C20D9A9B6EAE9241C");
        RequestConfiguration requestConfiguration = new RequestConfiguration
            .Builder()
            .SetTestDeviceIds(deviceIds)
            .build();
        MobileAds.SetRequestConfiguration(requestConfiguration);
        MobileAds.Initialize(initStatus => {
            RequestInterstitial();
            RequestRetryAd();
            RequestGoodsAd();
        });
    }

    public static void OnClickInterstitial()
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }
    }
    public static void OnClickRetry()
    {
        if (retryAd.IsLoaded())
        {
            retryAd.Show();
        }
    }
    public static void OnClickGoods()
    {
        if (goodsAd.IsLoaded())
        {
            goodsAd.Show();
        }
    }
    public static void RequestInterstitial()
    {
        if (interstitial != null)
        {
            interstitial.Destroy();
        }
        interstitial = new InterstitialAd(interstitial1Id);
        interstitial.OnAdLoaded += HandleOnAdLoaded;
        interstitial.OnAdOpening += HandleOnAdOpened;
        interstitial.OnAdClosed += HandleOnAdClosed;
        interstitial.OnAdFailedToLoad += OnAdFailedToLoad;
        AdRequest request = new AdRequest.Builder().Build();
        interstitial.LoadAd(request);
    }
    public static void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
        //interstitialButton.interactable = true;
    }

    public static void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
        //interstitialButton.interactable = false;
    }
    public static void HandleOnAdClosed(object sender, EventArgs args)
    {
        isClosed = true;
        RequestInterstitial();
    }
    public static void OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e) { }

    public static void RequestRetryAd()
    {
        if (retryAd != null)
        {
            retryAd.Destroy();
        }
        retryAd = new RewardedAd(reward1Id);
        //retryAd.OnAdLoaded += HandleRewardedAdLoaded;
        //retryAd.OnAdOpening += HandleRewardedAdOpening;
        retryAd.OnAdClosed += ClosedAdRetry;
        retryAd.OnAdFailedToLoad += FailedToRoadAdRetry;
        retryAd.OnUserEarnedReward += HandleUserEarnedReward;
        AdRequest request = new AdRequest.Builder().Build();
        retryAd.LoadAd(request);
    }
    public static void ClosedAdRetry(object sender, EventArgs args)
    {
        GameManager.instance.state = GameManager.GameState.ReStart;
        RequestRetryAd();
    }
    public static void FailedToRoadAdRetry(object sender, AdFailedToLoadEventArgs e) 
    {
        GameManager.instance.state = GameManager.GameState.ReStart;
    }
    public static void RequestGoodsAd()
    {
        if (goodsAd != null)
        {
            goodsAd.Destroy();
        }
        goodsAd = new RewardedAd(reward1Id);
        //retryAd.OnAdLoaded += HandleRewardedAdLoaded;
        //retryAd.OnAdOpening += HandleRewardedAdOpening;
        goodsAd.OnAdClosed += ClosedAdGoods;
        goodsAd.OnAdFailedToLoad += FailedToRoadAdGoods;
        goodsAd.OnUserEarnedReward += HandleUserEarnedReward;
        AdRequest request = new AdRequest.Builder().Build();
        goodsAd.LoadAd(request);
    }
    public static void ClosedAdGoods(object sender, EventArgs args)
    {
        GameManager.instance.totalEnemyCatch += 1;
        RequestGoodsAd();
    }
    public static void FailedToRoadAdGoods(object sender, AdFailedToLoadEventArgs e)
    {
    }

    //public static void HandleRewardedAdLoaded(object sender, EventArgs args)
    //{
    //    MonoBehaviour.print("HandleRewardedAdLoaded event received");
    //    //rewardButton.interactable = true;
    //    //reward.text = "리워드 광고 출력";
    //}

    //public static void HandleRewardedAdOpening(object sender, EventArgs args)
    //{
    //    MonoBehaviour.print("HandleRewardedAdOpening event received");
    //    //rewardButton.interactable = false;
    //}

    public static void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        //reward.text = "received for " + amount.ToString() + " " + type;
    }
}