using UnityEngine;
using TMPro;

public class AchivementManager : MonoBehaviour {

    public SaveManager saveManager;
    public GoldManager goldManager;

    public AchivementPanel[] achivements;
    public TextMeshProUGUI totalCompletedTxt;

    public int achivemntLeftToCollect;
    public Animator notification;

    private void Start() {
        int completed = 0;
        foreach (AchivementPanel achivement in achivements) {
            achivement.UpdateUI();

            if (achivement.completed)
                completed += 1;

            if (achivement.canCollect) {
                achivemntLeftToCollect += 1;
            }
        }

        if (achivemntLeftToCollect > 0)
            notification.SetTrigger("Avail");

        totalCompletedTxt.text = "Completed: " + completed + "/10";
    }

    public void CollectAchivementReward(int achivementID) {
        saveManager.SaveIntData("Achivement" + achivementID + "Collected", 1);
        goldManager.AddGold(25, false);
    }
}
