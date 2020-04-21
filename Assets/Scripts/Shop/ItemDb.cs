using System.Collections.Generic;
using UnityEngine;

public class ItemDb : MonoBehaviour {
    
    public static ItemDb Instance;

    public List<Item> PlayerCars;
    public List<Item> Cars2;
    public List<Item> Cars3;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        BuildItemDb();
    }

    public Item GetItem(string cat, int ID) {
        Item returnItem = null;

        if (cat == "PlayerCar") {
            returnItem = PlayerCars[ID];
        } else if (cat == "2LongCar") {
            returnItem = Cars2[ID];
        } else if (cat == "3LongCar") {
            returnItem = Cars3[ID];
        }
        return returnItem;
    }

    public int GetLengthOfCat(string cat) {
        int returnLength = -1;

        if (cat == "PlayerCar") {
            returnLength = PlayerCars.Count;
        } else if (cat == "2LongCar") {
            returnLength = Cars2.Count;
        } else if (cat == "3LongCar") {
            returnLength = Cars3.Count;
        }
        return returnLength;
    }

    void BuildItemDb() {
        PlayerCars = new List<Item>() {
            new Item("Police Car", "PlayerCar", 0, false, 0),
            new Item("Fire Truck", "PlayerCar", 1, true, 25),
            new Item("Ambulance", "PlayerCar", 2, true, 25),
            new Item("Police Motorcycle", "PlayerCar", 3, false, 0)
        };
        Cars2 = new List<Item>() {
            new Item("Standard", "2LongCar", 0, false, 0)
        };
        Cars3 = new List<Item>() {
            new Item("Standard", "3LongCar", 0, false, 0)
        };
    }
}
