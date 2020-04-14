using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CrossSceneManager : MonoBehaviour {

    public static CrossSceneManager Instance;

    public GameObject settingsUI;
    public Button settingsBtn;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        settingsBtn = GameObject.Find("SettingsBtn").GetComponent<Button>();
        settingsBtn.onClick.AddListener(delegate { ToggleSettings(); });

        settingsUI.SetActive(false);
    }

    public void ToggleSettings() {
        settingsUI.SetActive(!settingsUI.activeSelf);
        AudioManager.Instance.PlayButtonClick();
    }
}
