using UnityEngine;

public class Item {
    public string name; // Name of item
    public int ID; // id in catagory
    public bool needBuy; // true = car can be bought, false = needs to be unlocked
    public int cost; // Cost to buy
    public Sprite sprite;

    public Item (string name, int id, bool buy, int cost) {
        this.name = name;
        this.ID = id;
        this.needBuy = buy;
        this.cost = cost;
        this.sprite = Resources.Load<Sprite>("Sprites/Cars/PlayerCars/" + name);
    }
}