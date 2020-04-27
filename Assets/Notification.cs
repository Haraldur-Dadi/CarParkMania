using UnityEngine;

public class Notification : MonoBehaviour {
    public DailySpin dailySpin;
    public DailyChallenges dailyChallenges;

    private void OnEnable() {
        if (dailySpin) {
            dailySpin.canSpin();
        } else {
            dailyChallenges.SetNotification();
        }
    }
}
