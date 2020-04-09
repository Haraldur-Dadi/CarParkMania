using UnityEngine;

public class Item {
    /* Constructor class for items that are to be found within the game */

    public string name; // Name of item
    public string catagory; // PlayerCar, 2LongCar, 3LongCar, Board
    public int ID; // id in catagory
    public bool needBuy; // true = car can be bought, false = needs to be unlocked
    public int cost; // Cost to buy
    public Sprite sprite;

    public Item (string name, string cat, int id, bool buy, int cost) {
        this.name = name;
        this.catagory = cat;
        this.ID = id;
        this.needBuy = buy;
        this.cost = cost;
        if (cat == "PlayerCar") {
            this.sprite = Resources.Load<Sprite>("Sprites/Cars/PlayerCars/" + name);
        } else if (cat == "2LongCar") {
            this.sprite = Resources.Load<Sprite>("Sprites/Cars/2Long/" + name);
        } else if (cat == "3LongCar") {
            this.sprite = Resources.Load<Sprite>("Sprites/Cars/3Long/" + name);
        }
    }

    public Item (Item item) {
        this.name = item.name;
        this.catagory = item.catagory;
        this.ID = item.ID;
        this.needBuy = item.needBuy;
        this.cost = item.cost;
        this.sprite = item.sprite;
    }
}