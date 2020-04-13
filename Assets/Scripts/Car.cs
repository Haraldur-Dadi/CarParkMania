using UnityEngine;

public class Car : MonoBehaviour {

    public float length; // 0 = playerCar
    public float maxValAxis;

    public BoxCollider2D boxCol2D;
    public SpriteRenderer carImg;

    public virtual void Start() {
        // Get requried components
        ActiveCar(false);
    }

    public void ActiveCar(bool active) {
        /* Sets required information depending if the car is active */
        if (active) {
            boxCol2D.isTrigger = true;
        } else {
            boxCol2D.isTrigger = false;
        }
    }
}
