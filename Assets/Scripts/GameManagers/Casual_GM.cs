using UnityEngine;

public class Casual_GM : GameManager {
    public override void Awake() {
        if (CrossSceneManager.Instance.gameModeNr == 1 || CrossSceneManager.Instance.gameModeNr == 3) {
            base.Awake();
        } else {
            Destroy(this);
        }
    }

    public override void LevelComplete() {
        base.LevelComplete();
        if (PlayerPrefs.GetInt("CasualLevelReached", 0) <= levelIndex) {
            PlayerPrefs.SetInt("CasualLevelReached", levelIndex + 1);
        }
        
        AchivementManager.Instance.IncreaseAchivementProgress(0);
        AchivementManager.Instance.IncreaseAchivementProgress(1);
        LoadNextLevel();
    }
}
