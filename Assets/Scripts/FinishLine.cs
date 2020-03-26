using UnityEngine;

public class FinishLine : MonoBehaviour {
    public GameManager gameManager;

    private void Start() {
        gameManager = GameManager.instance;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        gameManager.LevelComplete();
    }
}
