using System;
using UnityEngine;
using UnityEngine.UI;

public class DailyChallenges : MonoBehaviour {
    int multiplier;
    public Button challenge1Btn;
    public Button challenge2Btn;
    public Button challenge3Btn;
    public Button challenge4Btn;

    public GameObject claimScreen;
    public GameObject claimBtn;
    public GameObject notification;

    void Start() {
        multiplier = PlayerPrefs.GetInt("DailyMultiplier", 0);

        if (!PlayerPrefs.HasKey("LastChallengeCompletedDate")) { PlayerPrefs.SetString("LastChallengeCompletedDate", "1582-09-15"); }
        NewDayReset();

        notification.SetActive(!HasCompleted());
        claimScreen.SetActive(!notification.activeSelf);
        claimBtn.SetActive(PlayerPrefs.GetInt("HasClaimed", 0) == 1);
    }

    void NewDayReset() {
        DateTime currDate = DateTime.Today;
        DateTime lastCompleteDate = DateTime.ParseExact(PlayerPrefs.GetString("LastChallengeCompletedDate", "1582-09-15"), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        
        if (currDate > lastCompleteDate) {
            PlayerPrefs.SetInt("HasClaimed", 0);
            multiplier = UnityEngine.Random.Range(0, 2);
            PlayerPrefs.SetInt("DailyMultiplier", multiplier);
            PlayerPrefs.SetInt("DailyChallenge1Completed", 0);
            PlayerPrefs.SetInt("DailyChallenge2Completed", 0);
            PlayerPrefs.SetInt("DailyChallenge3Completed", 0);
            PlayerPrefs.SetInt("DailyChallenge4Completed", 0);
            PlayerPrefs.SetString("LastChallengeCompletedDate", currDate.Year + "-" + currDate.Month.ToString().PadLeft(2, '0') + "-" + currDate.Day.ToString().PadLeft(2, '0'));
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
        PlayerPrefs.SetInt("boardToLoad", (int)DateTime.Now.DayOfWeek + 7 * (multiplier + difficulty));
        CrossSceneManager.Instance.FadeToBuildIndex(1);
    }

    public void ClaimReward() {
        GoldManager.Instance.AddGold(25, false);
        PlayerPrefs.SetInt("HasClaimed", 1);
        AchivementManager.Instance.IncreaseAchivementProgress(7);
        claimBtn.SetActive(false);
    }
}