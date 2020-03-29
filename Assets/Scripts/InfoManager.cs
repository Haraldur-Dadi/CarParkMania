using UnityEngine;
using TMPro;

public class InfoManager : MonoBehaviour {

    private int standardTotalLevels = 100;

    // GameObjects
    public TextMeshProUGUI casualMainTxt;

    private void Start() {
        casualMainTxt.text = ((float)PlayerPrefs.GetInt("LevelReached", 0) / standardTotalLevels)*100 + "%\n" + PlayerPrefs.GetInt("LevelReached", 0) + "/" + standardTotalLevels;
    }
}
