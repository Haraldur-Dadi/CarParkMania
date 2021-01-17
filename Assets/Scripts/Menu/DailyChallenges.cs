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
        NewDayReset();
        multiplier = PlayerPrefs.GetInt("DailyMultiplier", 0);

        notification.SetActive(!HasCompleted());
        claimScreen.SetActive(!notification.activeSelf);
        claimBtn.SetActive(PlayerPrefs.GetInt("HasClaimed") != 1);
    }

    void NewDayReset() {
        if (DateTime.Today > DateTime.Parse(PlayerPrefs.GetString("LastChallengeCompletedDate", "1582-09-15"))) {
            PlayerPrefs.SetInt("HasClaimed", 0);
            PlayerPrefs.SetInt("DailyMultiplier", UnityEngine.Random.Range(0, 2));
            PlayerPrefs.SetInt("DailyChallenge1Completed", 0);
            PlayerPrefs.SetInt("DailyChallenge2Completed", 0);
            PlayerPrefs.SetInt("DailyChallenge3Completed", 0);
            PlayerPrefs.SetInt("DailyChallenge4Completed", 0);
            PlayerPrefs.SetString("LastChallengeCompletedDate", DateTime.Today.ToString("yyyy-MM-dd"));
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
        CrossSceneManager.Instance.gameModeNr = 0;
        CrossSceneManager.Instance.FadeToBuildIndex(1);
    }

    public void ClaimReward() {
        GoldManager.Instance.AddGold(25, false);
        PlayerPrefs.SetInt("HasClaimed", 1);
        AchivementManager.Instance.IncreaseAchivementProgress(7);
        claimBtn.SetActive(false);
    }
}