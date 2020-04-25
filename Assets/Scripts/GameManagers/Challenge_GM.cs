using UnityEngine;
using TMPro;

public class Challenge_GM : GameManager {
    public TextMeshProUGUI movesTxt;
    public TextMeshProUGUI completedTxt;
    public TextMeshProUGUI completedMovesTxt;
    public TextMeshProUGUI movesNextStarTxt;
    public GameObject nextLvlBtn;

    public GameObject[] stars;
    public Sprite blankStar;
    public Sprite goldStar;
    public Sprite silverStar;
    public Sprite bronzeStar;

    public int moves;

    public override void LoadLevel() {
        base.LoadLevel();
        moves = 0;
        movesTxt.text = "Moves: " + moves;
    }

    public override void LevelComplete() {
        base.LevelComplete();
        completedTxt.text = "Level Completed!";
        completedMovesTxt.text = "Moves: " + moves;

        if (PlayerPrefs.GetInt("ChallengeLevelReached", 0) <= levelIndex) {
            saveManager.SaveIntData("ChallengeLevelReached", levelIndex + 1);
        }
        
        saveManager.SaveIntData("boardToLoad", levelIndex + 1);

        if (levelIndex + 1 <= 99) {
            StartCoroutine(CountdownNextLevel());
        } else {
            StartCoroutine(DelayedLevelSelector());
        }
    }
}
