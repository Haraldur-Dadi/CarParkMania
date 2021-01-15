using UnityEngine;

public class Casual_GM : GameManager {
    public override void LoadLevel() {
        base.LoadLevel();
        levelTxt.text = "- " + (levelIndex + 1) + " -";
    }

    public override void LevelComplete() {
        base.LevelComplete();
        if (PlayerPrefs.GetInt("CasualLevelReached", 0) <= levelIndex) {
            PlayerPrefs.SetInt("CasualLevelReached", levelIndex + 1);
        }
        
        AchivementManager.Instance.IncreaseAchivementProgress(0);
        AchivementManager.Instance.IncreaseAchivementProgress(1);
        if (levelIndex < (level_boards_easy.Length + level_boards_medium.Length + level_boards_hard.Length + level_boards_expert.Length) - 1) {
            StartCoroutine(CountdownNextLevel());
        } else {
            StartCoroutine(DelayedLevelSelector());
        }
    }
}
