using UnityEngine;
using TMPro;

public class InfoManager : MonoBehaviour {
    int standardTotalLevels = 100;

    public TextMeshProUGUI totalCompleted;
    public TextMeshProUGUI[] casualCompleted;
    public TextMeshProUGUI[] challengeCompleted;
    public TextMeshProUGUI[] _8x8Completed;
    public TextMeshProUGUI[] _8x8ChallengeCompleted;

    void Start() {
        int casualReached = PlayerPrefs.GetInt("CasualLevelReached", 0);
        int challengeReached = PlayerPrefs.GetInt("ChallengeLevelReached", 0);
        int _8x8Reached = PlayerPrefs.GetInt("8x8LevelReached", 0);
        int _8x8ChallengeReached = PlayerPrefs.GetInt("8x8ChallengeLevelReached", 0);

        totalCompleted.text = "Total: " + (casualReached + challengeReached + _8x8Reached) + "/" + (standardTotalLevels*2 + 40 + 40);
        casualCompleted[0].text = ((float) casualReached / standardTotalLevels) * 100 + "%";
        casualCompleted[1].text = casualReached + "/" + standardTotalLevels;
        challengeCompleted[0].text = ((float) challengeReached / standardTotalLevels) * 100 + "%";
        challengeCompleted[1].text = challengeReached + "/" + standardTotalLevels;
        _8x8Completed[0].text = ((float) _8x8Reached / 40) * 100 + "%";
        _8x8Completed[1].text = _8x8Reached + "/" + 40;
        _8x8ChallengeCompleted[0].text = ((float) _8x8ChallengeReached / 40) * 100 + "%";
        _8x8ChallengeCompleted[1].text = _8x8ChallengeReached + "/" + 40;
    }
}
