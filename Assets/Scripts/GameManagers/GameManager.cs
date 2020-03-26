using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public int stageIndex;
    public int levelBuildIndex;
    public int lastLevelInStageIndex;

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

        levelBuildIndex = SceneManager.GetActiveScene().buildIndex;

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

        LoadLevel();
    }

    public virtual void Start() {
        /* Sets required information for class at start up */
        finished = true;
        paused = true;

        ToggleSettings();
        ToggleLevelCompleteUI();
    }

    public void LoadLevel() {
        GameObject level_board = level_boards[PlayerPrefs.GetInt("boardToLoad", 0)];
        Instantiate(level_board, gameBoard.transform);
    }

    public virtual void LevelComplete() {
        if (PlayerPrefs.GetInt("LevelReached", 1) < levelBuildIndex) {
            PlayerPrefs.SetInt("LevelReached", levelBuildIndex);
        }

        if (PlayerPrefs.GetInt("StageCompleted", 1) == stageIndex) {
            if (PlayerPrefs.GetInt("LevelReached", 1) == lastLevelInStageIndex) {
                PlayerPrefs.SetInt("StageCompleted", stageIndex + 1);
            }
        }

        ToggleLevelCompleteUI();

        retryBtn = levelCompleteUi.transform.Find("RetryButton").GetComponent<Button>();
        nextLevelBtn = levelCompleteUi.transform.Find("NextLevelButton").GetComponent<Button>();

        retryBtn.onClick.AddListener(delegate { RetryLevel(); });
        nextLevelBtn.onClick.AddListener(delegate { LoadNextLevel(); });
    }

    public void GoToLevelSelector() {
        /* Sends player to home screen */
        SceneManager.LoadScene(1);
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
        SceneManager.LoadScene(levelBuildIndex);
    }

    public void LoadNextLevel() {
        if (stageIndex != lastLevelInStageIndex) {
            SceneManager.LoadScene(levelBuildIndex);
            PlayerPrefs.SetInt("boardToLoad", PlayerPrefs.GetInt("boardToLoad", 0) + 1);
        } else {
            SceneManager.LoadScene(levelBuildIndex + 1);
            PlayerPrefs.SetInt("boardToLoad", 0);
        }
    }
}