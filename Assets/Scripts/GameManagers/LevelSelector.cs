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

    public GameObject panelHeader;
    public TextMeshProUGUI uiName;
    public GameObject mainScreen;
    public GameObject dailyChallengePanel;
    public GameObject dailySpinPanel;
    public GameObject achivementsPanel;
    public GameObject shopPanel;
    public GameObject aboutPanel;

    public DailySpin dailySpin;
    public About about;

    public void Start() {
        sceneFader = SceneFader.Instance;
        saveManager = SaveManager.Instance;

        int casualLevelReached = PlayerPrefs.GetInt("CasualLevelReached", 0);

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

        UIStartState();
    }

    public void UIStartState() {
        home.SetActive(true);
        mainScreen.SetActive(true);
        panelHeader.SetActive(false);
        levelSelector.SetActive(false);
        dailyChallengePanel.SetActive(false);
        dailySpinPanel.SetActive(false);
        achivementsPanel.SetActive(false);
        shopPanel.SetActive(false);
        aboutPanel.SetActive(false);
    }

    public void ToggleUiPanel(string panelName) {
        sceneFader.FadeBetweenObjects();
        StartCoroutine(TogglePanels(panelName));
        AudioManager.Instance.PlayButtonClick();
    }
    
    public void BackHome() {
        sceneFader.FadeBetweenObjects();
        AudioManager.Instance.PlayButtonClick();
        StartCoroutine(HomeScreen());
    }

    public void OpenGameModes() {
        sceneFader.FadeBetweenObjects();
        AudioManager.Instance.PlayButtonClick();
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
        AudioManager.Instance.PlayButtonClick();
    }

    public void SelecteLevel(int buildIndex, int level) {
        saveManager.SaveIntData("boardToLoad", level);
        AudioManager.Instance.PlayButtonClick();
        sceneFader.FadeToBuildIndex(buildIndex);
    }

    public IEnumerator TogglePanels(string panelName) {
        float t = 0f;

        while (t < 0.5f) {
            t += Time.deltaTime;
            yield return 0;
        }
        mainScreen.SetActive(!mainScreen.activeSelf);
        panelHeader.SetActive(!panelHeader.activeSelf);
        
        uiName.text = panelName;

        if (panelName == "Achivements") {
            achivementsPanel.SetActive(!achivementsPanel.activeSelf);
        } else if (panelName == "Challenge") {
            dailyChallengePanel.SetActive(!dailyChallengePanel.activeSelf);
        } else if (panelName == "Spin") {
            dailySpinPanel.SetActive(!dailySpinPanel.activeSelf);
            dailySpin.OpenUI();
        } else if (panelName == "Shop") {
            shopPanel.SetActive(!shopPanel.activeSelf);
        } else if (panelName == "About") {
            aboutPanel.SetActive(!aboutPanel.activeSelf);
            about.ResetTutImage();
        }
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

        selectPanelName.text = "Modes";

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
        SelectDifficulty(0);
    }
}
