using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CrossSceneManager : MonoBehaviour {

    public static CrossSceneManager Instance;

    public GameObject settingsUI;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (Instance != this) {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        settingsUI = GameObject.Find("SettingsUI");

        Button settingsBtn = GameObject.Find("SettingsBtn").GetComponent<Button>();
        settingsBtn.onClick.AddListener(delegate { ToggleSettings(); });
    }

    public void ToggleSettings() {
        settingsUI.SetActive(!settingsUI.activeSelf);
    }
}
