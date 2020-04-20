using System.Collections;
using UnityEngine;

public class DailyChallenge_GM : GameManager {

    public override void LoadLevel() {
        base.LoadLevel();
    }

    public override void LevelComplete() {
        base.LevelComplete();
        if (levelIndex < boardLength) {
            saveManager.SaveIntData("DailyChallenge1Completed", 1);
        } else if (boardLength <= levelIndex && levelIndex < (boardLength * 2)) {
            saveManager.SaveIntData("DailyChallenge2Completed", 1);
        } else if ((boardLength * 2) <= levelIndex && levelIndex < (boardLength * 3)) {
            saveManager.SaveIntData("DailyChallenge3Completed", 1);
        } else if ((boardLength * 3) <= levelIndex) {
            saveManager.SaveIntData("DailyChallenge4Completed", 1);
        }

        StartCoroutine(DelayedLevelSelector());
    }
}