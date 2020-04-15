using UnityEngine;
using TMPro;

public class Challange_GM : GameManager {

    public TextMeshProUGUI movesTxt;
    public int moves;

    public override void Start() {
        base.Start();
        movesTxt = GameObject.Find("MovesTxt").GetComponent<TextMeshProUGUI>();
        movesTxt.text = "Moves: 0";
    }

    public override void IncreaseMoves(int increaseBy) {
        moves += increaseBy;
        movesTxt.text = string.Format("Moves: {0}", moves);
    }

    public override void DecreaseMoves(int decreaseBy) {
        moves -= decreaseBy;
        movesTxt.text = string.Format("Moves: {0}", moves);
    }
}
