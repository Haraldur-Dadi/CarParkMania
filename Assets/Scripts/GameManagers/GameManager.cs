﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    public int levelIndex;
    public int boardLength;

    public GameObject gameBoard;
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
            boardLength = level_boards_easy.Length;
            LoadLevel();
        } else {
            Destroy(this);
        }
    }

    public virtual void LoadLevel() {
        GetComponent<CarMovement>().Refresh();
        if (level_board != null)
            Destroy(level_board);
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
                carObj.carImg.sprite = ItemDb.Instance.GetItem(PlayerPrefs.GetInt("PlayerCarEquipped", 0)).sprite;
            }
        }
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
        if (PlayerPrefs.GetInt("GamesPlayed", 0) % 10 == 0)
            AdManager.Instance.ShowVideoAd();
        PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed", 0) + 1);
        
        CrossSceneManager.Instance.FadeBetweenObjects();
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