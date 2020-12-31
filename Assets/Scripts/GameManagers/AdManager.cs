using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsListener {
    public static AdManager Instance;
    #if UNITY_IOS
    string gameID = "3535681";
    #elif UNITY_ANDROID
    string gameID = "3535680";
    #endif
    string rewardVideoID = "rewardedVideo";
    public GameObject[] rewardAdsBtn;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            Advertisement.AddListener(this);
            Advertisement.Initialize(gameID);
            SceneManager.sceneLoaded += OnSceneLoaded;
        } else {
            Destroy(this);
        }
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        rewardAdsBtn = GameObject.FindGameObjectsWithTag("rewardAdBtn");
        if (!Advertisement.IsReady()) { ToggleAdBtns(false); }
    }

    public void ShowVideoAd() {
        if (Advertisement.IsReady()) { Advertisement.Show(); }
    }
    public void ShowRewardVideo() {
        if (Advertisement.IsReady()) { Advertisement.Show(rewardVideoID); }
    }

    public void OnUnityAdsReady(string placementId) {
        if (placementId == rewardVideoID) { ToggleAdBtns(true); }
    }
    public void OnUnityAdsDidStart(string placementId) {}
    public void OnUnityAdsDidError(string message) {}
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult) {
        if (placementId == rewardVideoID) {
            if (showResult == ShowResult.Finished) { GoldManager.Instance.AddGold(25, true); }
            ToggleAdBtns(false);
        }
    }
    void ToggleAdBtns(bool active) {
        foreach (GameObject btn in rewardAdsBtn){
            if (btn) { btn.SetActive(active); }
        }
    }
}
