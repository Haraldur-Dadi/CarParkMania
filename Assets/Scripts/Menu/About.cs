using UnityEngine;
using TMPro;

public class About : MonoBehaviour {
    public int tutImgIndex = 0;
    public GameObject[] tutorialImages;

    public GameObject prevButton;
    public GameObject nextButton;
    public TextMeshProUGUI indexTxt;

    void Start() {
        for (int i = 0; i < tutorialImages.Length; i++) {
            tutorialImages[i].SetActive(false);
        }
        tutorialImages[tutImgIndex].SetActive(true);
        ToggleButtons();
    }

    public void ShowNextTutImage() {
        tutorialImages[tutImgIndex].SetActive(false);
        tutImgIndex++;
        tutorialImages[tutImgIndex].SetActive(true);
        ToggleButtons();
    }

    public void ShowPrevTutImage() {
        tutorialImages[tutImgIndex].SetActive(false);
        tutImgIndex--;
        tutorialImages[tutImgIndex].SetActive(true);
        ToggleButtons();
    }

    public void ToggleButtons() {
        indexTxt.text = (tutImgIndex + 1) + "/" + tutorialImages.Length;
        prevButton.SetActive(tutImgIndex != 0);
        nextButton.SetActive(tutImgIndex != tutorialImages.Length - 1);
    }
}
