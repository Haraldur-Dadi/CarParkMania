using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour {

    public Button button;
    public TextMeshProUGUI text;
    public GameObject checkMark;
    public GameObject locked;

    public void Unavailable(int level) {
        button.interactable = false;
        locked.SetActive(true);
        checkMark.SetActive(false);
        text.text = level.ToString();
    }

    public void Finished(int level) {
        button.interactable = true;
        locked.SetActive(false);
        checkMark.SetActive(true);
        text.text = level.ToString();
    }

    public void NextLevel(int level) {
        button.interactable = true;
        locked.SetActive(false);
        checkMark.SetActive(false);
        text.text = level.ToString();
    }
}
