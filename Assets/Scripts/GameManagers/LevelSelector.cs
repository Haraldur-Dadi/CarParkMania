using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelector : MonoBehaviour {

    public CrossSceneManager crossSceneManager;
    public SceneFader sceneFader;
    public SaveManager saveManager;

    public CanvasGroup canvasGroup;
    private bool startUI;

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
        crossSceneManager = CrossSceneManager.Instance;
        sceneFader = SceneFader.Instance;
        saveManager = SaveManager.Instance;

        int casualLevelReached = PlayerPrefs.GetInt("CasualLevelReached", 0);

        for (int i = 0; i < casualLevelBtns.Length; i++) {
            if (i > casualLevelReached) {
                casualLevelBtns[i].interactable = false;
            } else {
                int level = i;
                casualLevelBtns[i].onClick.AddListener(() => SelecteLevel(1, level));
                casualLevelBtns[i].onClick.AddListener(() => TmpPreventClicks());

                if (i < casualLevelReached) {
                    casualLevelBtns[i].gameObject.GetComponent<Image>().color = new Color(0.4029462f, 1, 0.2311321f);
                } else {
                    casualLevelBtns[i].gameObject.GetComponent<Image>().color = new Color(1, 0.9341092f, 0.2313725f);
                }
            }

            casualLevelBtns[i].GetComponentInChildren<TextMeshProUGUI>().text = (i + 1).ToString();
        }

        startUI = true;
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

        string panelName = crossSceneManager.panelName;
        int gameModeNr = crossSceneManager.gameModeNr;
        int difficulty = crossSceneManager.difficulty;

        if (panelName != "") {
            ToggleUiPanel(panelName);
        } else if (gameModeNr != -1) {
            OpenGameModes();
            SelectGameMode(gameModeNr);
            SelectDifficulty(difficulty);
        }
        startUI = false;
    }

    public void ToggleUiPanel(string panelName) {
        if (!startUI)
            sceneFader.FadeBetweenObjects();
        StartCoroutine(TogglePanels(panelName));
        crossSceneManager.panelName = panelName;
    }


    public void OpenGameModes() {
        if (!startUI)
            sceneFader.FadeBetweenObjects();
        StartCoroutine(GameModes());
    }

    public void SelectGameMode(int gameModeNr) {
        if (!startUI)
            sceneFader.FadeBetweenObjects();
        StartCoroutine(SelectGameModeUI(gameModeNr));
        crossSceneManager.gameModeNr = gameModeNr;
    }

    public void SelectDifficulty(int difficulty) {
        for (int i = 0; i < casualLevelDifficulties.Length; i++) {
            if (i == difficulty) {
                casualLevelDifficulties[i].SetActive(true);
            } else {
                casualLevelDifficulties[i].SetActive(false);
            }
        }
        crossSceneManager.difficulty = difficulty;
    }

    public void SelecteLevel(int buildIndex, int level) {
        TmpPreventClicks();
        saveManager.SaveIntData("boardToLoad", level);
        sceneFader.FadeToBuildIndex(buildIndex);
    }

    public void BackHome() {
        sceneFader.FadeBetweenObjects();
        StartCoroutine(HomeScreen());
        crossSceneManager.panelName = "";
        crossSceneManager.gameModeNr = -1;
        crossSceneManager.difficulty = -1;
    }

    public void TmpPreventClicks() {
        CrossSceneManager.Instance.TmpPreventClicks();
    }

    public void PlayButtonClick() {
        AudioManager.Instance.PlayButtonClick();
    }

    public IEnumerator TogglePanels(string panelName) {
        if (!startUI) {
            yield return new WaitForSeconds(0.5f);
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

    public IEnumerator GameModes() {
        if (!startUI) {
            yield return new WaitForSeconds(0.5f);
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
        if (!startUI) {
            yield return new WaitForSeconds(0.5f);
        }

        gameModesParent.SetActive(false);
        gameModes[gameModeNr].SetActive(true);

        homeBtn.SetActive(false);
        backBtn.SetActive(true);

        selectPanelName.text = gameModes[gameModeNr].name;
        SelectDifficulty(0);
    }

    public IEnumerator HomeScreen() {
        yield return new WaitForSeconds(0.5f);
        UIStartState();
    }
}
