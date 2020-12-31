using UnityEngine;

public class DailyChallenge_GM : GameManager {
    public override void LevelComplete() {
        base.LevelComplete();
        if (levelIndex < boardLength) {
            PlayerPrefs.SetInt("DailyChallenge1Completed", 1);
        } else if (boardLength <= levelIndex && levelIndex < (boardLength * 2)) {
            PlayerPrefs.SetInt("DailyChallenge2Completed", 1);
        } else if ((boardLength * 2) <= levelIndex && levelIndex < (boardLength * 3)) {
            PlayerPrefs.SetInt("DailyChallenge3Completed", 1);
        } else if ((boardLength * 3) <= levelIndex) {
            PlayerPrefs.SetInt("DailyChallenge4Completed", 1);
        }

        StartCoroutine(DelayedLevelSelector());
    }
}