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
            saveManager.SaveStringData("LastChallengeCompletedDate", "1582-09-15");
        
        NewDayReset();

        if (!HasCompleted()) {
            notification.SetTrigger("Avail");
            claimScreen.SetActive(false);
        } else if (PlayerPrefs.GetInt("HasClaimed", 0) == 1) {
            claimBtn.SetActive(false);
        }
    }

    private void NewDayReset() {
        DateTime currDate = DateTime.Today;
        DateTime lastSpinDate = DateTime.ParseExact(PlayerPrefs.GetString("LastChallengeCompletedDate", "1582-09-15"), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        
        if (currDate > lastSpinDate) {
            saveManager.SaveIntData("HasClaimed", 0);
            saveManager.SaveIntData("DailyChallenge1Completed", 0);
            saveManager.SaveIntData("DailyChallenge2Completed", 0);
            saveManager.SaveIntData("DailyChallenge3Completed", 0);
            saveManager.SaveIntData("DailyChallenge4Completed", 0);
            saveManager.SaveStringData("LastChallengeCompletedDate", currDate.Year + "-" + currDate.Month.ToString().PadLeft(2, '0') + "-" + currDate.Day.ToString().PadLeft(2, '0'));
        }
    }

    private bool HasCompleted() {
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

        return completed;
    }

    public void LoadChallengeLevel(int difficulty) {
        saveManager.SaveIntData("boardToLoad", (7 * (difficulty - 1)) + weekday);
        sceneFader.FadeToBuildIndex(2);
    }

    public void ClaimReward() {
        goldManager.AddGold(25, false);
        saveManager.SaveIntData("HasClaimed", 1);
        saveManager.IncreaseAchivementProgress(7);
        claimBtn.SetActive(false);
    }
}