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

    public GameObject rewardBtn;

    public int allowAds;
    public Toggle allowAdsToggle;
    public Toggle denyAdsToggle;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }

        goldManager = GoldManager.Instance;
        saveManager = SaveManager.Instance;
        crossSceneManager = CrossSceneManager.Instance;

        Advertisement.AddListener(this);
        Advertisement.Initialize(gameID, testMode);

        allowAds = PlayerPrefs.GetInt("AllowAds", 1);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        ShowBanner();

        Toggle[] settingsToggler = crossSceneManager.settingsUI.GetComponentsInChildren<Toggle>();

        allowAdsToggle = settingsToggler[4];
        denyAdsToggle = settingsToggler[5];

        allowAdsToggle.onValueChanged.AddListener(delegate { AllowAds(allowAdsToggle, 1); });
        denyAdsToggle.onValueChanged.AddListener(delegate { AllowAds(denyAdsToggle, 0); });

        if (allowAds == 1) {
            allowAdsToggle.isOn = true;
        } else {
            denyAdsToggle.isOn = true;
        }
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

            if (allowAds == 1) {
                denyAdsToggle.isOn = false;
            } else {
                allowAdsToggle.isOn = false;
            }

            saveManager.SaveIntData("AllowAds", allow);
        }
    }

    public void HideBanner() {
        Advertisement.Banner.Hide();
    }

    public void ShowBanner() {
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
        // If the reward ad Placement is ready, activate the button: 
        if (placementId == rewardVideoID) {
            if (rewardBtn)
                rewardBtn.SetActive(true);
        }
    }

    public void OnUnityAdsDidStart(string placementId) {
        // Optional actions to take when the end-users triggers an ad.
    }

    public void OnUnityAdsDidError(string message) {
        // Log the error
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult) {
        if (placementId == rewardVideoID) {
            if (rewardBtn)
                rewardBtn.SetActive(false);
        }

        // Reward the user for watching the ad if watched to completion.
        if (showResult == ShowResult.Finished) {
            goldManager.AddGold(25, true);
        }

        ShowBanner();
    }
}
