using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    public Sprite board_8x8;
    public Transform finishLine;

    public int levelIndex;
    public int difficultySize;
    public GameObject level_board;
    public GameObject[] levels_6x6;
    public GameObject[] levels_8x8;

    public Button homeBtn;
    public Button retryBtn;

    public GameObject levelCompleteUi;
    public TextMeshProUGUI levelTxt;
    public TextMeshProUGUI difficultyTxt;
    public TextMeshProUGUI completedTxt;
    public string[] winMessages;

    public virtual void Awake() {
        Instance = this;
        if (CrossSceneManager.Instance.gameModeNr > 2) {
            GetComponent<SpriteRenderer>().sprite = board_8x8;
            finishLine.position += new Vector3(1.1f, 0f, 0f);
            Camera.main.orthographicSize += 1.9f;
        }
        difficultySize = (CrossSceneManager.Instance.gameModeNr > 0) ? 25 : 14;

        homeBtn.onClick.AddListener(delegate { GoToLevelSelector(); });
        retryBtn.onClick.AddListener(delegate { RetryLevel(); });
        LoadLevel();
    }

    public virtual void LoadLevel() {
        /* Loads up the current board */
        GetComponent<CarMovement>().Refresh();
        levelCompleteUi.SetActive(false);

        levelIndex = PlayerPrefs.GetInt("boardToLoad", 0);
        levelTxt.text = "- " + (levelIndex + 1) + " -";
        if (levelIndex < difficultySize) {
            difficultyTxt.text = "Easy";
            CrossSceneManager.Instance.difficulty = 0;
        } else if (levelIndex < (difficultySize*2)) {
            difficultyTxt.text = "Medium";
            CrossSceneManager.Instance.difficulty = 1;
        } else if (levelIndex < (difficultySize*3)) {
            difficultyTxt.text = "Hard";
            CrossSceneManager.Instance.difficulty = 2;
        } else {
            difficultyTxt.text = "Expert";
            CrossSceneManager.Instance.difficulty = 3;
        }
        if (CrossSceneManager.Instance.gameModeNr < 3) {
            level_board = Instantiate(levels_6x6[levelIndex], transform);
        } else {
            level_board = Instantiate(levels_8x8[levelIndex], transform);
        }
        GameObject.FindWithTag("Car").GetComponent<SpriteRenderer>().sprite = ItemDb.Instance.GetItem(PlayerPrefs.GetInt("PlayerCarEquipped", 0)).sprite;
    }

    public virtual void ChangeMoves(bool increase) {}
    public virtual void LevelComplete() {
        AudioManager.Instance.PlayWinSound();

        if (levelIndex < difficultySize)
            AchivementManager.Instance.IncreaseAchivementProgress(4);
        AchivementManager.Instance.IncreaseAchivementProgress(5);
        AchivementManager.Instance.IncreaseAchivementProgress(6);

        levelCompleteUi.SetActive(true);
    }

    public void GoToLevelSelector() {
        /* Sends player to home screen */
        AudioManager.Instance.PlayButtonClick();
        CrossSceneManager.Instance.FadeToBuildIndex(0);
    }
    public void RetryLevel() { 
        /* Reloads the same level */
        AudioManager.Instance.PlayButtonClick();
        StartCoroutine(PreLoadLevel());
    }
    public void LoadNextLevel() {
        /* Loads up next level */
        PlayerPrefs.SetInt("boardToLoad", levelIndex + 1);
        StartCoroutine(PreLoadLevel());
    }

    public IEnumerator PreLoadLevel() {
        if (CrossSceneManager.Instance.gameModeNr < 3 && levelIndex == (levels_6x6.Length-1) || CrossSceneManager.Instance.gameModeNr > 2 && levelIndex == (levels_8x8.Length-1)) {
            GoToLevelSelector();
        }
        if (PlayerPrefs.GetInt("GamesPlayed", 0) % 7 == 0) {
            AdManager.Instance.ShowVideoAd();
        }
        PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed", 0) + 1);

        CrossSceneManager.Instance.FadeBetweenObjects(1.25f);
        Destroy(level_board);
        yield return new WaitForSeconds(1f);
        LoadLevel();
    }
    public IEnumerator DelayedLevelSelector() {
        yield return new WaitForSeconds(2);
        GoToLevelSelector();
    }
}