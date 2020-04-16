using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {

    public SceneFader sceneFader;
    public SaveManager saveManager;
    public ItemDb itemDb;

    public int levelIndex;

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

    public virtual void Start() {
        sceneFader = SceneFader.Instance;
        saveManager = SaveManager.Instance;
        itemDb = ItemDb.Instance;
        
        level_board = null;
        LoadLevel();
    }

    public virtual void LoadLevel() {
        if (level_board != null)
            Destroy(level_board);

        finished = false;
        levelCompleteUi.SetActive(false);

        levelIndex = PlayerPrefs.GetInt("boardToLoad", 0);
        GameObject boardToInst = null;
        if (levelIndex < level_boards_easy.Length) {
            difficultyTxt.text = "Easy";
            boardToInst = level_boards_easy[levelIndex];
        } else if (level_boards_easy.Length <= levelIndex && levelIndex < level_boards_medium.Length) {
            difficultyTxt.text = "Medium";
            boardToInst = level_boards_medium[levelIndex-25];
        } else if (level_boards_medium.Length <= levelIndex && levelIndex < level_boards_hard.Length) {
            difficultyTxt.text = "Hard";
            boardToInst = level_boards_hard[levelIndex - 50];
        } else if (level_boards_hard.Length <= levelIndex) {
            difficultyTxt.text = "Expert";
            boardToInst = level_boards_expert[levelIndex - 75];
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
        AudioManager.Instance.PlayButtonClick();
        sceneFader.FadeToBuildIndex(0);
    }

    public virtual void IncreaseMoves(int amount) {
        // Base method for Challenge Mode
    }

    public virtual void DecreaseMoves(int amount) {
        // Base method for Challange Mode
    }

    public void RetryLevel() {
        StartCoroutine(PreLoadLevel());
    }

    public void LoadNextLevel() {
        saveManager.SaveIntData("boardToLoad", levelIndex + 1);
        StartCoroutine(PreLoadLevel());
    }

    public IEnumerator PreLoadLevel() {
        AudioManager.Instance.PlayButtonClick();
        sceneFader.FadeBetweenObjects();

        float t = 0f;

        while (t < 0.5f) {
            t += Time.deltaTime;
            yield return 0;
        }

        LoadLevel();
    }
}