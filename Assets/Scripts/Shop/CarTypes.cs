using System.Collections.Generic;
using UnityEngine;

public class CarTypes : MonoBehaviour {
    public static CarTypes instance;

    public Sprite[] carSprites;
    public List<int> unlockableCars;
    public List<string> unlockStrs;
    public List<int> unlocked;

    private void Awake() {
        if (!instance) {
            instance = this;
        } else {
            Debug.Log("Another instance exists in the scene");
            Destroy(this);
        }
    }

    private void Start() {
        unlocked = new List<int>();

        PlayerPrefs.SetString("Car3UnlockableTxt", "Is unlocked by completing all 6x6 levels in the Beginner section of the Standard Pack");
        PlayerPrefs.SetString("Car4UnlockableTxt", "Is unlocked by completing all 6x6 levels in the Standard Pack");

        for (int i = 0; i < carSprites.Length; i++) {
            // Check if car has already been unlocked
            string unlockSearchStr = "Car" + i + "Unlocked";

            if (PlayerPrefs.GetInt(unlockSearchStr, 0) == 1) {
                unlocked.Add(i);
            }

            // Check if car is an unlockable type
            string ifUnlockableCar = "Car" + i + "Unlockable";

            if (PlayerPrefs.GetInt(ifUnlockableCar, 0) == 1) {
                unlockableCars.Add(i);
                // Also get the unlock string of that Car
                string unlockStr = ifUnlockableCar + "Txt";
                unlockStrs.Add(PlayerPrefs.GetString(unlockStr, "a"));
            }
        }
    }

    public Sprite GetCarImg(int carId) {
        return carSprites[carId];
    }

    public string GetCarName(int carId) {
        return carSprites[carId].name;
    }

    public void UnlockCar(int carId) {
        unlocked.Add(carId);

        string insertStr = "Car" + carId + "Unlocked";
        PlayerPrefs.SetInt(insertStr, 1);
    }
}
