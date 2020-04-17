using System.Collections;
using UnityEngine;

public class Casual_GM : GameManager {

    public override void LoadLevel() {
        base.LoadLevel();
        levelTxt.text = (levelIndex + 1).ToString();
    }

    public override void LevelComplete() {
        base.LevelComplete();

        if (PlayerPrefs.GetInt("CasualLevelReached", 0) <= levelIndex) {
            saveManager.SaveIntData("CasualLevelReached", levelIndex + 1);
            saveManager.IncreaseAchivementProgress(0);
            saveManager.IncreaseAchivementProgress(1);
        }

        saveManager.SaveIntData("boardToLoad", levelIndex + 1);
        StartCoroutine(CountdownNextLevel());
    }

    IEnumerator CountdownNextLevel() {
        yield return new WaitForSeconds(2);
        StartCoroutine(PreLoadLevel());
    }
}
