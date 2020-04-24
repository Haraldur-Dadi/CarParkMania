using System.Collections.Generic;
using UnityEngine;

public class ItemDb : MonoBehaviour {
    
    public static ItemDb Instance;
    public List<Item> PlayerCars;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }

        BuildItemDb();
    }

    public Item GetItem(int ID) {
        return PlayerCars[ID];
    }

    public int GetLengthOfCat() {
        return PlayerCars.Count;
    }

    void BuildItemDb() {
        PlayerCars = new List<Item>() {
            new Item("Police Car", 0, false, 0),
            new Item("Fire Truck", 1, true, 25),
            new Item("Ambulance", 2, true, 25),
            new Item("Motorcycle", 3, true, 100),
            new Item("Police Motorcycle", 4, false, 0),
            new Item("Suv", 5, true, 150),
            new Item("Formula Car", 6, true, 200)
        };
    }
}
