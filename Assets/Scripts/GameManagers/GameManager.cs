using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public SceneFader sceneFader;
    public SaveManager saveManager;

    public int levelIndex;

    public bool finished;

    public Button homeBtn;
    public Button settingsBtn;
    public Button retryBtn;
    public Button nextLevelBtn;
    public Button resetButton;

    public GameObject gameBoard;
    public GameObject tmpBoard;
    public GameObject[] level_boards;

    public GameObject topUI;
    public GameObject levelCompleteUi;
    public TextMeshProUGUI levelTxt;
    public TextMeshProUGUI difficultyTxt;

    public virtual void Awake() {
        if (!instance) {
            instance = this;
        } else {
            Destroy(this);
        }

        /* Get UI elements required for class at start up and adds onClick to buttons */
        gameBoard = GameObject.Find("Board");
        topUI = GameObject.Find("TopUI");
        homeBtn = topUI.transform.Find("HomeBtn").GetComponent<Button>();
        settingsBtn = topUI.transform.Find("SettingsBtn").GetComponent<Button>();
        levelCompleteUi = GameObject.Find("LevelCompleteUI").gameObject;
        resetButton = GameObject.Find("ResetButton").GetComponent<Button>();
        levelTxt = GameObject.Find("Level").GetComponent<TextMeshProUGUI>();
        difficultyTxt = GameObject.Find("Difficulty").GetComponent<TextMeshProUGUI>();

        homeBtn.onClick.AddListener(delegate { GoToLevelSelector(); });
        resetButton.onClick.AddListener(delegate { RetryLevel(); });
    }

    public virtual void Start() {
        sceneFader = SceneFader.Instance;
        saveManager = SaveManager.Instance;

        /* Sets required information for class at start up */
        finished = true;
        tmpBoard = null;

        LoadLevel();
        ToggleLevelCompleteUI();
    }

    public void LoadLevel() {
        levelIndex = PlayerPrefs.GetInt("boardToLoad", 0);
        levelTxt.text = levelIndex.ToString();
        if (levelIndex < 25) {
            difficultyTxt.text = "Beginner";
        }

        GameObject level_board = level_boards[levelIndex];
        Instantiate(level_board, gameBoard.transform);
    }

    public virtual void LevelComplete() {
        if (PlayerPrefs.GetInt("LevelReached", 0) <= levelIndex) {
            saveManager.SaveIntData("LevelReached", levelIndex + 1);
        }

        ToggleLevelCompleteUI();

        retryBtn = levelCompleteUi.transform.Find("RetryButton").GetComponent<Button>();
        nextLevelBtn = levelCompleteUi.transform.Find("NextLevelButton").GetComponent<Button>();

        retryBtn.onClick.AddListener(delegate { RetryLevel(); });
        nextLevelBtn.onClick.AddListener(delegate { LoadNextLevel(); });
    }

    public void GoToLevelSelector() {
        /* Sends player to home screen */
        sceneFader.FadeToBuildIndex(0);
    }

    public void ToggleLevelCompleteUI() {
        /* Toggles level complete ui */
        levelCompleteUi.SetActive(!levelCompleteUi.activeSelf);
        finished = !finished;
    }

    public virtual void IncreaseMoves(int amount) {
        // Base method for Challenge Mode
    }

    public virtual void DecreaseMoves(int amount) {
        // Base method for Challange Mode
    }

    public void RetryLevel() {
        sceneFader.FadeToBuildIndex(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel() {
        saveManager.SaveIntData("boardToLoad", levelIndex + 1);
        RetryLevel();
    }
}