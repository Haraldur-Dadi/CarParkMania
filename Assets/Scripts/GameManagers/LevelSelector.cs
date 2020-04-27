using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelector : MonoBehaviour {

    public CrossSceneManager crossSceneManager;
    public SceneFader sceneFader;
    private bool startUI;

    public GameObject home;
    public GameObject levelSelector;
    public GameObject gameModesParent;
    public GameObject casualGameModePanel;
    public GameObject challengeGameModePanel;

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

        if (crossSceneManager.panelName != "") {
            ToggleUiPanel(crossSceneManager.panelName);
        } else if (crossSceneManager.gameModeNr > 0) {
            StartCoroutine(GameModeSelector());
            StartCoroutine(SelectGameModeUI(crossSceneManager.gameModeNr));
        }
        startUI = false;
    }

    public void ToggleUiPanel(string panelName) {
        if (!startUI)
            sceneFader.FadeBetweenObjects();
        StartCoroutine(TogglePanels(panelName));
    }

    public void OpenGameModeSelector() {
        if (!startUI)
            sceneFader.FadeBetweenObjects();
        StartCoroutine(GameModeSelector());
    }

    public void SelectGameMode(int gameModeNr) {
        if (!startUI)
            sceneFader.FadeBetweenObjects();
        StartCoroutine(SelectGameModeUI(gameModeNr));
    }

    public void BackHome() {
        sceneFader.FadeBetweenObjects();
        StartCoroutine(HomeScreen());
    }

    public void PlayButtonClick() {
        AudioManager.Instance.PlayButtonClick();
    }

    public IEnumerator TogglePanels(string panelName) {
        if (!startUI) {
            float t = 0f;
            while (t < 1f) {
                t += Time.deltaTime * 3;
                yield return null;
            }
        }

        mainScreen.SetActive(!mainScreen.activeSelf);
        panelHeader.SetActive(!panelHeader.activeSelf);

        crossSceneManager.panelName = panelName;        
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

    public IEnumerator GameModeSelector() {
        if (!startUI) {
            float t = 0f;
            while (t < 1f) {
                t += Time.deltaTime * 3;
                yield return null;
            }
        }

        home.SetActive(false);
        levelSelector.SetActive(true);
        gameModesParent.SetActive(true);
        casualGameModePanel.SetActive(false);
        challengeGameModePanel.SetActive(false);
        homeBtn.SetActive(true);
        backBtn.SetActive(false);

        selectPanelName.text = "Modes";
    }

    public IEnumerator SelectGameModeUI(int gameModeNr) {
        crossSceneManager.gameModeNr = gameModeNr;
        
        if (!startUI) {
            float t = 0f;
            while (t < 1f) {
                t += Time.deltaTime * 3;
                yield return null;
            }
            
            if (gameModeNr == 1) {
                casualGameModePanel.GetComponent<GameModePanel>().SelectDifficulty(0);
            } else {
                challengeGameModePanel.GetComponent<GameModePanel>().SelectDifficulty(0);
            }
        }

        gameModesParent.SetActive(false);
        if (gameModeNr == 1) {
            selectPanelName.text = "Casual";
            casualGameModePanel.SetActive(true);
        } else {
            selectPanelName.text = "Challenge";
            challengeGameModePanel.SetActive(true);
        }
        homeBtn.SetActive(false);
        backBtn.SetActive(true);
    }

    public IEnumerator HomeScreen() {
        float t = 0f;
        while (t < 1f) {
            t += Time.deltaTime * 3;
            yield return null;
        }

        crossSceneManager.panelName = "";
        crossSceneManager.gameModeNr = 0;
        crossSceneManager.difficulty = 0;
        UIStartState();
    }
}
