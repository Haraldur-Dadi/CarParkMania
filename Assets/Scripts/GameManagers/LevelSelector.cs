using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelector : MonoBehaviour {

    public SceneFader sceneFader;
    public SaveManager saveManager;

    public GameObject home;
    public GameObject levelSelector;
    public GameObject gameModesParent;
    public GameObject[] gameModes;

    public GameObject[] casualLevelDifficulties;
    public Button[] casualLevelBtns;

    public GameObject homeBtn;
    public GameObject backBtn;
    public TextMeshProUGUI selectPanelName;

    public GameObject mainScreen;
    public GameObject dailyChallangePanel;
    public GameObject dailySpinPanel;
    public GameObject achivementsPanel;
    public GameObject shopPanel;
    public GameObject aboutPanel;

    public DailySpin dailySpin;
    public About about;

    public void Start() {
        sceneFader = SceneFader.Instance;
        saveManager = SaveManager.Instance;

        dailySpin = GetComponent<DailySpin>();
        about = GetComponent<About>();

        int casualLevelReached = PlayerPrefs.GetInt("CasualLevelReached", 0);
        int challengedLevelReached = PlayerPrefs.GetInt("ChallengeLevelReached", 0);
        int timedLevelReached = PlayerPrefs.GetInt("TimedLevelReached", 0);

        for (int i = 0; i < casualLevelBtns.Length; i++) {
            if (i > casualLevelReached) {
                casualLevelBtns[i].interactable = false;
            } else {
                int level = i;
                casualLevelBtns[i].onClick.AddListener(() => SelecteLevel(1, level));

                if (i < casualLevelReached) {
                    casualLevelBtns[i].gameObject.GetComponent<Image>().color = new Color(0.4029462f, 1, 0.2311321f);
                } else {
                    casualLevelBtns[i].gameObject.GetComponent<Image>().color = new Color(1, 0.9341092f, 0.2313725f);
                }
            }

            casualLevelBtns[i].GetComponentInChildren<TextMeshProUGUI>().text = (i + 1).ToString();
        }

        SelectDifficulty(0);
        UIStartState();
    }

    public void UIStartState() {
        home.SetActive(true);
        mainScreen.SetActive(true);
        levelSelector.SetActive(false);
        dailyChallangePanel.SetActive(false);
        dailySpinPanel.SetActive(false);
        achivementsPanel.SetActive(false);
        shopPanel.SetActive(false);
        aboutPanel.SetActive(false);
    }

    public void ToggleAchivements() {
        achivementsPanel.SetActive(!achivementsPanel.activeSelf);
        mainScreen.SetActive(!mainScreen.activeSelf);
    }

    public void ToggleDailyChallange() {
        dailyChallangePanel.SetActive(!dailyChallangePanel.activeSelf);
        mainScreen.SetActive(!mainScreen.activeSelf);
    }

    public void ToggleDailySpin() {
        dailySpinPanel.SetActive(!dailySpinPanel.activeSelf);
        dailySpin.OpenUI();
        mainScreen.SetActive(!mainScreen.activeSelf);
    }

    public void ToggleShop() {
        shopPanel.SetActive(!shopPanel.activeSelf);
        mainScreen.SetActive(!mainScreen.activeSelf);
    }

    public void ToggleAbout() {
        aboutPanel.SetActive(!aboutPanel.activeSelf);
        about.ResetTutImage();
        mainScreen.SetActive(!mainScreen.activeSelf);
    }

    public void BackHome() {
        sceneFader.FadeBetweenObjects();
        StartCoroutine(HomeScreen());
    }

    public void OpenGameModes() {
        sceneFader.FadeBetweenObjects();
        StartCoroutine(GameModes());
    }

    public void SelectGameMode(int gameModeNr) {
        sceneFader.FadeBetweenObjects();
        StartCoroutine(SelectGameModeUI(gameModeNr));
    }

    public void SelectDifficulty(int difficulty) {
        for (int i = 0; i < casualLevelDifficulties.Length; i++) {
            if (i == difficulty) {
                casualLevelDifficulties[i].SetActive(true);
            } else {
                casualLevelDifficulties[i].SetActive(false);
            }
        }
    }

    public void SelecteLevel(int buildIndex, int level) {
        sceneFader.FadeToBuildIndex(buildIndex);
        saveManager.SaveIntData("boardToLoad", level);
    }

    public IEnumerator HomeScreen() {
        float t = 0f;

        while (t < 0.5f) {
            t += Time.deltaTime;
            yield return 0;
        }

        UIStartState();
    }

    public IEnumerator GameModes() {
        float t = 0f;

        while (t < 0.5f) {
            t += Time.deltaTime;
            yield return 0;
        }

        home.SetActive(false);
        levelSelector.SetActive(true);
        gameModesParent.SetActive(true);

        homeBtn.SetActive(true);
        backBtn.SetActive(false);

        selectPanelName.text = "Game Modes";

        foreach (GameObject item in gameModes) {
            item.SetActive(false);
        }
    }

    public IEnumerator SelectGameModeUI(int gameModeNr) {
        float t = 0f;

        while (t < 0.5f) {
            t += Time.deltaTime;
            yield return 0;
        }

        gameModesParent.SetActive(false);
        gameModes[gameModeNr].SetActive(true);

        homeBtn.SetActive(false);
        backBtn.SetActive(true);

        selectPanelName.text = gameModes[gameModeNr].name;
    }
}
