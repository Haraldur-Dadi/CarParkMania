using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    public int levelIndex;
    public int boardLength;

    public GameObject level_board;
    public GameObject[] level_boards_easy;
    public GameObject[] level_boards_medium;
    public GameObject[] level_boards_hard;
    public GameObject[] level_boards_expert;

    public GameObject levelCompleteUi;
    public TextMeshProUGUI levelTxt;
    public TextMeshProUGUI difficultyTxt;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            if (CrossSceneManager.Instance.gameModeNr > 1) {
                Camera.main.orthographicSize += 1.9f;
            }
            boardLength = level_boards_easy.Length;
            LoadLevel();
        } else {
            Destroy(this);
        }
    }

    public virtual void LoadLevel() {
        GetComponent<CarMovement>().Refresh();
        levelCompleteUi.SetActive(false);

        levelIndex = PlayerPrefs.GetInt("boardToLoad", 0);
        if (levelIndex < boardLength) {
            difficultyTxt.text = "Easy";
            CrossSceneManager.Instance.difficulty = 0;
        } else if (levelIndex < (boardLength * 2)) {
            difficultyTxt.text = "Medium";
            CrossSceneManager.Instance.difficulty = 1;
        } else if (levelIndex < (boardLength * 3)) {
            difficultyTxt.text = "Hard";
            CrossSceneManager.Instance.difficulty = 2;
        } else if (levelIndex >= (boardLength * 3)) {
            difficultyTxt.text = "Expert";
            CrossSceneManager.Instance.difficulty = 3;
        }
        level_board = Instantiate(level_boards_easy[levelIndex - (level_boards_easy.Length * CrossSceneManager.Instance.difficulty)], transform);
        GameObject.FindWithTag("Car").GetComponent<SpriteRenderer>().sprite = ItemDb.Instance.GetItem(PlayerPrefs.GetInt("PlayerCarEquipped", 0)).sprite;
    }

    public virtual void ChangeMoves(bool increase) {}
    public virtual void LevelComplete() {
        AudioManager.Instance.PlayWinSound();

        if (levelIndex < level_boards_easy.Length)
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
        AudioManager.Instance.PlayButtonClick();
        StartCoroutine(PreLoadLevel());
    }
    public void LoadNextLevel() {
        PlayerPrefs.SetInt("boardToLoad", levelIndex + 1);
        StartCoroutine(PreLoadLevel());
    }

    public IEnumerator PreLoadLevel() {
        if (PlayerPrefs.GetInt("GamesPlayed", 0) % 7 == 0)
            AdManager.Instance.ShowVideoAd();
        PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed", 0) + 1);

        CrossSceneManager.Instance.FadeBetweenObjects();
        Destroy(level_board);
        yield return new WaitForSeconds(0.33f);
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