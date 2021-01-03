using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour {
    public Button button;
    public TextMeshProUGUI levelTxt;

    public GameObject locked;
    public Image display;

    public Sprite checkMark;
    public Sprite blankStar;
    public Sprite bronzeStar;
    public Sprite silverStar;
    public Sprite goldStar;

    public void SetStar(int stars) {
        if (stars == 0) {
            display.sprite = blankStar;
        } else if (stars == 1) {
            display.sprite = bronzeStar;
        } else if (stars == 2) {
            display.sprite = silverStar;
        } else {
            display.sprite = goldStar;
        }
        HideDisplay(false);
    }
    public void Unavailable() {
        button.interactable = false;
        locked.SetActive(true);
        HideDisplay(true);
    }
    public void Finished() {
        button.interactable = true;
        locked.SetActive(false);
        HideDisplay(false);
        display.sprite = checkMark;
    }
    public void NextLevel() {
        button.interactable = true;
        locked.SetActive(false);
        HideDisplay(true);
    }

    void HideDisplay(bool hide) {
        Color col = display.color;
        col.a = hide ? 0f : 1f;
        display.color = col;
    }
}
