using UnityEngine;
using UnityEngine.UI;

public class GameModePanel : MonoBehaviour {
    public Scrollbar scrollbar;
    public LevelButton[] levelBtns;
    public Button[] difficultyBtns;

    public void Hide() { gameObject.SetActive(false); }
    public void Display() {
        gameObject.SetActive(true);
        SelectDifficulty(CrossSceneManager.Instance.difficulty);
    }

    public void SelectDifficulty(int difficulty) {
        string gameModeName = (CrossSceneManager.Instance.gameModeNr == 1) ? "Casual" : "Challenge";
        int levelReached = PlayerPrefs.GetInt(gameModeName + "LevelReached", 0);
        CrossSceneManager.Instance.difficulty = difficulty;
        scrollbar.value = 1;
        
        for (int i = 0; i < levelBtns.Length; i++) {
            int iValue = i + (25 * difficulty);
            
            if (iValue > levelReached) {
                levelBtns[i].Unavailable();
            } else {
                levelBtns[i].button.onClick.AddListener(() => SelectLevel(iValue));
                if (gameModeName == "Casual" && iValue < levelReached) {
                    levelBtns[i].Finished();
                } else {
                    levelBtns[i].NextLevel();
                }
                if (gameModeName == "Challenge") {
                    levelBtns[i].SetStar(PlayerPrefs.GetInt("Challenge" + iValue + "Stars", 0));
                }
            }
            levelBtns[i].levelTxt.text = (iValue + 1).ToString();
        }

        for (int i = 0; i < difficultyBtns.Length; i++) {
            difficultyBtns[i].interactable = (i != difficulty);
        }
    }

    public void SelectLevel(int level) {
        AudioManager.Instance.PlayButtonClick();
        SaveManager.Instance.SaveIntData("boardToLoad", level);
        CrossSceneManager.Instance.FadeToBuildIndex(CrossSceneManager.Instance.gameModeNr + 1);
    }
}
