//using UnityEngine;
//using UnityEngine.Advertisements;

//public class UnityAdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
//{
//    [SerializeField] string _androidGameId;
//    [SerializeField] string _iOSGameId;
//    [SerializeField] bool _testMode = true;
//    private string _gameId;

//    [SerializeField] string _androidAdUnitId = "Interstitial_Android";
//    [SerializeField] string _iOsAdUnitId = "Interstitial_iOS";
//    string _adUnitId;
//    public static UnityAdsManager Instance { set; get; }
//    void Awake()
//    {
//        Instance = this;
//        DontDestroyOnLoad(gameObject);
//        InitializeAds();
//    }

//    public void InitializeAds()
//    {
//        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
//            ? _iOSGameId
//            : _androidGameId;
//        Advertisement.Initialize(_gameId, _testMode, this);
//        _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
//            ? _iOsAdUnitId
//            : _androidAdUnitId;
//    }
//    public void OnInitializationComplete()
//    {
//    }

//    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
//    {
//        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
//    }

//    // Load content to the Ad Unit:
//    public void LoadAd()
//    {
//        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
//        Advertisement.Load(_adUnitId, this);
//    }

//    // Show the loaded content in the Ad Unit:
//    public void ShowAd()
//    {
//        // Note that if the ad content wasn't previously loaded, this method will fail
//        Advertisement.Show(_adUnitId, this);
//    }

//    // Implement Load Listener and Show Listener interface methods: 
//    public void OnUnityAdsAdLoaded(string adUnitId)
//    {
//        // Optionally execute code if the Ad Unit successfully loads content.
//    }

//    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
//    {
//        Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
//        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
//    }

//    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
//    {
//        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
//        // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
//    }

//    public void OnUnityAdsShowStart(string adUnitId) { }
//    public void OnUnityAdsShowClick(string adUnitId) { }
//    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState) { }
//}