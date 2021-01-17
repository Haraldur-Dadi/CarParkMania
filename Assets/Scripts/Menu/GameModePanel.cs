using UnityEngine;
using UnityEngine.UI;

public class GameModePanel : MonoBehaviour {
    public Scrollbar scrollbar;
    public LevelButton[] levelBtns;
    public Button[] difficultyBtns;
    string gameModeName;

    int avail_8x8_levels = 20;

    public void Display(string gameMode) {
        gameModeName = gameMode;
        gameObject.SetActive(true);
        SelectDifficulty(CrossSceneManager.Instance.difficulty);
    }

    public void SelectDifficulty(int difficulty) {
        int levelReached = PlayerPrefs.GetInt(gameModeName + "LevelReached", 0);
        CrossSceneManager.Instance.difficulty = difficulty;
        scrollbar.value = 1;
        
        for (int i = 0; i < levelBtns.Length; i++) {
            int iValue = i + (25 * difficulty);
            
            if (gameModeName.Contains("8x8") && iValue >= avail_8x8_levels) {
                levelBtns[i].gameObject.SetActive(false);
            } else if (iValue > levelReached) {
                levelBtns[i].gameObject.SetActive(true);
                levelBtns[i].Unavailable();
            } else {
                levelBtns[i].gameObject.SetActive(true);
                levelBtns[i].button.onClick.AddListener(() => SelectLevel(iValue));
                if ((gameModeName == "Casual" || gameModeName == "8x8") && iValue < levelReached) {
                    levelBtns[i].Finished();
                } else {
                    levelBtns[i].NextLevel();
                }
                if (gameModeName == "Challenge" || gameModeName == "8x8 Challenge") {
                    levelBtns[i].SetStar(PlayerPrefs.GetInt(gameModeName + iValue + "Stars", 0));
                }
            }
            levelBtns[i].levelTxt.text = (iValue + 1).ToString();
        }

        for (int i = 0; i < difficultyBtns.Length; i++) {
            difficultyBtns[i].interactable = (gameModeName.Contains("8x8")) ? false : (i != difficulty);
        }
    }

    public void SelectLevel(int level) {
        AudioManager.Instance.PlayButtonClick();
        PlayerPrefs.SetInt("boardToLoad", level);
        CrossSceneManager.Instance.FadeToBuildIndex(1);
    }
}
