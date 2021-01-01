using UnityEngine;

public class AchivementManager : MonoBehaviour {
    public static AchivementManager Instance;
    public GameObject resetProgressWindow;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            resetProgressWindow.SetActive(false);
        } else {
            Destroy(gameObject);
        }
    }

    public void IncreaseAchivementProgress(int achivementID) {
        float currAmount = PlayerPrefs.GetFloat("Achivement" + achivementID, 0);
        PlayerPrefs.SetFloat("Achivement" + achivementID, currAmount + 1);
    }
    public void UpdateAchivementsUI() {
        AchivementPanel[] achivements = GameObject.FindObjectsOfType<AchivementPanel>();
        foreach (AchivementPanel achivement in achivements) {
            achivement.UpdateUI();
        }
    }

    public void ResetProgressPrompt() { resetProgressWindow.SetActive(!resetProgressWindow.activeSelf); }
    public void ResetProgress() {
        PlayerPrefs.DeleteAll();
        CrossSceneManager.Instance.FadeToBuildIndex(0);
    }
}
