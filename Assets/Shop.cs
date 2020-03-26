using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour {
    public CarTypes carTypes;

    public GameObject shopUI;

    public int carId;

    public TextMeshProUGUI carName;
    public Image carImg;

    public GameObject showPrev;
    public GameObject showNext;

    public GameObject buyCarBtn;
    public GameObject equipCarBtn;
    public TextMeshProUGUI unlockCarTxt;

    private void Awake() {
        //PlayerPrefs.SetInt("EquippedCar", 0);
        //PlayerPrefs.SetInt("Car0Unlocked", 1);
    }

    private void Start() {
        carTypes = CarTypes.instance;
        DisplayShop();
    }

    public void DisplayShop() {
        shopUI.SetActive(!shopUI.activeSelf);

        if (shopUI.activeSelf == true) {
            carId = 0;
            UpdateShopUI();
        }
    }

    public void UpdateShopUI() {
        /* Updates name, img and display ui/btns for cars in shop */
        carName.text = carTypes.GetCarName(carId);
        carImg.sprite = carTypes.GetCarImg(carId);

        // Display ui for equipped cars, unlockable cars or cars that player can buy
        if (carId == PlayerPrefs.GetInt("EquippedCar")) {
            // Player already has this car equiped
            buyCarBtn.SetActive(false);
            equipCarBtn.SetActive(true);
            unlockCarTxt.gameObject.SetActive(false);

            equipCarBtn.GetComponent<Button>().interactable = false;
            equipCarBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Equipped";
        } else if (carTypes.unlocked.Contains(carId)) {
            // Player has unlocked the car and can equip it
            buyCarBtn.SetActive(false);
            equipCarBtn.SetActive(true);
            unlockCarTxt.gameObject.SetActive(false);

            equipCarBtn.GetComponent<Button>().interactable = true;
            equipCarBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Equip";
        } else if (carTypes.unlockableCars.Contains(carId)) {
            // Player has not yet unlocked the car and cant buy it
            equipCarBtn.SetActive(false);
            buyCarBtn.SetActive(false);
            unlockCarTxt.gameObject.SetActive(true);
            unlockCarTxt.text = carTypes.unlockStrs[carTypes.unlockableCars.IndexOf(carId)];
        } else {
            // Player has not yet unlocked the car and needs to buy it
            equipCarBtn.SetActive(false);
            buyCarBtn.SetActive(true);
            unlockCarTxt.gameObject.SetActive(false);
        }

        // Display button based on what car looking at
        if (carId == 0) {
            // First car, no prev car
            showPrev.SetActive(false);
        } else if (carId == 4) {
            // Last car, no next car
            showNext.SetActive(false);
        } else {
            // Somewhere in middle, prev and next car
            showPrev.SetActive(true);
            showNext.SetActive(true);
        }
    }

    public void UpdateShopUIBtnEvent(bool increase) {
        /* Void that allows prev/next buttons to update the shop ui */

        if (increase) {
            carId += 1;
        } else {
            carId -= 1;
        }

        UpdateShopUI();
    }

    public void BuyCar() {
        carTypes.UnlockCar(carId);
        UpdateShopUI();
    }

    public void EquipCar() {
        PlayerPrefs.SetInt("EquippedCar", carId);
        UpdateShopUI();
    }
}
