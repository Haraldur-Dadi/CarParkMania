using UnityEngine;

public class DailyChallenge_GM : GameManager {
    public override void Awake() {
        if (CrossSceneManager.Instance.gameModeNr == 0) {
            base.Awake();
        } else {
            Destroy(this);
        }
    }

    public override void LoadLevel() {
        base.LoadLevel();
        levelTxt.text = "";
    }

    public override void LevelComplete() {
        base.LevelComplete();
        if (levelIndex < 14) {
            PlayerPrefs.SetInt("DailyChallenge1Completed", 1);
        } else if (levelIndex < 28) {
            PlayerPrefs.SetInt("DailyChallenge2Completed", 1);
        } else if (levelIndex < 42) {
            PlayerPrefs.SetInt("DailyChallenge3Completed", 1);
        } else {
            PlayerPrefs.SetInt("DailyChallenge4Completed", 1);
        }

        StartCoroutine(DelayedLevelSelector());
    }
}