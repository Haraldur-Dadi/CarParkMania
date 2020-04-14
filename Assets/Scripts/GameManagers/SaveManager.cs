using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour {

    public static SaveManager Instance;
    public AchivementManager achivementManager;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if(scene.buildIndex == 0) {
            achivementManager = AchivementManager.Instance;
            achivementManager.UpdateAchivements();
        }
    }


    public void SaveIntData(string varName, int value) {
        PlayerPrefs.SetInt(varName, value);
        //PlayerPrefs.Save();
    }

    public void SaveFloatData(string varName, float value) {
        PlayerPrefs.SetFloat(varName, value);
        //PlayerPrefs.Save();
    }

    public void SaveStringData(string varName, string value) {
        PlayerPrefs.SetString(varName, value);
        //PlayerPrefs.Save();
    }

    public void IncreaseAchivementProgress(int achivementID) {
        float currAmount = PlayerPrefs.GetFloat("Achivement" + achivementID, 0);
        SaveFloatData("Achivement" + achivementID, currAmount + 1);

        if (achivementManager)
            achivementManager.UpdateAchivements();
    }
}
