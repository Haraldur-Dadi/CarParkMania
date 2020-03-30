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
    public GameObject[] level_boards_easy;
    public GameObject[] level_boards_medium;
    public GameObject[] level_boards_hard;
    public GameObject[] level_boards_expert;

    public GameObject levelCompleteUi;
    public TextMeshProUGUI levelTxt;
    public TextMeshProUGUI difficultyTxt;

    public virtual void Awake() {
        if (!instance) {
            instance = this;
        } else {
            Destroy(this);
        }
    }

    public virtual void Start() {
        sceneFader = SceneFader.Instance;
        saveManager = SaveManager.Instance;

        /* Sets required information for class at start up */
        finished = false;

        LoadLevel();
        levelCompleteUi.SetActive(false);
    }

    public void LoadLevel() {
        levelIndex = PlayerPrefs.GetInt("boardToLoad", 0);
        levelTxt.text = (levelIndex + 1).ToString();
        GameObject level_board = null;

        if (levelIndex < 25) {
            difficultyTxt.text = "Easy";
             level_board = level_boards_easy[levelIndex];
        } else if (25 <= levelIndex && levelIndex < 50) {
            difficultyTxt.text = "Medum";
            level_board = level_boards_medium[levelIndex-25];
        } else if (50 <= levelIndex && levelIndex < 75) {
            difficultyTxt.text = "Hard";
            level_board = level_boards_hard[levelIndex - 50];
        } else if (75 <= levelIndex) {
            difficultyTxt.text = "Expert";
            level_board = level_boards_expert[levelIndex - 75];
        }

        Instantiate(level_board, gameBoard.transform);
    }

    public virtual void LevelComplete() {
        if (PlayerPrefs.GetInt("LevelReached", 0) <= levelIndex) {
            saveManager.SaveIntData("LevelReached", levelIndex + 1);
        }

        ToggleLevelCompleteUI();
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