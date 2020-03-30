using UnityEngine;
using UnityEngine.UI;

public class CrossSceneManager : MonoBehaviour {

    public static CrossSceneManager instance;

    public GameObject settingsUI;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (instance != this) {
            Destroy(gameObject);
        }

        settingsUI = GameObject.Find("SettingsUI");

        Button settingsBtn = GameObject.Find("SettingsBtn").GetComponent<Button>();
        settingsBtn.onClick.AddListener(delegate { ToggleSettings(); });

    }

    private void Start() {
        settingsUI.SetActive(false);
    }

    public void ToggleSettings() {
        settingsUI.SetActive(!settingsUI.activeSelf);
    }
}
