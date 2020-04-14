using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class About : MonoBehaviour {

    public int tutImgIndex;
    public GameObject[] tutorialImages;

    public GameObject prevButton;
    public GameObject nextButton;

    public TextMeshProUGUI indexTxt;

    public void ResetTutImage() {
        tutImgIndex = 0;

        for (int i = 0; i < tutorialImages.Length; i++) {
            tutorialImages[i].SetActive(false);
        }

        tutorialImages[tutImgIndex].SetActive(true);
        ToggleButtons();
    }

    public void ShowNextTutImage() {
        AudioManager.Instance.PlayButtonClick();
        tutorialImages[tutImgIndex].SetActive(false);

        tutImgIndex += 1;
        tutorialImages[tutImgIndex].SetActive(true);

        ToggleButtons();
    }

    public void ShowPrevTutImage() {
        AudioManager.Instance.PlayButtonClick();
        tutorialImages[tutImgIndex].SetActive(false);

        tutImgIndex -= 1;
        tutorialImages[tutImgIndex].SetActive(true);

        ToggleButtons();
    }

    public void ToggleButtons() {
        indexTxt.text = (tutImgIndex + 1) + "/" + tutorialImages.Length;

        if (tutImgIndex == 0) {
            prevButton.SetActive(false);
            nextButton.SetActive(true);
        } else if (tutImgIndex == tutorialImages.Length - 1) {
            prevButton.SetActive(true);
            nextButton.SetActive(false);
        } else {
            prevButton.SetActive(true);
            nextButton.SetActive(true);
        }
    }
}
