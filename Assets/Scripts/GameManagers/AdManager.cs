using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsListener {

    public static AdManager Instance;

    public SaveManager saveManager;
    public GoldManager goldManager;

    string gameID = "3535681";
    string bannerPlacementID = "MainMenu";
    string rewardVideoID = "rewardedVideo";
    bool testMode = true;

    public GameObject rewardAdsBtn;

    public int allowAds;
    public Button allowAdsBtn;
    public Button denyAdsBtn;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            Advertisement.AddListener(this);
            Advertisement.Initialize(gameID, testMode);
            SceneManager.sceneLoaded += OnSceneLoaded;
        } else {
            return;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.buildIndex == 0) {
            goldManager = GoldManager.Instance;
            rewardAdsBtn = GameObject.Find("WatchAdForMoney");
        }

        allowAds = PlayerPrefs.GetInt("AllowAds", 1);
        AllowAds(allowAds);
    }

    public void AllowAds(int allow) {
        allowAds = allow;
        
        if (allow == 1) {
            allowAdsBtn.interactable = false;
            denyAdsBtn.interactable = true;
            
            if (rewardAdsBtn)
                rewardAdsBtn.SetActive(true);

            ShowBanner();
        } else {
            allowAdsBtn.interactable = true;
            denyAdsBtn.interactable = false;

            if (rewardAdsBtn)
                rewardAdsBtn.SetActive(false);
            
            HideBanner();
        }
        
        saveManager.SaveIntData("AllowAds", allow);
    }

    public void HideBanner() {
        Advertisement.Banner.Hide();
    }

    public void ShowBanner() {
        if (allowAds == 1)
            StartCoroutine(ShowBannerWhenReady());
    }

    public void ShowRewardVideo() {
        StartCoroutine(ShowRewardVideoWhenReady());
    }

    IEnumerator ShowBannerWhenReady() {
        while (!Advertisement.IsReady(bannerPlacementID)) {
            yield return new WaitForSeconds(0.5f);
        }

        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show(bannerPlacementID);
    }

    IEnumerator ShowRewardVideoWhenReady() {
        while (!Advertisement.IsReady(rewardVideoID)) {
            yield return new WaitForSeconds(0.5f);
        }
        HideBanner();
        Advertisement.Show(rewardVideoID);
    }

    public void OnUnityAdsReady(string placementId) {
    }

    public void OnUnityAdsDidStart(string placementId) {
    }

    public void OnUnityAdsDidError(string message) {
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult) {
        // Reward the user for watching the ad if watched to completion.
        if (showResult == ShowResult.Finished) {
            goldManager.AddGold(25, true);
        }

        ShowBanner();
    }
}
