using System.Collections;
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
        completedTxt.text = winMessages[Random.Range(0, winMessages.Length)];
        string mode = (CrossSceneManager.Instance.gameModeNr == 1) ? "CasualLevelReached" : "8x8LevelReached";
        if (PlayerPrefs.GetInt(mode, 0) <= levelIndex) {
            PlayerPrefs.SetInt(mode, levelIndex + 1);
        }
        
        AchivementManager.Instance.IncreaseAchivementProgress(0);
        AchivementManager.Instance.IncreaseAchivementProgress(1);
        StartCoroutine(CountdownNextLevel());
    }
    public IEnumerator CountdownNextLevel() {
        yield return new WaitForSeconds(2);
        LoadNextLevel();
    }
}
