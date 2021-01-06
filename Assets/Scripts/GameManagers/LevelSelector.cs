﻿using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class LevelSelector : MonoBehaviour {
    bool startUI;

    public GameObject home;
    public GameObject levelSelector;
    public GameObject gameModesParent;
    public GameModePanel gameModePanel;

    public GameObject homeBtn;
    public GameObject backBtn;
    public TextMeshProUGUI selectPanelName;

    public GameObject panelHeader;
    public TextMeshProUGUI uiName;
    public GameObject mainScreen;
    public GameObject dailyChallengePanel;
    public GameObject dailySpinPanel;
    public DailySpin dailySpin;
    public GameObject achivementsPanel;
    public GameObject shopPanel;
    public GameObject aboutPanel;
    public GameObject watchAdCon;

    void Start() {
        dailySpin = GetComponent<DailySpin>();
        dailySpin.OpenDailySpin();
        if (PlayerPrefs.HasKey("LastOpened")) {
            startUI = true;
            UIStartState();
            if (DateTime.Today > DateTime.Parse(PlayerPrefs.GetString("LastOpened", "1582-09-15"))) {
                // Open up daily spin if available
                startUI = true;
                PlayerPrefs.SetString("LastOpened", DateTime.Today.ToString("yyyy-MM-dd"));
                ToggleUiPanel("Spin");
                startUI = false;
            }
        } else {
            home.SetActive(false);
            levelSelector.SetActive(false);
            PlayerPrefs.SetString("LastOpened", "1582-09-15");
            PlayerPrefs.SetInt("boardToLoad", 0);
            CrossSceneManager.Instance.FadeToBuildIndex(2);
        }
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
        watchAdCon.SetActive(false);

        if (CrossSceneManager.Instance.panelName != "") {
            ToggleUiPanel(CrossSceneManager.Instance.panelName);
        } else if (CrossSceneManager.Instance.gameModeNr > 0) {
            StartCoroutine(GameModeSelector());
            StartCoroutine(SelectGameModeUI(CrossSceneManager.Instance.gameModeNr));
        }
        startUI = false;
    }

    public void ToggleUiPanel(string panelName) { StartCoroutine(TogglePanels(panelName)); }
    public void OpenGameModeSelector() { StartCoroutine(GameModeSelector()); }
    public void SelectGameMode(int gameModeNr) { StartCoroutine(SelectGameModeUI(gameModeNr)); }
    public void BackHome() { StartCoroutine(HomeScreen()); }
    public void ToggleAdGoldConformation() { watchAdCon.SetActive(!watchAdCon.activeSelf); }
    public void PlayButtonClick() { AudioManager.Instance.PlayButtonClick(); }

    IEnumerator TogglePanels(string panelName) {
        yield return WaitTimer();

        mainScreen.SetActive(!mainScreen.activeSelf);
        panelHeader.SetActive(!panelHeader.activeSelf);
        CrossSceneManager.Instance.panelName = panelName;        
        uiName.text = panelName;

        if (panelName == "Achivements") {
            achivementsPanel.SetActive(!achivementsPanel.activeSelf);
            AchivementManager.Instance.UpdateAchivementsUI();
        } else if (panelName == "Challenge") {
            dailyChallengePanel.SetActive(!dailyChallengePanel.activeSelf);
        } else if (panelName == "Spin") {
            dailySpinPanel.SetActive(!dailySpinPanel.activeSelf);
            dailySpin.OpenDailySpin();
        } else if (panelName == "Shop") {
            shopPanel.SetActive(!shopPanel.activeSelf);
        } else if (panelName == "About") {
            aboutPanel.SetActive(!aboutPanel.activeSelf);
        }
        watchAdCon.SetActive(false);
    }

    IEnumerator GameModeSelector() {
        yield return WaitTimer();

        home.SetActive(false);
        watchAdCon.SetActive(false);
        levelSelector.SetActive(true);
        gameModesParent.SetActive(true);
        gameModePanel.gameObject.SetActive(false);
        homeBtn.SetActive(true);
        backBtn.SetActive(false);

        selectPanelName.text = "Modes";
    }
    IEnumerator SelectGameModeUI(int gameModeNr) {
        CrossSceneManager.Instance.gameModeNr = gameModeNr;
        yield return WaitTimer();

        gameModesParent.SetActive(false);
        if (gameModeNr == 1) {
            selectPanelName.text = "Casual";
        } else if (gameModeNr == 2) {
            selectPanelName.text = "Challenge";
        } else if (gameModeNr == 3) {
            selectPanelName.text = "8x8";
        }
        gameModePanel.Display(selectPanelName.text);
        homeBtn.SetActive(false);
        backBtn.SetActive(true);
    }
    IEnumerator HomeScreen() {
        yield return WaitTimer();

        CrossSceneManager.Instance.panelName = "";
        CrossSceneManager.Instance.gameModeNr = 0;
        CrossSceneManager.Instance.difficulty = 0;
        UIStartState();
    }
    IEnumerator WaitTimer() {
        if (!startUI) {
            CrossSceneManager.Instance.FadeBetweenObjects();
            yield return new WaitForSeconds(0.33f);
        }
        yield return null;
    }
}