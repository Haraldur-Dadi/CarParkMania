using UnityEngine;
using UnityEngine.UI;

public class GameModePanel : MonoBehaviour {
    public string gameModeName;
    public LevelButton[] levelBtns;
    public Button[] difficultyBtns;
    private int levelReached;

    private void Start() {
        levelReached = PlayerPrefs.GetInt(gameModeName + "LevelReached", 0);
        SelectDifficulty(CrossSceneManager.Instance.difficulty);
    }

    public void SelectDifficulty(int difficulty) {
        CrossSceneManager.Instance.difficulty = difficulty;
        
        for (int i = 0; i < levelBtns.Length; i++) {
            int iValue = i + (25 * difficulty);
            
            if (iValue > levelReached) {
                levelBtns[i].Unavailable(iValue);
            } else {
                levelBtns[i].button.onClick.AddListener(() => SelectLevel(CrossSceneManager.Instance.gameModeNr, iValue));

                if (iValue < levelReached) {
                    levelBtns[i].Finished(iValue);
                } else {
                    levelBtns[i].NextLevel(iValue);
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
