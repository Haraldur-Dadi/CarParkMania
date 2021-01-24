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
        StartCoroutine(FadeIn());
    }
    public void ToggleSettings() { 
        AudioManager.Instance.PlayButtonClick();
        settingsUI.SetActive(!settingsUI.activeSelf);
    }

    public void FadeBetweenObjects(float multiplier) { StartCoroutine(FadeBetweenObjInScene(multiplier)); }
    IEnumerator FadeBetweenObjInScene(float multiplier) {
        yield return FadeOut(multiplier);
        yield return FadeIn(multiplier);
    }
    public void FadeToBuildIndex(int buildIndex) { StartCoroutine(FadeOutBuildindex(buildIndex)); }
    IEnumerator FadeOutBuildindex(int buildindex) {
        yield return FadeOut();
        SceneManager.LoadScene(buildindex);
    }
    public IEnumerator FadeIn(float multiplier = 2f) {
        StartCoroutine(PreventClicks(multiplier));
        float t = 1f;
        while (t > 0f) {
            t -= Time.deltaTime * multiplier;
            img.color = new Color(0f, 0f, 0f, curve.Evaluate(t));
            yield return null;
        }
    }
    public IEnumerator FadeOut(float multiplier = 2f) {
        StartCoroutine(PreventClicks(multiplier));
        float t = 0f;
        while (t < 1f) {
            t += Time.deltaTime * multiplier;
            img.color = new Color(0f, 0f, 0f, curve.Evaluate(t));
            yield return null;
        }
    }
    IEnumerator PreventClicks(float multiplier) {
        foreach (CanvasGroup group in canvasGroups) {
            if (group) { group.interactable = false; }
        }
        yield return new WaitForSeconds(1f/multiplier);
        foreach (CanvasGroup group in canvasGroups) {
            if (group) { group.interactable = true; }
        }
    }
}
