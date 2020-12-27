using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
            Advertisement.Initialize(gameID, false);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.buildIndex == 0) {
            rewardAdsBtn = GameObject.FindGameObjectsWithTag("rewardAdBtn");
            foreach (GameObject btn in rewardAdsBtn){
                btn.SetActive(false);
            }
        }
    }

    public void ShowVideoAd() {
        if (Advertisement.IsReady()) {
            Advertisement.Show();
        }
    }

    public void ShowRewardVideo() {
        Advertisement.Show(rewardVideoID);
    }

    public void OnUnityAdsReady(string placementId) {
        if (placementId == rewardVideoID) {
            foreach (GameObject btn in rewardAdsBtn){
                if (btn) { btn.SetActive(true); }
            }
        }
    }
    public void OnUnityAdsDidStart(string placementId) {}
    public void OnUnityAdsDidError(string message) {}
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult) {
        if (placementId == rewardVideoID) {
            if (showResult == ShowResult.Finished) { GoldManager.Instance.AddGold(25, true); }

            foreach (GameObject btn in rewardAdsBtn){
                btn.SetActive(false);
            }
        }
    }
}
