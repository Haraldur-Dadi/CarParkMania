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
    public Button retryBtn;
    public Button nextLevelBtn;
    public Button resetButton;

    public GameObject gameBoard;
    public GameObject tmpBoard;
    public GameObject[] level_boards;

    public GameObject levelCompleteUi;
    public TextMeshProUGUI levelTxt;
    public TextMeshProUGUI difficultyTxt;

    public virtual void Awake() {
        if (!instance) {
            instance = this;
        } else {
            Destroy(this);
        }

        /* Adds onClick to buttons */
        homeBtn.onClick.AddListener(delegate { GoToLevelSelector(); });
        resetButton.onClick.AddListener(delegate { RetryLevel(); });
    }

    public virtual void Start() {
        sceneFader = SceneFader.Instance;
        saveManager = SaveManager.Instance;

        /* Sets required information for class at start up */
        finished = false;
        tmpBoard = null;

        LoadLevel();
        levelCompleteUi.SetActive(false);
    }

    public void LoadLevel() {
        levelIndex = PlayerPrefs.GetInt("boardToLoad", 0);
        levelTxt.text = (levelIndex + 1).ToString();
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