﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour {

    public Image img;
    public AnimationCurve curve;

    public static SceneFader Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        StartCoroutine(FadeIn());
    }

    public void FadeBetweenObjects() {
        CrossSceneManager.Instance.TmpPreventClicks();
        StartCoroutine(FadeBetweenObjInScene());
    }

    public void FadeToBuildIndex(int buildIndex) {
        StartCoroutine(FadeOutBuildindex(buildIndex));
    }

    public IEnumerator FadeIn() {
        float t = 1f;

        while (t > 0f) {
            t -= Time.deltaTime * 5;
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return null;
        }
    }

    public IEnumerator FadeBetweenObjInScene() {
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

    public IEnumerator FadeOutBuildindex(int buildindex) {
        float t = 0f;
        while (t < 1f) {
            t += Time.deltaTime * 5;
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return null;
        }

        SceneManager.LoadScene(buildindex);
    }
}
