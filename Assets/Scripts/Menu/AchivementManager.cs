using UnityEngine;
using TMPro;

public class AchivementManager : MonoBehaviour {
    public static AchivementManager Instance;

    public AchivementPanel[] achivements;
    public TextMeshProUGUI totalCompletedTxt;
    public GameObject resetProgressWindow;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void SetStartState() {
        resetProgressWindow.SetActive(false);
        UpdateAchivements();
    }

    public void IncreaseAchivementProgress(int achivementID) {
        float currAmount = PlayerPrefs.GetFloat("Achivement" + achivementID, 0);
        PlayerPrefs.SetFloat("Achivement" + achivementID, currAmount + 1);

        if (AchivementManager.Instance)
            AchivementManager.Instance.UpdateAchivements();
    }

    public void UpdateAchivements() {
        int completed = 0;
        foreach (AchivementPanel achivement in achivements) {
            achivement.UpdateUI();

            if (achivement.completed)
                completed += 1;
        }

        totalCompletedTxt.text = "Completed: " + completed + "/10";
    }
    public void CollectAchivementReward(int achivementID) {
        PlayerPrefs.SetInt("Achivement" + achivementID + "Collected", 1);
        GoldManager.Instance.AddGold(25, false);
    }

    public void ResetProgressPrompt() { resetProgressWindow.SetActive(!resetProgressWindow.activeSelf); }
    public void ResetProgress() {
        PlayerPrefs.DeleteAll();
        CrossSceneManager.Instance.FadeToBuildIndex(0);
    }
}
