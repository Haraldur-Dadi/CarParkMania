using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Challenge_GM : GameManager {
    public TextMeshProUGUI movesTxt;
    public TextMeshProUGUI completedTxt;
    public TextMeshProUGUI completedMovesTxt;
    public TextMeshProUGUI movesNextStarTxt;
    public GameObject nextLvlBtn;

    public Image star;
    public Sprite blankStar;
    public Sprite goldStar;
    public Sprite silverStar;
    public Sprite bronzeStar;

    public int moves;
    public ChallengeLevelDb challengeLevelDb;

    public override void Awake() {
        if (CrossSceneManager.Instance.gameModeNr == 2 || CrossSceneManager.Instance.gameModeNr == 4) {
            base.Awake();
        } else {
            Destroy(this);
        }
    }

    public override void LoadLevel() {
        base.LoadLevel();
        moves = 0;
        movesTxt.text = "Moves: " + moves;
    }

    public override void ChangeMoves(bool increase) {
        moves += increase ? 1 : -1;
        movesTxt.text = "Moves: " + moves;
    }

    public override void LevelComplete() {
        levelCompleteUi.SetActive(true);
        PlayerPrefs.SetInt("LevelsCompleted", PlayerPrefs.GetInt("LevelsCompleted", 1) + 1);

        ChallengeLevel currLevel = challengeLevelDb.GetChallengeLevel(levelIndex);
        if (moves < currLevel.minMoves + 6) {
            string mode = (CrossSceneManager.Instance.gameModeNr == 2) ? "ChallengeLevelReached" : "8x8ChallengeLevelReached";
            if (PlayerPrefs.GetInt(mode, 0) <= levelIndex) {
                PlayerPrefs.SetInt(mode, levelIndex + 1);
            }
            
            levelCompleteUi.GetComponent<Image>().color = new Color32(30,236,34,245);

            CarMovement carMovement = GetComponent<CarMovement>();
            if (carMovement.carToMove != carMovement.undoList[carMovement.undoList.Count - 1].car) {
                moves += 1;
            }

            completedMovesTxt.text = "Moves: " + (moves);
            if (moves <= currLevel.minMoves) {
                star.sprite = goldStar;
                movesNextStarTxt.text = "";
                PlayerPrefs.SetInt(mode + levelIndex + "Stars", 3);
            } else if (moves <= currLevel.minMoves + 2) {
                star.sprite = silverStar;
                movesNextStarTxt.text = "Next star: " + currLevel.minMoves;
                if (PlayerPrefs.GetInt(mode + levelIndex + "Stars") < 2)
                    PlayerPrefs.SetInt(mode + levelIndex + "Stars", 2);
            } else {
                star.sprite = bronzeStar;
                movesNextStarTxt.text = "Next star: " + (currLevel.minMoves + 2);
                if (PlayerPrefs.GetInt(mode + levelIndex + "Stars") < 1)
                    PlayerPrefs.SetInt(mode + levelIndex + "Stars", 1);
            }
        } else {
            levelCompleteUi.GetComponent<Image>().color = new Color32(236,51,30,245);
            star.sprite = blankStar;
            movesNextStarTxt.text = "Minimum moves: " + (currLevel.minMoves + 6);
        }
        completedTxt.text = (moves <= currLevel.minMoves + 5) ? "Level Completed!" : "You lost!";
        nextLvlBtn.SetActive(moves <= currLevel.minMoves + 5);
    }
}
