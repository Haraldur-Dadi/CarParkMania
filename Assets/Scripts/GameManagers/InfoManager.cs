using UnityEngine;
using TMPro;

public class InfoManager : MonoBehaviour {

    private int standardTotalLevels = 100;

    // Level Selector
    public TextMeshProUGUI totalCompleted;
    public TextMeshProUGUI[] casualCompleted;
    public TextMeshProUGUI[] challengeCompleted;
    public TextMeshProUGUI[] timedCompleted;

    private void Start() {
        int casualReached = PlayerPrefs.GetInt("CasualLevelReached", 0);
        int challengeReached = PlayerPrefs.GetInt("ChallengeLevelReached", 0);
        int timedReached = PlayerPrefs.GetInt("TimedLevelReached", 0);

        totalCompleted.text = "Total: " + (casualReached + challengeReached + timedReached) + "/" + standardTotalLevels*3;
        casualCompleted[0].text = ((float) casualReached / standardTotalLevels) * 100 + "%";
        casualCompleted[1].text = casualReached + "/" + standardTotalLevels;
        challengeCompleted[0].text = ((float) challengeReached / standardTotalLevels) * 100 + "%";
        challengeCompleted[1].text = challengeReached + "/" + standardTotalLevels;
        timedCompleted[0].text = ((float) timedReached / standardTotalLevels) * 100 + "%";
        timedCompleted[1].text = timedReached + "/" + standardTotalLevels;
    }
}
