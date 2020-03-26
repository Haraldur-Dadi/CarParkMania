using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour {

    public SceneFader sceneFader;

    private void Start() {
        sceneFader = SceneFader.Instance;

    }

    void Update() {
        if (Input.touchCount > 0) {
            sceneFader.FadeToBuildIndex(1);
        }
    }
}
