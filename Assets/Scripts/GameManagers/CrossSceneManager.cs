using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CrossSceneManager : MonoBehaviour {
    public static CrossSceneManager Instance;

    public CanvasGroup[] canvasGroups;
    public GameObject settingsUI;
    public Button settingsBtn;

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
        canvasGroups = GameObject.FindObjectsOfType<CanvasGroup>();
        settingsBtn = GameObject.Find("SettingsBtn").GetComponent<Button>();
        settingsBtn.onClick.AddListener(delegate { ToggleSettings(); });
        settingsUI.SetActive(false);
        TmpPreventClicks();
        StartCoroutine(FadeIn());
    }

    public void ToggleSettings() { settingsUI.SetActive(!settingsUI.activeSelf); }
    public void TmpPreventClicks() { StartCoroutine(PreventClicks()); }
    IEnumerator PreventClicks() {
        foreach (CanvasGroup group in canvasGroups) {
            if (group) {
                group.interactable = false;
            }
        }
        yield return new WaitForSeconds(0.4f);
        foreach (CanvasGroup group in canvasGroups) {
            group.interactable = true;
        }
    }

    IEnumerator FadeIn() {
        float t = 1f;

        while (t > 0f) {
            t -= Time.deltaTime * 2;
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return null;
        }
    }

    public void FadeBetweenObjects() {
        CrossSceneManager.Instance.TmpPreventClicks();
        StartCoroutine(FadeBetweenObjInScene());
    }
    IEnumerator FadeBetweenObjInScene() {
        float t = 0f;
        while (t < 1f) {
            t += Time.deltaTime * 3;
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return null;
        }

        t = 1f;
        while (t > 0f) {
            t -= Time.deltaTime * 3;
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return null;
        }
    }
    public void FadeToBuildIndex(int buildIndex) {
        TmpPreventClicks();
        StartCoroutine(FadeOutBuildindex(buildIndex));
    }
    IEnumerator FadeOutBuildindex(int buildindex) {
        float t = 0f;
        while (t < 1f) {
            t += Time.deltaTime * 2;
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return null;
        }

        SceneManager.LoadScene(buildindex);
    }
}
