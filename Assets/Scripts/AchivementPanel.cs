﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchivementPanel : MonoBehaviour {

    public int achivementID;

    public string whatToDo;
    public TextMeshProUGUI wtd;

    public float currVal;
    public float totalNeeded;
    public Slider progressBar;
    public TextMeshProUGUI progressTxt;

    public GameObject completedImg;

    void Start() {
        string achivementName = "Achivement" + achivementID;
        currVal = PlayerPrefs.GetFloat(achivementName, 0f);

        wtd.text = whatToDo;
        progressBar.value = currVal / totalNeeded;

        if (progressBar.value == 1f) {
            progressTxt.text = "Completed";
            completedImg.SetActive(true);
        } else {
            progressTxt.text = currVal + "/" + totalNeeded;        
            completedImg.SetActive(false);
        }
    }
}
