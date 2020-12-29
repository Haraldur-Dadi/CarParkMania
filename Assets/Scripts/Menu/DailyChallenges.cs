using System;
using UnityEngine;
using UnityEngine.UI;

public class DailyChallenges : MonoBehaviour {
    public SaveManager saveManager;

    int multiplier;
    public Button challenge1Btn;
    public Button challenge2Btn;
    public Button challenge3Btn;
    public Button challenge4Btn;

    public GameObject claimScreen;
    public GameObject claimBtn;
    public GameObject notification;

    void Start() {
        saveManager = SaveManager.Instance;
        multiplier = PlayerPrefs.GetInt("DailyMultiplier", 0);

        if (!PlayerPrefs.HasKey("LastChallengeCompletedDate")) { saveManager.SaveStringData("LastChallengeCompletedDate", "1582-09-15"); }
        NewDayReset();

        notification.SetActive(!HasCompleted());
        claimScreen.SetActive(!notification.activeSelf);
        claimBtn.SetActive(PlayerPrefs.GetInt("HasClaimed", 0) == 1);
    }

    void NewDayReset() {
        DateTime currDate = DateTime.Today;
        DateTime lastCompleteDate = DateTime.ParseExact(PlayerPrefs.GetString("LastChallengeCompletedDate", "1582-09-15"), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        
        if (currDate > lastCompleteDate) {
            saveManager.SaveIntData("HasClaimed", 0);
            multiplier = UnityEngine.Random.Range(0, 2);
            saveManager.SaveIntData("DailyMultiplier", multiplier);
            saveManager.SaveIntData("DailyChallenge1Completed", 0);
            saveManager.SaveIntData("DailyChallenge2Completed", 0);
            saveManager.SaveIntData("DailyChallenge3Completed", 0);
            saveManager.SaveIntData("DailyChallenge4Completed", 0);
            saveManager.SaveStringData("LastChallengeCompletedDate", currDate.Year + "-" + currDate.Month.ToString().PadLeft(2, '0') + "-" + currDate.Day.ToString().PadLeft(2, '0'));
        }
    }

    bool HasCompleted() {
        // Check if player has finished all challenges
        challenge1Btn.interactable = PlayerPrefs.GetInt("DailyChallenge1Completed", 0) == 0;
        challenge2Btn.interactable = PlayerPrefs.GetInt("DailyChallenge2Completed", 0) == 0;
        challenge3Btn.interactable = PlayerPrefs.GetInt("DailyChallenge3Completed", 0) == 0;
        challenge4Btn.interactable = PlayerPrefs.GetInt("DailyChallenge4Completed", 0) == 0;
        return !(challenge1Btn.interactable || challenge2Btn.interactable || challenge3Btn.interactable || challenge4Btn.interactable);
    }

    public void LoadChallengeLevel(int difficulty) {
        saveManager.SaveIntData("boardToLoad", (int)DateTime.Now.DayOfWeek + 7 * (multiplier + difficulty));
        SceneFader.Instance.FadeToBuildIndex(1);
    }

    public void ClaimReward() {
        GoldManager.Instance.AddGold(25, false);
        saveManager.SaveIntData("HasClaimed", 1);
        saveManager.IncreaseAchivementProgress(7);
        claimBtn.SetActive(false);
    }
}