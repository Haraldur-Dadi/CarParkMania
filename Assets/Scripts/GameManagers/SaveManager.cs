using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour {

    public static SaveManager Instance;
    public AchivementManager achivementManager;

    public GameObject resetProgressWindow;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.buildIndex == 0) {
            resetProgressWindow.SetActive(false);
            achivementManager = AchivementManager.Instance;
            achivementManager.UpdateAchivements();
        }
    }

    public void SaveIntData(string varName, int value) {
        PlayerPrefs.SetInt(varName, value);
    }

    public void SaveFloatData(string varName, float value) {
        PlayerPrefs.SetFloat(varName, value);
    }

    public void SaveStringData(string varName, string value) {
        PlayerPrefs.SetString(varName, value);
    }

    public void IncreaseAchivementProgress(int achivementID) {
        float currAmount = PlayerPrefs.GetFloat("Achivement" + achivementID, 0);
        SaveFloatData("Achivement" + achivementID, currAmount + 1);

        if (achivementManager)
            achivementManager.UpdateAchivements();
    }

    public void ResetProgressPrompt() {
        resetProgressWindow.SetActive(!resetProgressWindow.activeSelf);
    }

    public void ResetProgress() {
        PlayerPrefs.DeleteAll();
        SceneFader.Instance.FadeToBuildIndex(0);
    }
}
