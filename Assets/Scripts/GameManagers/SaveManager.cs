﻿using UnityEngine;

public class SaveManager : MonoBehaviour {

    public static SaveManager Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    public void SaveIntData(string varName, int value) {
        PlayerPrefs.SetInt(varName, value);
        //PlayerPrefs.Save();
    }

    public void SaveFloatData(string varName, float value) {
        PlayerPrefs.SetFloat(varName, value);
        //PlayerPrefs.Save();
    }

    public void SaveStringData(string varName, string value) {
        PlayerPrefs.SetString(varName, value);
        //PlayerPrefs.Save();
    }
}
