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
    public Button allowAdsBtn;
    public Button denyAdsBtn;
    public GameObject allowAdsPrompt;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            Advertisement.AddListener(this);
            Advertisement.Initialize(gameID, false);
            AllowAds(PlayerPrefs.GetInt("AllowAds", 1));
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.buildIndex == 0) {
            rewardAdsBtn = GameObject.FindGameObjectsWithTag("rewardAdBtn");
            foreach (GameObject btn in rewardAdsBtn){
                btn.SetActive(true);
            }
        }
    }

    public void AllowAds(int allow) {
        if (allow == 1) {
            allowAdsBtn.interactable = false;
            denyAdsBtn.interactable = true;
            allowAdsPrompt.SetActive(true);
        } else {
            allowAdsBtn.interactable = true;
            denyAdsBtn.interactable = false;
            allowAdsPrompt.SetActive(false);
        }
        
        SaveManager.Instance.SaveIntData("AllowAds", allow);
    }

    public void ShowVideoAd() {
        if (Advertisement.IsReady())
            Advertisement.Show();
    }

    public void ShowRewardVideo() {
        Advertisement.Show(rewardVideoID);
    }

    public void OnUnityAdsReady(string placementId) {
        if (placementId == rewardVideoID) {
            if (rewardAdsBtn.Length > 0) {
                foreach (GameObject btn in rewardAdsBtn){
                    if (btn)
                        btn.SetActive(true);
                }
            }
        }
    }
    public void OnUnityAdsDidStart(string placementId) {
    }
    public void OnUnityAdsDidError(string message) {
    }
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult) {
        // Reward the user for watching the ad if watched to completion.
        if (placementId == rewardVideoID) {
            if (showResult == ShowResult.Finished) {
                GoldManager.Instance.AddGold(25, true);
            }

            foreach (GameObject btn in rewardAdsBtn){
                btn.SetActive(false);
            }
        }
    }
}
