using UnityEngine;
using UnityEngine.UI;

public class CanvasScaling : MonoBehaviour {

    public CanvasScaler canvasScaler;

    void Awake () {
        float aspect = (float)Screen.height / (float)Screen.width; // Portrait

        if (aspect >= 1.87) {
            // 19.5:9
            Camera.main.orthographicSize = 8.25f;
            canvasScaler.matchWidthOrHeight = 1;
        }
        else if (aspect >= 1.74) {
            // 16:9
            Camera.main.orthographicSize = 6.75f;
            canvasScaler.matchWidthOrHeight = 0;
        }
        else if (aspect >= 1.5) {
            // 3:2
            Camera.main.orthographicSize = 5.75f;
            canvasScaler.matchWidthOrHeight = 1;
        }
        else {
            // 4:3
            Camera.main.orthographicSize = 6f;
            canvasScaler.matchWidthOrHeight = 1f;
        }
    }
}
