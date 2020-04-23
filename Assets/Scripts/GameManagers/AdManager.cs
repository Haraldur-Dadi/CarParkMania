using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsListener {

    public static AdManager Instance;
    public SaveManager saveManager;
    
    #if UNITY_IOS
    string gameID = "3535681";
    #elif UNITY_ANDROID
    string gameID = "3535680";
    #endif
    string videoID = "video";

    public GameObject rewardAdsBtn;
    public int allowAds;
    public Button allowAdsBtn;
    public Button denyAdsBtn;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            Advertisement.AddListener(this);
            Advertisement.Initialize(gameID, true);
            SceneManager.sceneLoaded += OnSceneLoaded;
        } else {
            return;
        }

        AllowAds(PlayerPrefs.GetInt("AllowAds", 1));
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.buildIndex == 0) {
            rewardAdsBtn = GameObject.Find("WatchAdForMoney");
        }
    }

    public void AllowAds(int allow) {
        allowAds = allow;
        
        if (allow == 1) {
            allowAdsBtn.interactable = false;
            denyAdsBtn.interactable = true;
        } else {
            allowAdsBtn.interactable = true;
            denyAdsBtn.interactable = false;
        }
        
        saveManager.SaveIntData("AllowAds", allow);
    }

    public void ShowVideoAd() {
        Advertisement.Show();
    }

    public void ShowRewardVideo() {
        Advertisement.Show(rewardVideoID);
    }

    public void OnUnityAdsReady(string placementId) {
        if (placementId == rewardVideoID) {
            if (rewardAdsBtn) {
                rewardAdsBtn.SetActive(true);
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

            rewardAdsBtn.SetActive(false);
        }
    }
}
