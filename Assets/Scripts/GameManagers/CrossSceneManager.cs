using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CrossSceneManager : MonoBehaviour {
    public static CrossSceneManager Instance;

    public CanvasGroup[] canvasGroups;
    public GameObject settingsUI;
    public Image img;
    public AnimationCurve curve;

    public string panelName;
    public int gameModeNr;
    public int difficulty;

    void Awake() {
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
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        settingsUI.SetActive(false);
        if (scene.buildIndex == 0 && !PlayerPrefs.HasKey("LastOpened")) {
            img.color = new Color(0f, 0f, 0f, 1f);
            PlayerPrefs.SetString("LastOpened", "1582-09-15");
            PlayerPrefs.SetInt("boardToLoad", 0);
            gameModeNr = 1;
            SceneManager.LoadScene(1);
            return;
        }
        canvasGroups = GameObject.FindObjectsOfType<CanvasGroup>();
        GameObject.Find("SettingsBtn").GetComponent<Button>().onClick.AddListener(delegate { ToggleSettings(); });
        StartCoroutine(FadeIn(2));
    }
    public void ToggleSettings() { 
        AudioManager.Instance.PlayButtonClick();
        settingsUI.SetActive(!settingsUI.activeSelf);
    }

    public void FadeBetweenObjects() { StartCoroutine(FadeBetweenObjInScene()); }
    IEnumerator FadeBetweenObjInScene() {
        yield return FadeOut(3);
        yield return FadeIn(3);
    }
    public void FadeToBuildIndex(int buildIndex) { StartCoroutine(FadeOutBuildindex(buildIndex)); }
    IEnumerator FadeOutBuildindex(int buildindex) {
        yield return FadeOut(2);
        SceneManager.LoadScene(buildindex);
    }
    public IEnumerator FadeIn(int multiplier) {
        StartCoroutine(PreventClicks());
        float t = 1f;
        while (t > 0f) {
            t -= Time.deltaTime * multiplier;
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return null;
        }
    }
    public IEnumerator FadeOut(int multiplier) {
        StartCoroutine(PreventClicks());
        float t = 0f;
        while (t < 1f) {
            t += Time.deltaTime * multiplier;
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return null;
        }
    }
    IEnumerator PreventClicks() {
        foreach (CanvasGroup group in canvasGroups) {
            if (group) { group.interactable = false; }
        }
        yield return new WaitForSeconds(0.4f);
        foreach (CanvasGroup group in canvasGroups) {
            if (group) { group.interactable = true; }
        }
    }
}
