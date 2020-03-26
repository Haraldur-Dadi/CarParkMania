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

    public Button[] levelBtns; 

    public GameObject homeBtn;
    public GameObject backBtn;
    public TextMeshProUGUI selectPanelName;

    public string[] gameModeNames;

    public void Start() {
        sceneFader = SceneFader.Instance;
        saveManager = SaveManager.Instance;

        int LevelReached = PlayerPrefs.GetInt("LevelReached", 0);

        for (int i = 0; i < levelBtns.Length; i++) {
            if (i > LevelReached) {
                levelBtns[i].interactable = false;
            }

            int level = i;
            levelBtns[i].onClick.AddListener(() => SelecteLevel(1, level));
        }

        UIStartState();
    }

    public void UIStartState() {
        home.SetActive(true);
        levelSelector.SetActive(false);
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

    public void SelecteLevel(int buildIndex, int level) {
        sceneFader.FadeToBuildIndex(buildIndex);
        saveManager.SaveData("boardToLoad", level);
    }

    public IEnumerator HomeScreen() {
        float t = 0f;

        while (t < 0.5f) {
            t += Time.deltaTime;
            yield return 0;
        }

        home.SetActive(true);
        levelSelector.SetActive(false);
        gameModesParent.SetActive(false);
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

        selectPanelName.text = gameModeNames[gameModeNr];
    }
}
