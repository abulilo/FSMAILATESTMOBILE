using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using admob;


public class Admanager : MonoBehaviour
{
    public string bannerID;
    public string VideoID;
    public string RewardVideoID;
    public string bannerIDIOS;
    public string VideoIDIOS;
    public string RewardVideoIDIos;



    public static Admanager Instance { get; set; }
    // Use this for initialization
    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
#if UNITY_EDITOR
        Debug.Log("i wont run under unity Console");
        //Admob.Instance().initAdmob(bannerID, VideoID);

#elif UNITY_ANDROID

        Admob.Instance().initAdmob(bannerID, VideoID);
        //Admob.Instance().initAdmob(RewardVideoID,VideoID);
        Admob.Instance().loadInterstitial();
        ShowBanner();
        Admob.Instance().loadRewardedVideo(RewardVideoID);
#elif UNITY_IOS
         Admob.Instance().initAdmob(bannerIDIOS, VideoIDIOS);
       // Admob.Instance().initAdmob(RewardVideoIDIos,VideoIDIOS);
        Admob.Instance().loadInterstitial();
        ShowBanner();
        Admob.Instance().loadRewardedVideo(RewardVideoID);
#endif
    }

    public void ShowBanner()
    {
#if UNITY_EDITOR
        Debug.Log("cannotplayInterstitial bcoz of Unity Engine");
#elif UNITY_ANDROID

        Admob.Instance().showBannerAbsolute(AdSize.Banner,AdPosition.TOP_CENTER,5);
        Admob.Instance().loadRewardedVideo(RewardVideoID);
#elif UNITY_IOS
        Admob.Instance().showBannerAbsolute(AdSize.Banner,AdPosition.TOP_CENTER,5);
        Admob.Instance().loadRewardedVideo(RewardVideoID);
#endif

    }

    public void HideBanner()
    {
#if UNITY_EDITOR
        Debug.Log("Hiding banners bcoz of Unity Engine");

#elif UNITY_ANDROID

        Admob.Instance().removeBanner();
#elif UNITY_IOS
        Admob.Instance().removeBanner();
#endif

    }
    public void ShowVideo()
    {
#if UNITY_EDITOR


#elif UNITY_ANDROID
        if (Admob.Instance().isInterstitialReady())
        {
            Admob.Instance().showInterstitial();
        }
        else {Admob.Instance().loadInterstitial();}
        Admob.Instance().loadInterstitial();
#elif UNITY_IOS
        if (Admob.Instance().isInterstitialReady())
        {
            Admob.Instance().showInterstitial();
        }
        else {Admob.Instance().loadInterstitial();}
        Admob.Instance().loadInterstitial();

#endif
    }



    public void ShowRewardedvideo()
    {
#if UNITY_EDITOR

#elif UNITY_ANDROID
    
        if (Admob.Instance().isRewardedVideoReady())
        {
         Admob.Instance().showRewardedVideo();
        }
        else
        Admob.Instance().loadRewardedVideo(RewardVideoID);

#elif UNITY_IOS

        if (Admob.Instance().isRewardedVideoReady())
        {
         Admob.Instance().showRewardedVideo();
        }
        else
        Admob.Instance().loadRewardedVideo(RewardVideoID);

#endif
    }







}