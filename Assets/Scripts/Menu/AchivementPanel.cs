using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchivementPanel : MonoBehaviour {

    public int achivementID;
    public bool completed;
    public bool canCollect;

    public string whatToDo;
    public TextMeshProUGUI wtd;

    public float currVal;
    public float totalNeeded;
    public Image progressBar;
    public TextMeshProUGUI progressTxt;

    public void UpdateUI() {
        currVal = Mathf.Clamp(PlayerPrefs.GetFloat("Achivement" + achivementID, 0f), 0, totalNeeded);

        wtd.text = whatToDo;
        progressBar.fillAmount = currVal / totalNeeded;

        if (progressBar.fillAmount == 1f) {
            if (PlayerPrefs.GetInt("Achivement" + achivementID + "Collected", 0) == 1) {
                canCollect = false;
            } else {
                canCollect = true;
            }

            completed = true;
            progressBar.color = new Color32(42, 255, 0, 255);
            progressTxt.text = "";
        } else {
            completed = false;
            progressTxt.text = currVal + "/" + totalNeeded;        
        }
    }
}