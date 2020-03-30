﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoManager : MonoBehaviour {

    private int standardTotalLevels = 100;

    // GameObjects
    public TextMeshProUGUI totalCompleted;
    public TextMeshProUGUI[] casualCompleted;
    public TextMeshProUGUI[] challengeCompleted;
    public TextMeshProUGUI[] timedCompleted;

    public Scrollbar casualViewScrollbar;

    private void Start() {
        totalCompleted.text = "Total: " + PlayerPrefs.GetInt("TotalLevelsComplete", 0) + "/" + standardTotalLevels*3;
        casualCompleted[0].text = ((float)PlayerPrefs.GetInt("CasualLevelReached", 0) / standardTotalLevels) * 100 + "%";
        casualCompleted[1].text = PlayerPrefs.GetInt("CasualLevelReached", 0) + "/" + standardTotalLevels;
        challengeCompleted[0].text = ((float)PlayerPrefs.GetInt("ChallengeLevelReached", 0) / standardTotalLevels) * 100 + "%";
        challengeCompleted[1].text = PlayerPrefs.GetInt("ChallengeLevelReached", 0) + "/" + standardTotalLevels;
        timedCompleted[0].text = ((float)PlayerPrefs.GetInt("TimedLevelReached", 0) / standardTotalLevels) * 100 + "%";
        timedCompleted[1].text = PlayerPrefs.GetInt("TimedLevelReached", 0) + "/" + standardTotalLevels;
    }

    public void DifficultyToggle(float value) {
        casualViewScrollbar.value = value;
    }
}
