using UnityEngine;

public class SaveManager : MonoBehaviour {

    public static SaveManager Instance;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }

    public void SaveData(string varName, int value) {
        PlayerPrefs.SetInt(varName, value);
        //PlayerPrefs.Save();
    }
}
