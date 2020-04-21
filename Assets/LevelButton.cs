using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour {

    public Button button;
    public TextMeshProUGUI text;
    public GameObject checkMark;
    public GameObject locked;

    public void Unavailable() {
        text.gameObject.SetActive(false);
        checkMark.SetActive(false);
    }

    public void Finished(int level) {
        locked.SetActive(false);
        BtnStandard(level);
    }

    public void NextLevel(int level) {
        locked.SetActive(false);
        checkMark.SetActive(false);
        BtnStandard(level);
    }

    private void BtnStandard(int level) {
        text.text = level.ToString();
    }
}
