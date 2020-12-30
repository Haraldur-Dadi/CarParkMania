using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CrossSceneManager : MonoBehaviour {
    public static CrossSceneManager Instance;

    public CanvasGroup[] canvasGroups;
    public GameObject settingsUI;
    public Button settingsBtn;

    public string panelName;
    public int gameModeNr;
    public int difficulty;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            panelName = "";
            gameModeNr = -1;
            difficulty = 0;
            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        canvasGroups = GameObject.FindObjectsOfType<CanvasGroup>();
        settingsBtn = GameObject.Find("SettingsBtn").GetComponent<Button>();
        settingsBtn.onClick.AddListener(delegate { ToggleSettings(); });
        settingsUI.SetActive(false);
        TmpPreventClicks();
    }

    public void ToggleSettings() { settingsUI.SetActive(!settingsUI.activeSelf); }
    public void TmpPreventClicks() { StartCoroutine(PreventClicks()); }
    IEnumerator PreventClicks() {
        foreach (CanvasGroup group in canvasGroups) {
            group.interactable = false;
        }
        yield return new WaitForSeconds(0.4f);
        foreach (CanvasGroup group in canvasGroups) {
            group.interactable = true;
        }
    }
}
