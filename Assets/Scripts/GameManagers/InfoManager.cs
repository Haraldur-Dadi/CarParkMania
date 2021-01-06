using UnityEngine;
using TMPro;

public class InfoManager : MonoBehaviour {
    int standardTotalLevels = 100;

    public TextMeshProUGUI totalCompleted;
    public TextMeshProUGUI[] casualCompleted;
    public TextMeshProUGUI[] challengeCompleted;
    public TextMeshProUGUI[] _8x8Completed;

    void Start() {
        int casualReached = PlayerPrefs.GetInt("CasualLevelReached", 0);
        int challengeReached = PlayerPrefs.GetInt("ChallengeLevelReached", 0);
        int _8x8Reached = PlayerPrefs.GetInt("8x8LevelReached", 0);

        totalCompleted.text = "Total: " + (casualReached + challengeReached + _8x8Reached) + "/" + (standardTotalLevels*2 + 40);
        casualCompleted[0].text = ((float) casualReached / standardTotalLevels) * 100 + "%";
        casualCompleted[1].text = casualReached + "/" + standardTotalLevels;
        challengeCompleted[0].text = ((float) challengeReached / standardTotalLevels) * 100 + "%";
        challengeCompleted[1].text = challengeReached + "/" + standardTotalLevels;
        _8x8Completed[0].text = ((float) _8x8Reached / 40) * 100 + "%";
        _8x8Completed[1].text = _8x8Reached + "/" + 40;
    }
}
