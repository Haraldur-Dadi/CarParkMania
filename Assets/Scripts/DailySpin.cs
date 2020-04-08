using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DailySpin : MonoBehaviour {
    
    SaveManager saveManager;
    GoldManager goldManager;

    public bool canSpin;

    public GameObject wheel;
    public GameObject unableToSpinPanel;
    public TextMeshProUGUI countdown;
    public GameObject winPanel;
    public GameObject winGoldPanel;
    public TextMeshProUGUI wonGoldAmountTxt;
    public GameObject winCarPanel;
    public Button spinBtn;

    private int randomValue;
    private float timeInterval;
    private int finalAngle;

    private int rewardAmount;

    void Start() {
        saveManager = SaveManager.Instance;
        goldManager = GoldManager.Instance;

        DateTime currDate = DateTime.Now;
        //PlayerPrefs.SetString("LastDateSpun", currDate.Year + "-" + currDate.Month.ToString().PadLeft(2, '0') + "-" + currDate.Day.ToString().PadLeft(2, '0'));
        PlayerPrefs.SetString("LastDateSpun", "1582-09-15");
        DateTime lastSpinDate = DateTime.ParseExact(PlayerPrefs.GetString("LastDateSpun", "1582-09-15"), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture).AddDays(1);

        if (currDate > lastSpinDate) {
            canSpin = true;
            spinBtn.gameObject.SetActive(true);
            spinBtn.interactable = true;
            unableToSpinPanel.SetActive(false);
        } else {
            canSpin = false;
            spinBtn.gameObject.SetActive(false);
            unableToSpinPanel.SetActive(true);
        }
        winPanel.SetActive(false);
    }

    private void Update() {
        if (!canSpin) {
            TimeSpan timeUntilMidnight = DateTime.Today.AddDays(1).Subtract(DateTime.Now);
            countdown.text = timeUntilMidnight.Hours.ToString().PadLeft(2, '0') + ":" + timeUntilMidnight.Minutes.ToString().PadLeft(2, '0') + ":" + timeUntilMidnight.Seconds.ToString().PadLeft(2, '0');
        }
    }

    public void SpinWheel() {
        spinBtn.interactable = false;
        DateTime currDate = DateTime.Now;
        saveManager.SaveStringData("LastDateSpun", currDate.Year + "-" + currDate.Month.ToString().PadLeft(2, '0') + "-" + currDate.Day.ToString().PadLeft(2, '0'));
        StartCoroutine(Spin());
    }

    IEnumerator Spin() {
        randomValue = UnityEngine.Random.Range(30, 40);
        timeInterval = 0.1f;

        for (int i = 0; i < randomValue; i++) {
            wheel.transform.Rotate(0, 0, 11.25f);
            if (i > Mathf.RoundToInt(randomValue * 0.65f)) {
                timeInterval = 0.2f;
            } else if (i > Mathf.RoundToInt(randomValue * 0.85f)) {
                timeInterval = 0.35f;
            }
            yield return new WaitForSeconds(timeInterval/2);
            wheel.transform.Rotate(0, 0, 11.25f);
            yield return new WaitForSeconds(timeInterval/2);
        }

        if (Mathf.RoundToInt(wheel.transform.eulerAngles.z) % 45 != 0) {
            wheel.transform.Rotate(0, 0, 22.5f);
        }

        finalAngle = Mathf.RoundToInt(wheel.transform.eulerAngles.z);
        wheel.transform.Rotate(0, 0, 22.5f);
        spinBtn.gameObject.SetActive(false);

        switch (finalAngle) {
            case 0:
                // Give Car!!!
                break;
            case 45:
                // Add 10 gold
                rewardAmount = 10;
                break;
            case 90:
                // Add 75 gold
                rewardAmount = 75;
                break;
            case 135:
                // Add 25 gold
                rewardAmount = 25;
                break;
            case 180:
                // Add 10 gold
                rewardAmount = 10;
                break;
            case 225:
                // Add 25 gold
                rewardAmount = 25;
                break;
            case 270:
                // Add 99 gold
                rewardAmount = 99;
                break;
            case 315:
                // Add 10 gold
                rewardAmount = 10;
                break;
            case 360:
                break;
        }
        winPanel.SetActive(true);

        if (rewardAmount > 0) {
            goldManager.AddGold(rewardAmount, false);
            wonGoldAmountTxt.text = rewardAmount.ToString();
            winGoldPanel.SetActive(true);
            winCarPanel.SetActive(false);
        } else {
            winGoldPanel.SetActive(false);
            winCarPanel.SetActive(true);
        }

    }
}
