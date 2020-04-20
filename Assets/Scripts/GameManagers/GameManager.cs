using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;
    public SceneFader sceneFader;
    public SaveManager saveManager;
    public ItemDb itemDb;

    public int levelIndex;
    public int boardLength;
    
    public bool finished;

    public GameObject gameBoard;
    public GameObject level_board;
    public GameObject[] level_boards_easy;
    public GameObject[] level_boards_medium;
    public GameObject[] level_boards_hard;
    public GameObject[] level_boards_expert;

    public GameObject levelCompleteUi;
    public TextMeshProUGUI levelTxt;
    public TextMeshProUGUI difficultyTxt;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }
    }

    public virtual void Start() {
        sceneFader = SceneFader.Instance;
        saveManager = SaveManager.Instance;
        itemDb = ItemDb.Instance;

        level_board = null;
        boardLength = level_boards_easy.Length;
        LoadLevel();
    }

    public virtual void LoadLevel() {
        CrossSceneManager.Instance.TmpPreventClicks();
        if (level_board != null)
            Destroy(level_board);

        finished = false;
        levelCompleteUi.SetActive(false);

        levelIndex = PlayerPrefs.GetInt("boardToLoad", 0);
        GameObject boardToInst = null;
        if (levelIndex < boardLength) {
            difficultyTxt.text = "Easy";
            boardToInst = level_boards_easy[levelIndex];
            CrossSceneManager.Instance.difficulty = 0;
        } else if (boardLength <= levelIndex && levelIndex < (boardLength * 2)) {
            difficultyTxt.text = "Medium";
            boardToInst = level_boards_medium[levelIndex - level_boards_easy.Length];
            CrossSceneManager.Instance.difficulty = 1;
        } else if ((boardLength * 2) <= levelIndex && levelIndex < (boardLength * 3)) {
            difficultyTxt.text = "Hard";
            boardToInst = level_boards_hard[levelIndex - (level_boards_easy.Length * 2)];
            CrossSceneManager.Instance.difficulty = 2;
        } else if ((boardLength * 3) <= levelIndex) {
            difficultyTxt.text = "Expert";
            boardToInst = level_boards_expert[levelIndex - (level_boards_easy.Length * 3)];
            CrossSceneManager.Instance.difficulty = 3;
        }

        level_board = Instantiate(boardToInst, gameBoard.transform);
        ChangeCarSprites();
    }

    public void ChangeCarSprites() {
        // Switches sprites of cars to match equipped
        GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");

        foreach (GameObject car in cars) {
            Car carObj = car.GetComponent<Car>();

            if (carObj.length == 0) {
                carObj.carImg.sprite = itemDb.GetItem("PlayerCar", PlayerPrefs.GetInt("PlayerCarEquipped", 0)).sprite;
            } else if (carObj.length == 2) {
                carObj.carImg.sprite = itemDb.GetItem("2LongCar", PlayerPrefs.GetInt("2LongCarEquipped", 0)).sprite;
            } else if (carObj.length == 3) {
                carObj.carImg.sprite = itemDb.GetItem("3LongCar", PlayerPrefs.GetInt("3LongCarEquipped", 0)).sprite;
            }
        }
    }

    public virtual void LevelComplete() {
        AudioManager.Instance.PlayWinSound();
        finished = true;

        if (levelIndex < level_boards_easy.Length)
            saveManager.IncreaseAchivementProgress(4);
        saveManager.IncreaseAchivementProgress(5);
        saveManager.IncreaseAchivementProgress(6);

        levelCompleteUi.SetActive(true);
    }

    public void GoToLevelSelector() {
        /* Sends player to home screen */
        CrossSceneManager.Instance.TmpPreventClicks();
        sceneFader.FadeToBuildIndex(0);
    }

    public void RetryLevel() {
        StartCoroutine(PreLoadLevel());
    }

    public void LoadNextLevel() {
        saveManager.SaveIntData("boardToLoad", levelIndex + 1);
        StartCoroutine(PreLoadLevel());
    }

    public void PlayButtonClick() {
        AudioManager.Instance.PlayButtonClick();
    }

    public IEnumerator PreLoadLevel() {
        CrossSceneManager.Instance.TmpPreventClicks();
        sceneFader.FadeBetweenObjects();
        yield return new WaitForSeconds(0.5f);
        LoadLevel();
    }
    
    public IEnumerator DelayedLevelSelector() {
        yield return new WaitForSeconds(2);
        GoToLevelSelector();
    }

    public IEnumerator CountdownNextLevel() {
        yield return new WaitForSeconds(2);
        StartCoroutine(PreLoadLevel());
    }
}