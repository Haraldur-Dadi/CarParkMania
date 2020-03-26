using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    public GameObject tutMenu;

    public int tutImgIndex;
    public GameObject[] tutorialImages;

    public Button prevButton;
    public Button nextButton;

    private void Start() {
        tutMenu.SetActive(false);
    }

    public void ToggleTutMenu() {
        bool toogle = !tutMenu.activeSelf;

        tutMenu.SetActive(toogle);
        tutImgIndex = 0;

        if (toogle) {
            ResetTutImage();
            ToggleButtons();
        }
    }

    public void ResetTutImage() {
        tutorialImages[tutImgIndex].SetActive(true);

        for (int i = 1; i < tutorialImages.Length; i++) {
            tutorialImages[i].SetActive(false);
        }
    }

    public void ShowNextTutImage() {
        tutorialImages[tutImgIndex].SetActive(false);

        tutImgIndex += 1;
        tutorialImages[tutImgIndex].SetActive(true);

        ToggleButtons();
    }

    public void ShowPrevTutImage() {
        tutorialImages[tutImgIndex].SetActive(false);

        tutImgIndex -= 1;
        tutorialImages[tutImgIndex].SetActive(true);

        ToggleButtons();
    }

    public void ToggleButtons() {
        if (tutImgIndex == 0) {
            prevButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(true);
        } else if (tutImgIndex == tutorialImages.Length - 1) {
            prevButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(false);
        } else {
            prevButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(true);
        }
    }
}
