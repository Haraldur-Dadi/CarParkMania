using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchivementPanel : MonoBehaviour {

    public int achivementID;

    public string whatToDo;
    public TextMeshProUGUI wtd;

    public float currVal;
    public float totalNeeded;
    public Image progressBar;
    public TextMeshProUGUI progressTxt;

    void Start() {
        string achivementName = "Achivement" + achivementID;
        currVal = PlayerPrefs.GetFloat(achivementName, 0f);

        wtd.text = whatToDo;
        progressBar.fillAmount = currVal / totalNeeded;

        if (progressBar.fillAmount == 1f) {
            progressBar.color = new Color32(42, 255, 0, 255);
            progressTxt.text = "Completed";
        } else {
            progressTxt.text = currVal + "/" + totalNeeded;        
        }
    }
}
