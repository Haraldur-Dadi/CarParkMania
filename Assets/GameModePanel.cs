using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModePanel : MonoBehaviour {
    public string gameModeName;
    public int gameModeScene;
    public LevelButton[] levelButtons;
    private int levelReached;

    private void Start() {
        levelReached = PlayerPrefs.GetInt(gameModeName + "LevelReached", 0);
        ChangeDifficulty(0);
    }

    public void ChangeDifficulty(int difficulty) {
        for (int i = 0; i < levelButtons.Length; i++) {
            int iValue = i + (25 * difficulty);
            
            if (iValue > levelReached) {
                levelButtons[i].Unavailable(iValue);
            } else {
                levelButtons[i].button.onClick.AddListener(() => SelectLevel(gameModeScene, iValue));

                if (iValue < levelReached) {
                    levelButtons[i].Finished(iValue);
                } else {
                    levelButtons[i].NextLevel(iValue);
                }
            }
        }
    }

    public void SelectLevel(int buildIndex, int level) {
        CrossSceneManager.Instance.TmpPreventClicks();
        SaveManager.Instance.SaveIntData("boardToLoad", level);
        SceneFader.Instance.FadeToBuildIndex(buildIndex);
    }
}
