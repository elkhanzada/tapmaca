using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance { set; get; }
    public string bannerId = "";
    public string videoId;
    InterstitialAd interstital;
    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
#if UNITY_EDITOR
#elif UNITY_ANDROID
    RequestInterstitial();    

#endif

    }
    public void RequestInterstitial(){
		#if UNITY_EDITOR
		#elif UNITY_ANDROID
		interstital = new InterstitialAd(videoId);
		AdRequest request = new AdRequest.Builder().Build();
        interstital.LoadAd(request);
		#endif
    }

    public void ShowVideo()
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        if (interstital.IsLoaded())
        {
            interstital.Show();
        }
#endif


    }
}

