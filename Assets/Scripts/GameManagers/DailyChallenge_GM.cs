using System.Collections;
using UnityEngine;

public class DailyChallenge_GM : GameManager {

    public override void LoadLevel() {
        base.LoadLevel();
    }

    public override void LevelComplete() {
        base.LevelComplete();
        if (levelIndex < boardLength) {
            SaveManager.Instance.SaveIntData("DailyChallenge1Completed", 1);
        } else if (boardLength <= levelIndex && levelIndex < (boardLength * 2)) {
            SaveManager.Instance.SaveIntData("DailyChallenge2Completed", 1);
        } else if ((boardLength * 2) <= levelIndex && levelIndex < (boardLength * 3)) {
            SaveManager.Instance.SaveIntData("DailyChallenge3Completed", 1);
        } else if ((boardLength * 3) <= levelIndex) {
            SaveManager.Instance.SaveIntData("DailyChallenge4Completed", 1);
        }

        StartCoroutine(DelayedLevelSelector());
    }
}