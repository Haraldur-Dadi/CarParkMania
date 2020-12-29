using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DailySpin : MonoBehaviour {
    public Transform wheel;
    public GameObject unableToSpinPanel;
    public TextMeshProUGUI countdown;
    public GameObject winPanel;
    public GameObject winGoldPanel;
    public TextMeshProUGUI wonGoldAmountTxt;
    public GameObject winCarPanel;
    public TextMeshProUGUI winCarName;
    public Image winCarImg;
    public GameObject spinBtn;
    public GameObject closeBtn;

    bool canWinCar;
    public int[] winID;

    public GameObject carToWinBoardImg;
    public GameObject goldToWinBoardImg;
    public GameObject notification;

    void Start() {
        StartCoroutine(Counter());
    }

    bool canSpin() {
        canWinCar = PlayerPrefs.GetInt("DailyCarsWon", 0) < winID.Length;
        return DateTime.Today > DateTime.ParseExact(PlayerPrefs.GetString("LastDateSpun", "1582-09-15"), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
    }

    public void OpenUI() {
        notification.SetActive(canSpin());
        spinBtn.SetActive(notification.activeSelf);
        unableToSpinPanel.SetActive(!notification.activeSelf);
        carToWinBoardImg.SetActive(canWinCar);
        goldToWinBoardImg.SetActive(!canWinCar);
        winPanel.SetActive(false);
    }

    public void SpinWheel() {
        spinBtn.SetActive(false);
        DateTime currDate = DateTime.Now;
        SaveManager.Instance.SaveStringData("LastDateSpun", currDate.Year + "-" + currDate.Month.ToString().PadLeft(2, '0') + "-" + currDate.Day.ToString().PadLeft(2, '0'));
        SaveManager.Instance.IncreaseAchivementProgress(3);
        StartCoroutine(Spin());
    }

    IEnumerator Spin() {
        closeBtn.SetActive(false);
        int randomValue = UnityEngine.Random.Range(25, 35);
        float timeInterval = 0.1f;
        int rewardAmount = 0;

        AudioManager.Instance.PlayWheelSpinning();
        for (int i = 0; i < randomValue; i++) {
            wheel.Rotate(0, 0, 11.25f);
            if (i > Mathf.RoundToInt(randomValue * 0.65f)) {
                timeInterval = 0.2f;
            } else if (i > Mathf.RoundToInt(randomValue * 0.85f)) {
                timeInterval = 0.35f;
            }
            yield return new WaitForSeconds(timeInterval/2);
            wheel.Rotate(0, 0, 11.25f);
            yield return new WaitForSeconds(timeInterval/2);
        }

        if (Mathf.RoundToInt(wheel.eulerAngles.z) % 45 == 0) {
            wheel.Rotate(0, 0, 11.25f);
        }
        AudioManager.Instance.StopWheelSpinning();

        yield return new WaitForSeconds(1);
        while (Mathf.RoundToInt(wheel.eulerAngles.z) % 45 != 0) {
            wheel.Rotate(0, 0, -11.25f);
        }

        int finalAngle = Mathf.RoundToInt(wheel.eulerAngles.z);
        if (finalAngle == 45 || finalAngle == 180 || finalAngle == 315) {
            rewardAmount = 10;
        } else if (finalAngle == 135 || finalAngle == 225) {
            rewardAmount = 25;
        } else if (finalAngle == 90) {
            rewardAmount = 75;
        } else if (finalAngle == 270) {
            rewardAmount = 99;
        } else if (canWinCar) {
            AudioManager.Instance.PlayWinSound();
            Item winItem = ItemDb.Instance.GetItem(winID[UnityEngine.Random.Range(0, winID.Length)]);
            winCarName.text = winItem.name;
            winCarImg.sprite = winItem.sprite;
            SaveManager.Instance.SaveIntData("DailyCarsWon", 1);
            SaveManager.Instance.SaveIntData("PlayerCar" + winItem.ID + "Unlocked", 1);
            winGoldPanel.SetActive(false);
        } else {
            rewardAmount = 35;
        }

        if (rewardAmount > 0) {
            winCarPanel.SetActive(false);
            GoldManager.Instance.AddGold(rewardAmount, false);
            wonGoldAmountTxt.text = rewardAmount.ToString();
            AudioManager.Instance.PlayBuySound();
        } 
        closeBtn.SetActive(true);
        winPanel.SetActive(true);
        StartCoroutine(Counter());
    }

    IEnumerator Counter() {
        while (!canSpin()) {
            TimeSpan timeUntilMidnight = DateTime.Today.AddDays(1).Subtract(DateTime.Now);
            countdown.text = timeUntilMidnight.Hours.ToString().PadLeft(2, '0') + ":" + timeUntilMidnight.Minutes.ToString().PadLeft(2, '0') + ":" + timeUntilMidnight.Seconds.ToString().PadLeft(2, '0');

            yield return new WaitForSeconds(1);
        }
        OpenUI();
    }
}