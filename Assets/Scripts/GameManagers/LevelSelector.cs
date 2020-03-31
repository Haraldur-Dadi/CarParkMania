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

    public Button[] casualLevelBtns; 
    public Button[] challengeLevelBtns; 
    public Button[] timedLevelBtns; 

    public GameObject homeBtn;
    public GameObject backBtn;
    public TextMeshProUGUI selectPanelName;

    public void Start() {
        sceneFader = SceneFader.Instance;
        saveManager = SaveManager.Instance;

        int casualLevelReached = PlayerPrefs.GetInt("CasualLevelReached", 0);
        int challengedLevelReached = PlayerPrefs.GetInt("ChallengeLevelReached", 0);
        int timedLevelReached = PlayerPrefs.GetInt("TimedLevelReached", 0);

        for (int i = 0; i < casualLevelBtns.Length; i++) {
            if (i > casualLevelReached) {
                casualLevelBtns[i].interactable = false;
            } else {
                if (i < casualLevelReached) {
                    casualLevelBtns[i].gameObject.GetComponent<Image>().color = new Color(0.4029462f, 1, 0.2311321f);
                } else {
                    casualLevelBtns[i].gameObject.GetComponent<Image>().color = new Color(1, 0.9341092f, 0.2313725f);
                }

                int level = i;
                casualLevelBtns[i].onClick.AddListener(() => SelecteLevel(1, level));
            }
        }
        for (int i = 0; i < challengeLevelBtns.Length; i++) {
            if (i > challengedLevelReached) {
                challengeLevelBtns[i].interactable = false;
            } else {
                if (i < challengedLevelReached) {
                    challengeLevelBtns[i].gameObject.GetComponent<Image>().color = new Color(0.4029462f, 1, 0.2311321f);
                } else {
                    challengeLevelBtns[i].gameObject.GetComponent<Image>().color = new Color(1, 0.9341092f, 0.2313725f);
                }

                int level = i;
                challengeLevelBtns[i].onClick.AddListener(() => SelecteLevel(2, level));
            }
        }
        for (int i = 0; i < timedLevelBtns.Length; i++) {
            if (i > timedLevelReached) {
                timedLevelBtns[i].interactable = false;
            } else {
                if (i < timedLevelReached) {
                    timedLevelBtns[i].gameObject.GetComponent<Image>().color = new Color(0.4029462f, 1, 0.2311321f);
                } else {
                    timedLevelBtns[i].gameObject.GetComponent<Image>().color = new Color(1, 0.9341092f, 0.2313725f);
                }

                int level = i;
                timedLevelBtns[i].onClick.AddListener(() => SelecteLevel(3, level));
            }
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
        saveManager.SaveIntData("boardToLoad", level);
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

        selectPanelName.text = gameModes[gameModeNr].name;
    }
}
