using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Challenge_GM : GameManager {
    public TextMeshProUGUI movesTxt;
    public TextMeshProUGUI completedMovesTxt;
    public TextMeshProUGUI movesNextStarTxt;
    public string[] looseMessages;
    public GameObject nextLvlBtn;

    public GameObject infoPanel;
    public TextMeshProUGUI goldMoves;
    public TextMeshProUGUI silverMoves;
    public TextMeshProUGUI bronzeMoves;

    public Image star;
    public Sprite blankStar;
    public Sprite goldStar;
    public Sprite silverStar;
    public Sprite bronzeStar;

    public int moves;
    private int currLevelMoves;
    public ChallengeLevelDb challengeLevelDb;

    public override void Awake() {
        if (CrossSceneManager.Instance.gameModeNr == 2 || CrossSceneManager.Instance.gameModeNr == 4) {
            base.Awake();
        } else {
            Destroy(this);
        }
    }

    public void HideInfoPanel() {
        infoPanel.SetActive(false);
    }

    public override void LoadLevel() {
        base.LoadLevel();
        infoPanel.SetActive(true);
        currLevelMoves = challengeLevelDb.GetChallengeLevel(levelIndex);
        goldMoves.text = currLevelMoves.ToString();
        silverMoves.text = (currLevelMoves+1) + " - " + (currLevelMoves+3);
        bronzeMoves.text = (currLevelMoves+4) + " - " + (currLevelMoves+7);

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
        CarMovement carMovement = GetComponent<CarMovement>();
        if (carMovement.undoList.Count > 0) {
            if (carMovement.carToMove != carMovement.undoList[carMovement.undoList.Count - 1].car) {
                moves += 1;
            }
        }
        completedMovesTxt.text = "Moves: " + (moves);

        if (moves <= currLevelMoves + 7) {
            completedTxt.text = winMessages[Random.Range(0, winMessages.Length)];
            string mode = (CrossSceneManager.Instance.gameModeNr == 2) ? "Challenge" : "8x8 Challenge";
            if (PlayerPrefs.GetInt(mode + "LevelReached", 0) <= levelIndex) {
                PlayerPrefs.SetInt(mode + "LevelReached", levelIndex + 1);
            }

            levelCompleteUi.GetComponent<Image>().color = new Color32(30,236,34,245);
            if (moves <= currLevelMoves) {
                star.sprite = goldStar;
                movesNextStarTxt.text = "";
                PlayerPrefs.SetInt(mode + levelIndex + "Stars", 3);
            } else if (moves <= currLevelMoves + 3) {
                star.sprite = silverStar;
                movesNextStarTxt.text = "Next star: " + currLevelMoves;
                if (PlayerPrefs.GetInt(mode + levelIndex + "Stars") < 2)
                    PlayerPrefs.SetInt(mode + levelIndex + "Stars", 2);
            } else {
                star.sprite = bronzeStar;
                movesNextStarTxt.text = "Next star: " + (currLevelMoves + 3);
                if (PlayerPrefs.GetInt(mode + levelIndex + "Stars") < 1)
                    PlayerPrefs.SetInt(mode + levelIndex + "Stars", 1);
            }
            nextLvlBtn.SetActive(true);
        } else {
            completedTxt.text = looseMessages[Random.Range(0, looseMessages.Length)];
            levelCompleteUi.GetComponent<Image>().color = new Color32(236,51,30,245);
            star.sprite = blankStar;
            movesNextStarTxt.text = "Minimum moves: " + (currLevelMoves + 7);
            nextLvlBtn.SetActive(false);
        }
    }
}
