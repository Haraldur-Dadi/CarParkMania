using UnityEngine;
using UnityEngine.UI;

public class GameModePanel : MonoBehaviour {
    public string gameModeName;
    public LevelButton[] levelBtns;
    public Button[] difficultyBtns;
    private int levelReached;

    public Sprite blankStar;
    public Sprite bronzeStar;
    public Sprite silverStar;
    public Sprite goldStar;

    private void Start() {
        SelectDifficulty(CrossSceneManager.Instance.difficulty);
    }

    public void SelectDifficulty(int difficulty) {
        levelReached = PlayerPrefs.GetInt(gameModeName + "LevelReached", 0);
        CrossSceneManager.Instance.difficulty = difficulty;
        
        for (int i = 0; i < levelBtns.Length; i++) {
            int iValue = i + (25 * difficulty);
            
            if (iValue > levelReached) {
                levelBtns[i].Unavailable(iValue);
                if (gameModeName == "Challenge") {
                    levelBtns[i].star.enabled = false;
                }
            } else {
                levelBtns[i].button.onClick.AddListener(() => SelectLevel(CrossSceneManager.Instance.gameModeNr + 1, iValue));
                if (gameModeName == "Casual") {
                    if (iValue < levelReached) {
                        levelBtns[i].Finished(iValue);
                    } else {
                        levelBtns[i].NextLevel(iValue);
                    }
                } else {
                    levelBtns[i].NextLevel(iValue);
                    levelBtns[i].star.enabled = true;
                    int stars = PlayerPrefs.GetInt("Challenge" + iValue + "Stars", 0);
                    if (stars == 0) {
                        levelBtns[i].star.sprite = blankStar;
                    } else if (stars == 1) {
                        levelBtns[i].star.sprite = bronzeStar;
                    } else if (stars == 2) {
                        levelBtns[i].star.sprite = silverStar;
                    } else {
                        levelBtns[i].star.sprite = goldStar;
                    }
                }

            }
        }

        for (int i = 0; i < difficultyBtns.Length; i++) {
            if (i == difficulty) {
                difficultyBtns[i].interactable = false;
            } else {
                difficultyBtns[i].interactable = true;
            }
        }
    }

    public void SelectLevel(int buildIndex, int level) {
        CrossSceneManager.Instance.TmpPreventClicks();
        SaveManager.Instance.SaveIntData("boardToLoad", level);
        SceneFader.Instance.FadeToBuildIndex(buildIndex);
    }
}
