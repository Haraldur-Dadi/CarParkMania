using UnityEngine;

public class Casual_GM : GameManager {
    public override void LoadLevel() {
        base.LoadLevel();
        levelTxt.text = "- " + (levelIndex + 1) + " -";
    }

    public override void LevelComplete() {
        base.LevelComplete();
        if (PlayerPrefs.GetInt("CasualLevelReached", 0) <= levelIndex) {
            SaveManager.Instance.SaveIntData("CasualLevelReached", levelIndex + 1);
        }
        
        SaveManager.Instance.IncreaseAchivementProgress(0);
        SaveManager.Instance.IncreaseAchivementProgress(1);
        if (levelIndex < 99) {
            StartCoroutine(CountdownNextLevel());
        } else {
            StartCoroutine(DelayedLevelSelector());
        }
    }
}
