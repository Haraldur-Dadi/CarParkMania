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
        if (Instance != null) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        StartCoroutine(FadeIn());
    }

    public void FadeBetweenObjects() {
        StartCoroutine(FadeBetweenObjInScene());
    }

    public void FadeToBuildIndex(int buildIndex) {
        StartCoroutine(FadeOutBuildindex(buildIndex));
    }

    public IEnumerator FadeIn() {
        float t = 1f;

        while (t > 0f) {
            t -= Time.deltaTime * 2;
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }
    }

    public IEnumerator FadeBetweenObjInScene() {
        float t = 0f;
        while (t < 1f) {
            t += Time.deltaTime * 2;
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }

        t = 1f;
        while (t > 0f) {
            t -= Time.deltaTime * 2;
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }
    }

    public IEnumerator FadeOutBuildindex(int buildindex) {
        float t = 0f;
        while (t < 1f) {
            t += Time.deltaTime * 2;
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }

        SceneManager.LoadScene(buildindex);
    }
}
