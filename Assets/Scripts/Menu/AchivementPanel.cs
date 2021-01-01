using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchivementPanel : MonoBehaviour {
    public int achivementID;
    public string whatToDo;
    public TextMeshProUGUI wtd;

    public float totalNeeded;
    public Image progressBar;
    public TextMeshProUGUI progressTxt;

    public void UpdateUI() {
        float currVal = Mathf.Clamp(PlayerPrefs.GetFloat("Achivement" + achivementID, 0f), 0, totalNeeded);
        progressBar.fillAmount = currVal / totalNeeded;
        wtd.text = whatToDo;

        if (progressBar.fillAmount == 1f) {
            progressBar.color = new Color32(42, 255, 0, 255);
        }
        progressTxt.text = (progressBar.fillAmount == 1f) ? "" : currVal + "/" + totalNeeded;
    }
}