using UnityEngine;

public class Achivements : MonoBehaviour {

    public GameObject achivementsPanel;

    private void Start() {
        achivementsPanel.SetActive(false);
    }

    public void ToggleAchivementsPanel() {
        achivementsPanel.SetActive(!achivementsPanel.activeSelf);
    }
}
