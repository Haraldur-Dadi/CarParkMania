using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DailySpin : MonoBehaviour {
    
    SaveManager saveManager;
    GoldManager goldManager;
    ItemDb itemDb;

    public GameObject wheel;
    public GameObject unableToSpinPanel;
    public TextMeshProUGUI countdown;
    public GameObject winPanel;
    public GameObject winGoldPanel;
    public TextMeshProUGUI wonGoldAmountTxt;
    public GameObject winCarPanel;
    public TextMeshProUGUI winCarName;
    public Button spinBtn;

    private int randomValue;
    private float timeInterval;
    private int finalAngle;

    private Item winItem;
    public string[] winCatagories;
    public int[] winID;
    private int rewardAmount;

    private bool open;
    public Animator notification;

    void Start() {
        saveManager = SaveManager.Instance;
        goldManager = GoldManager.Instance;
        itemDb = ItemDb.Instance;

        //if (!PlayerPrefs.HasKey("LastDateSpun"))
            PlayerPrefs.SetString("LastDateSpun", "1582-09-15");
        
        StartCoroutine(Counter());
    }

    private bool canSpin() {
        DateTime currDate = DateTime.Today;
        DateTime lastSpinDate = DateTime.ParseExact(PlayerPrefs.GetString("LastDateSpun", "1582-09-15"), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        
        if (currDate > lastSpinDate) {
            notification.SetTrigger("Avail");
            return true;
        }
        return false;
    }

    public void OpenUI() {
        if (canSpin()) {
            spinBtn.gameObject.SetActive(true);
            spinBtn.interactable = true;
            unableToSpinPanel.SetActive(false);
        } else {
            spinBtn.gameObject.SetActive(false);
            unableToSpinPanel.SetActive(true);
        }

        winPanel.SetActive(false);
    }

    public void SpinWheel() {
        AudioManager.Instance.PlayButtonClick();
        spinBtn.interactable = false;
        DateTime currDate = DateTime.Now;
        saveManager.SaveStringData("LastDateSpun", currDate.Year + "-" + currDate.Month.ToString().PadLeft(2, '0') + "-" + currDate.Day.ToString().PadLeft(2, '0'));
        notification.SetTrigger("UnAvail");
        StartCoroutine(Spin());
        StartCoroutine(Counter());
    }

    IEnumerator Spin() {
        randomValue = UnityEngine.Random.Range(30, 40);
        timeInterval = 0.1f;

        AudioManager.Instance.PlayWheelSpinning();
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

        if (Mathf.RoundToInt(wheel.transform.eulerAngles.z) % 45 == 0) {
            wheel.transform.Rotate(0, 0, 11.25f);
        }
        AudioManager.Instance.StopWheelSpinning();

        yield return new WaitForSeconds(1);

        while (Mathf.RoundToInt(wheel.transform.eulerAngles.z) % 45 != 0) {
            wheel.transform.Rotate(0, 0, -11.25f);
        }

        finalAngle = Mathf.RoundToInt(wheel.transform.eulerAngles.z);
        wheel.transform.Rotate(0, 0, 22.5f);
        spinBtn.gameObject.SetActive(false);

        switch (finalAngle) {
            case 0:
                // Give Car
                int randomInt = winID[UnityEngine.Random.Range(0, winID.Length - 1)];
                string randomCat = winCatagories[UnityEngine.Random.Range(0, winCatagories.Length - 1)]; 

                winItem = itemDb.GetItem(randomCat, randomInt);
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
        }
        winPanel.SetActive(true);

        if (rewardAmount > 0) {
            goldManager.AddGold(rewardAmount, false);
            wonGoldAmountTxt.text = rewardAmount.ToString();
            winGoldPanel.SetActive(true);
            winCarPanel.SetActive(false);
            AudioManager.Instance.PlayBuySound();
        } else {
            winGoldPanel.SetActive(false);
            winCarPanel.SetActive(true);
            winCarName.name = winItem.name;
            PlayerPrefs.SetInt(winItem.catagory + winItem.ID + "Unlocked", 1);
        }
    }

    IEnumerator Counter() {
        while (!canSpin()) {
            TimeSpan timeUntilMidnight = DateTime.Today.AddDays(1).Subtract(DateTime.Now);
            countdown.text = timeUntilMidnight.Hours.ToString().PadLeft(2, '0') + ":" + timeUntilMidnight.Minutes.ToString().PadLeft(2, '0') + ":" + timeUntilMidnight.Seconds.ToString().PadLeft(2, '0');

            yield return new WaitForSeconds(1);
        }
    }
}
