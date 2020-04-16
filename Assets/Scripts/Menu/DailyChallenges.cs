using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DailyChallenges : MonoBehaviour {

    public SceneFader sceneFader;
    public SaveManager saveManager;
    public GoldManager goldManager;

    public int weekday;

    public Button challenge1Btn;
    public Button challenge2Btn;
    public Button challenge3Btn;
    public Button challenge4Btn;

    public GameObject claimScreen;
    public GameObject claimBtn;

    public Animator notification;

    private void Start() {
        sceneFader = SceneFader.Instance;
        saveManager = SaveManager.Instance;
        weekday = (int) DateTime.Now.DayOfWeek;

        if (!PlayerPrefs.HasKey("LastChallengeCompletedDate"))
            PlayerPrefs.SetString("LastChallengeCompletedDate", "1582-09-15");

        HasCompleted();

        if (CanComplete()) {
            claimScreen.SetActive(false);
        } else {
            if (PlayerPrefs.GetInt("HasClaimed", 0) == 1)
                claimBtn.SetActive(false);
        }
    }

    private bool CanComplete() {
        DateTime currDate = DateTime.Today;
        DateTime lastSpinDate = DateTime.ParseExact(PlayerPrefs.GetString("LastChallengeCompletedDate", "1582-09-15"), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        
        if (currDate > lastSpinDate) {
            saveManager.SaveIntData("HasClaimed", 0);
            notification.SetTrigger("Avail");
            return true;
        }
        return false;
    }

    private void HasCompleted() {
        // Check if player has finished all challenges
        bool completed = true;

        if (PlayerPrefs.GetInt("DailyChallenge1Completed", 0) == 1) {
            challenge1Btn.interactable = false;
        } else {
            completed = false;
        }

        if (PlayerPrefs.GetInt("DailyChallenge2Completed", 0) == 1) {
            challenge2Btn.interactable = false;
        } else {
            completed = false;
        }

        if (PlayerPrefs.GetInt("DailyChallenge3Completed", 0) == 1) {
            challenge3Btn.interactable = false;
        } else {
            completed = false;
        }

        if (PlayerPrefs.GetInt("DailyChallenge4Completed", 0) == 1) {
            challenge4Btn.interactable = false;
        } else {
            completed = false;
        }

        if (completed) {
            DateTime currDate = DateTime.Today;
            saveManager.SaveStringData("LastChallengeCompletedDate", currDate.Year + "-" + currDate.Month.ToString().PadLeft(2, '0') + "-" + currDate.Day.ToString().PadLeft(2, '0'));
        }
    }

    public void LoadChallengeLevel(int difficulty) {
        saveManager.SaveIntData("boardToLoad", (7 * (difficulty - 1)) + (weekday - 1));
        AudioManager.Instance.PlayButtonClick();
        sceneFader.FadeToBuildIndex(2);
    }

    public void ClaimReward() {
        goldManager.AddGold(25, false);
        saveManager.SaveIntData("HasClaimed", 1);
        claimBtn.SetActive(false);
    }
}
