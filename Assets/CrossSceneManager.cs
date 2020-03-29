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

        Button settingsBtn = GameObject.Find("settingsBtn").GetComponent<Button>();
        settingsBtn.onClick.AddListener(delegate { ToggleSettings(); });

        settingsUI.SetActive(false);
    }

    public void ToggleSettings() {
        settingsUI.SetActive(!settingsUI.activeSelf);
    }
}
