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

    public int GetLengthOfCat(string cat) {
        return PlayerCars.Count;
    }

    void BuildItemDb() {
        PlayerCars = new List<Item>() {
            new Item("Police Car", "PlayerCar", 0, false, 0),
            new Item("Fire Truck", "PlayerCar", 1, true, 25),
            new Item("Ambulance", "PlayerCar", 2, true, 25),
            new Item("Motorcycle", "PlayerCar", 3, true, 100),
            new Item("Police Motorcycle", "PlayerCar", 4, false, 0)
        };
    }
}
