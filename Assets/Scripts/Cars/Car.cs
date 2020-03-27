using UnityEngine;

public class Car : MonoBehaviour {
    // If the rotation.z = 0, movement is on y-axis, else it is on x-axis

    public BoxCollider2D boxCol2D;

    public int length; // 0 = playerCar

    public float snapValue;
    public float maxValAxis;

    public GameObject carInWay;
    public SpriteRenderer carImg;

    public virtual void Start() {
        // Get requried components
        boxCol2D = GetComponent<BoxCollider2D>();
        carImg = gameObject.transform.Find("CarBody").GetComponent<SpriteRenderer>();

        snapValue = 1.1f;

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
