using UnityEngine;
using UnityEngine.UI;

public class GameModePanel : MonoBehaviour {
    public string gameModeName;
    public int gameModeScene;
    public LevelButton[] levelBtns;
    public Button[] difficultyBtns;
    public Scrollbar scrollbar;
    private int levelReached;

    public int selectedDifficulty;

    private void Start() {
        levelReached = PlayerPrefs.GetInt(gameModeName + "LevelReached", 0);
        ChangeDifficulty(0);
    }

    public void ChangeDifficulty(int difficulty) {
        selectedDifficulty = difficulty;
        if (scrollbar)
            scrollbar.value = 1;
        
        for (int i = 0; i < levelBtns.Length; i++) {
            int iValue = i + (25 * difficulty);
            
            if (iValue > levelReached) {
                levelBtns[i].Unavailable(iValue);
            } else {
                levelBtns[i].button.onClick.AddListener(() => SelectLevel(gameModeScene, iValue));

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
