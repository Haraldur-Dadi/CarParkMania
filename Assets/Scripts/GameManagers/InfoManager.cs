using UnityEngine;
using TMPro;

public class InfoManager : MonoBehaviour {
    int standardTotalLevels = 100;

    public TextMeshProUGUI totalCompleted;
    public TextMeshProUGUI[] casualCompleted;
    public TextMeshProUGUI[] challengeCompleted;

    void Start() {
        int casualReached = PlayerPrefs.GetInt("CasualLevelReached", 0);
        int challengeReached = PlayerPrefs.GetInt("ChallengeLevelReached", 0);

        totalCompleted.text = "Total: " + (casualReached + challengeReached) + "/" + standardTotalLevels*2;
        casualCompleted[0].text = ((float) casualReached / standardTotalLevels) * 100 + "%";
        casualCompleted[1].text = casualReached + "/" + standardTotalLevels;
        challengeCompleted[0].text = ((float) challengeReached / standardTotalLevels) * 100 + "%";
        challengeCompleted[1].text = challengeReached + "/" + standardTotalLevels;
    }
}
