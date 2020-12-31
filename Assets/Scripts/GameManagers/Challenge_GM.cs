using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Challenge_GM : GameManager {
    public TextMeshProUGUI movesTxt;
    public TextMeshProUGUI completedTxt;
    public TextMeshProUGUI completedMovesTxt;
    public TextMeshProUGUI movesNextStarTxt;
    public GameObject nextLvlBtn;

    public Image[] stars;
    public Sprite blankStar;
    public Sprite goldStar;
    public Sprite silverStar;
    public Sprite bronzeStar;

    public int moves;
    public ChallengeLevel currLevel;
    public ChallengeLevelDb challengeLevelDb;

    public override void LoadLevel() {
        base.LoadLevel();
        levelTxt.text = "- " + (levelIndex + 1) + " -";
        currLevel = challengeLevelDb.GetChallengeLevel(levelIndex);
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

        if (moves <= currLevel.minMoves + 4) {
            if (PlayerPrefs.GetInt("ChallengeLevelReached", 0) <= levelIndex)
                PlayerPrefs.SetInt("ChallengeLevelReached", levelIndex + 1);
            
            levelCompleteUi.GetComponent<Image>().color = new Color32(30,236,34,245);

            CarMovement carMovement = GetComponent<CarMovement>();
            if (carMovement.carToMove != carMovement.undoList[carMovement.undoList.Count - 1].car) {
                moves += 1;
            }

            completedMovesTxt.text = "Moves: " + (moves);
            if (moves <= currLevel.minMoves) {
                foreach (Image s in stars) {
                    s.sprite = goldStar;
                }
                movesNextStarTxt.text = "";
                PlayerPrefs.SetInt("Challenge" + levelIndex + "Stars", 3);
            } else if (moves <= currLevel.minMoves + 2) {
                stars[0].sprite = silverStar;
                stars[1].sprite = silverStar;
                stars[2].sprite = blankStar;
                movesNextStarTxt.text = "Next star: " + currLevel.minMoves;
                if (PlayerPrefs.GetInt("Challenge" + levelIndex + "Stars") < 2)
                    PlayerPrefs.SetInt("Challenge" + levelIndex + "Stars", 2);
            } else {
                stars[0].sprite = bronzeStar;
                stars[1].sprite = blankStar;
                stars[2].sprite = blankStar;
                movesNextStarTxt.text = "Next star: " + (currLevel.minMoves + 2);
                if (PlayerPrefs.GetInt("Challenge" + levelIndex + "Stars") < 1)
                    PlayerPrefs.SetInt("Challenge" + levelIndex + "Stars", 1);
            }
        } else {
            levelCompleteUi.GetComponent<Image>().color = new Color32(236,51,30,245);
            foreach (Image s in stars) {
                s.sprite = blankStar;
            }
            movesNextStarTxt.text = "Minimum moves: " + (currLevel.minMoves + 5);
        }
        completedTxt.text = (moves <= currLevel.minMoves + 4) ? "Level Completed!" : "You lost!";
        nextLvlBtn.SetActive(moves <= currLevel.minMoves + 4);
    }
}
