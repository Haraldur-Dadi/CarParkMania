using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public SceneFader sceneFader;
    public SaveManager saveManager;

    public int levelIndex;

    public bool finished;
    public bool paused;

    public Button homeBtn;
    public Button settingsBtn;
    public Button retryBtn;
    public Button nextLevelBtn;
    public Button resetButton;

    public GameObject gameBoard;
    public GameObject[] level_boards;

    public GameObject topUI;
    public GameObject settingsUi;
    public GameObject levelCompleteUi;

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
        settingsUi = GameObject.Find("SettingsUI").gameObject;
        levelCompleteUi = GameObject.Find("LevelCompleteUI").gameObject;
        resetButton = GameObject.Find("ResetButton").GetComponent<Button>();

        homeBtn.onClick.AddListener(delegate { GoToLevelSelector(); });
        settingsBtn.onClick.AddListener(delegate { ToggleSettings(); });
        resetButton.onClick.AddListener(delegate { RetryLevel(); });
    }

    public virtual void Start() {
        sceneFader = SceneFader.Instance;
        saveManager = SaveManager.Instance;

        /* Sets required information for class at start up */
        finished = true;
        paused = true;

        LoadLevel();
        ToggleSettings();
        ToggleLevelCompleteUI();
    }

    public void LoadLevel() {
        levelIndex = PlayerPrefs.GetInt("boardToLoad", 0);
        GameObject level_board = level_boards[levelIndex];
        Instantiate(level_board, gameBoard.transform);
    }

    public virtual void LevelComplete() {
        if (PlayerPrefs.GetInt("LevelReached", 0) < levelIndex) {
            saveManager.SaveData("LevelReached", levelIndex);
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

    public virtual void ToggleSettings() {
        /* Toggles settings ui, 
           if ui active the game is paused */
        settingsUi.SetActive(!settingsUi.activeSelf);
        paused = !paused;
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
        saveManager.SaveData("boardToLoad", levelIndex + 1);
    }
}