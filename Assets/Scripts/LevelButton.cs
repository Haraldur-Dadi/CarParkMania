using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour {

    public Button button;
    public TextMeshProUGUI text;
    public GameObject checkMark;
    public GameObject locked;
    public Image star;

    public void Unavailable(int level) {
        button.interactable = false;
        locked.SetActive(true);
        if (checkMark)
            checkMark.SetActive(false);
        text.text = (level + 1).ToString();
    }

    public void Finished(int level) {
        button.interactable = true;
        locked.SetActive(false);
        if (checkMark)
            checkMark.SetActive(true);
        text.text = (level + 1).ToString();
    }

    public void NextLevel(int level) {
        button.interactable = true;
        locked.SetActive(false);
        if (checkMark)
            checkMark.SetActive(false);
        text.text = (level + 1).ToString();
    }
}
