using UnityEngine;

public class Car : MonoBehaviour {
    // If the rotation.z = 0, movement is on y-axis, else it is on x-axis

    public BoxCollider2D boxCol2D;

    public int length; // 0 = playerCar

    public float snapValue;
    public float maxValAxis;

    public bool canMoveUp;
    public bool canMoveDown;

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

        AllowMovement();
    }

    public void AllowMovement() {
        canMoveUp = true;
        canMoveDown = true;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision) {
            if (collision.CompareTag("Car")) {
                carInWay = collision.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        carInWay = null;
    }
}
