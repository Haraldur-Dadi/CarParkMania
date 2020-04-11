using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsListener {

    public static AdManager Instance;

    public GoldManager goldManager;
    public SaveManager saveManager;
    public CrossSceneManager crossSceneManager;

    string gameID = "3535681";
    string bannerPlacementID = "MainMenu";
    string rewardVideoID = "rewardedVideo";
    bool testMode = true;

    public GameObject rewardAdsBtn;

    public int allowAds;
    public Toggle allowAdsToggle;
    public Toggle denyAdsToggle;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        } else {
            return;
        }

        saveManager = GetComponent<SaveManager>();
        crossSceneManager = GetComponent<CrossSceneManager>();

        Advertisement.AddListener(this);
        Advertisement.Initialize(gameID, testMode);

        allowAds = PlayerPrefs.GetInt("AllowAds", 1);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.buildIndex == 0) {
            goldManager = GoldManager.Instance;
            rewardAdsBtn = GameObject.Find("WatchAdForMoney");
        }

        ShowBanner();

        Toggle[] settingsToggler = crossSceneManager.settingsUI.GetComponentsInChildren<Toggle>();

        allowAdsToggle = settingsToggler[4];
        denyAdsToggle = settingsToggler[5];

        allowAdsToggle.onValueChanged.AddListener(delegate { AllowAds(allowAdsToggle, 1); });
        denyAdsToggle.onValueChanged.AddListener(delegate { AllowAds(denyAdsToggle, 0); });

        ToggleAds();
    }

    public void AllowAds(Toggle toggler, int allow) {
        if (allow == allowAds) {
            if (allowAds == 1) {
                allowAdsToggle.isOn = true;
            } else {
                denyAdsToggle.isOn = true;
            }
        } else if (toggler.isOn) {
            allowAds = allow;
            ToggleAds();
            saveManager.SaveIntData("AllowAds", allow);
        }
    }

    public void ToggleAds() {
        if (allowAds == 1) {
            denyAdsToggle.isOn = false;
            if (rewardAdsBtn){
                rewardAdsBtn.SetActive(true);
            }
            ShowBanner();
        } else {
            allowAdsToggle.isOn = false;
            if (rewardAdsBtn){
                rewardAdsBtn.SetActive(false);
            }
            HideBanner();
        }
    }

    public void HideBanner() {
        Advertisement.Banner.Hide();
    }

    public void ShowBanner() {
        if (allowAds == 1) {
            StartCoroutine(ShowBannerWhenReady());
        }
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
