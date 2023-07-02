using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsListener{

    public static AdManager instance;

    private void Awake() {
        if (instance == null)
            instance = this;
        else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private string playStoreID = "3767173";
    private string appStoreID = "3767172";

    private string interstitialAd = "video";
    private string rewardedVideoAd = "rewardedVideo";

    public bool isTargetPlayStore;
    public bool isTestAd;
    [HideInInspector] public string endGameType = null;

    private void Start() {
        Advertisement.AddListener(this);
        InitializeAdvertisement();
    }
    private void InitializeAdvertisement() {
        if (isTargetPlayStore) {
            Advertisement.Initialize(playStoreID, isTestAd);
            return;
        }
        Advertisement.Initialize(appStoreID, isTestAd);
    }
    
    public void PlayInterstitialAd() {
        if (!Advertisement.IsReady(interstitialAd)) { return; }
        Advertisement.Show(interstitialAd);        
    }

    public void OnUnityAdsReady(string placementId) {
        //throw new NotImplementedException();
    }

    public void OnUnityAdsDidError(string message) {
        //throw new NotImplementedException();
    }

    public void OnUnityAdsDidStart(string placementId) {
        //throw new NotImplementedException();
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult) {
        //throw new NotImplementedException();
        FindObjectOfType<AudioManager>().SetPause("Background Music", false);
        FindObjectOfType<AudioManager>().SetVolume("Background Music", 0.2f);        
        switch (endGameType) {
            case "Win":
                FindObjectOfType<StatusHandler>().WinStatus();
                break;
            case "Game Over":
                FindObjectOfType<StatusHandler>().GameOverStatus();
                break;
            case "Time Out":
                FindObjectOfType<StatusHandler>().TimeOutStatus();
                break;
        }
    }
}
