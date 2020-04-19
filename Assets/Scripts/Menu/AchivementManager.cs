using UnityEngine;
using TMPro;

public class AchivementManager : MonoBehaviour {

    public static AchivementManager Instance;

    public SaveManager saveManager;
    public GoldManager goldManager;

    public AchivementPanel[] achivements;
    public TextMeshProUGUI totalCompletedTxt;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
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
        saveManager.SaveIntData("Achivement" + achivementID + "Collected", 1);
        goldManager.AddGold(25, false);
    }
}
