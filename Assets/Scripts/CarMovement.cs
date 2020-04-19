using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarMovement : MonoBehaviour {

    public GameManager gameManager;

    public Vector2 startPosOfCar; // Vector positio, used for undo
    public Vector2 posOfCar; // Vector position, used to determine current position of car
    public Vector2 posToMoveCar; // Vector position, used to determine where to move the car

    public Car carToMove;
    public float snapValue;

    public Button undoBtn;

    public List<UndoTuple> undoList;

    private void Start() {
        snapValue = 1.1f;

        undoList = new List<UndoTuple>();
        AbleToUndo();
    }

    bool CarInWay() {
        /* Bool, checks if pathway to posToMoveCar is blocked by another car */
        bool carInWay = false;
        Vector2 raycastPos = posToMoveCar;

        if (carToMove.transform.eulerAngles.z == 0 || carToMove.transform.eulerAngles.z == 180) {
            if (posToMoveCar.y > carToMove.transform.position.y) {
                raycastPos.y += snapValue;
            } else {
                raycastPos.y -= snapValue;
            }
        } else {
            if (posToMoveCar.x > carToMove.transform.position.x) {
                raycastPos.x += snapValue;
            } else {
                raycastPos.x -= snapValue;
            }
        }

        RaycastHit2D[] hits = Physics2D.LinecastAll(raycastPos, carToMove.transform.position);

        if (hits.Length > 0) {
            foreach (RaycastHit2D hit in hits) {
                Car car = hit.transform.GetComponent<Car>();
                if (car != carToMove && car != null) {
                    carInWay = true;
                }
            }
        }
        return carInWay;
    }

    Car RaycastAtPosition(Vector2 posToCheck) {
        /* Raycast at position to see if we hit Car object */
        RaycastHit2D hit = Physics2D.Raycast(posToCheck, Vector2.zero);

        if (hit) {
            return hit.transform.GetComponent<Car>();
        }
        return null;
    }

    private void Update() {
        if (!gameManager.finished) {
            /* If touch on screen determine what to do */
            if (Input.touchCount > 0) {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase) {
                    case TouchPhase.Began:
                        // Get the screen position of the touch
                        Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                        carToMove = RaycastAtPosition(touchPos);

                        // If the player touched a car, set variable carToMove and get position of Car
                        if (carToMove) {
                            startPosOfCar = carToMove.transform.position;
                            posOfCar = startPosOfCar;
                        }
                        break;
                    case TouchPhase.Moved:
                        // If we have a carToMove, determine where to move it on board
                        if (carToMove) {
                            // Get position for where to move the car
                            Vector2 touchPosMoved = Camera.main.ScreenToWorldPoint(touch.position);
                            touchPosMoved = SnapPosToGrid(touchPosMoved, carToMove.maxValAxis);
                            // If the snapPos of touchPos is not current position of car check if car in the way
                            if (touchPosMoved != posOfCar) {
                                posToMoveCar = touchPosMoved;
                                MoveCar();
                            }
                        }
                        break;
                    case TouchPhase.Canceled:
                        EndMove();
                        break;
                    case TouchPhase.Ended:
                        EndMove();
                        break;
                }
            }
        }
    }

    public Vector2 SnapPosToGrid(Vector2 snapPos, float maxValAxis) {
        /* Create grid like movement on board by rounding and clamping to within the boards border */

        // Car is facing up/down and is moving on y-axis
        if (carToMove.transform.eulerAngles.z == 0 || carToMove.transform.eulerAngles.z == 180) {
            snapPos.x = carToMove.transform.position.x;
            // Check if the position is outside of the maximum allowed movement values of car
            if (snapPos.y > maxValAxis) {
                snapPos.y = maxValAxis;
            } else if (snapPos.y < -maxValAxis) {
                snapPos.y = -maxValAxis;
            } else if (snapPos.y > carToMove.transform.position.y + (snapValue / 2)) {
                snapPos.y = carToMove.transform.position.y + snapValue;
            } else if (snapPos.y < carToMove.transform.position.y - (snapValue / 2)) {
                snapPos.y = carToMove.transform.position.y - snapValue;
            } else {
                snapPos.y = carToMove.transform.position.y;
            }
        }
        // Car is facing right/left and is moving on x-axis
        else {
            snapPos.y = carToMove.transform.position.y;
            // Check if the position is outside of the maximum allowed movement values of car
            if (snapPos.x > maxValAxis && carToMove.length != 0) {
                snapPos.x = maxValAxis;
            } else if (snapPos.x < -maxValAxis) {
                snapPos.x = -maxValAxis;
            } else if (snapPos.x > carToMove.transform.position.x + (snapValue / 2)) {
                snapPos.x = carToMove.transform.position.x + snapValue;
            } else if (snapPos.x < carToMove.transform.position.x - (snapValue / 2)) {
                snapPos.x = carToMove.transform.position.x - snapValue;
            } else {
                snapPos.x = carToMove.transform.position.x;
            }
        }

        return snapPos;
    }

    public void MoveCar() {
        // If there is no car in the way check if we can move car
        if (!CarInWay()) {
            carToMove.transform.position = posToMoveCar;
            posOfCar = posToMoveCar;
        }
    }

    public void EndMove() {
        // If we have a carToMove, deactivate it and set variable to null
        if (carToMove) {
            AudioManager.Instance.PlayCarMoved();
            UndoTuple tuple = new UndoTuple(carToMove, startPosOfCar);
            undoList.Add(tuple);

            carToMove = null;
            AbleToUndo();
        }
    }

    // UNDO features
    public void AbleToUndo() {
        if (undoList.Count > 0) {
            undoBtn.interactable = true;
        } else {
            undoBtn.interactable = false;
        }
    }

    public void UndoMove() {
        /* Undo the last move
           "Move carToMove to posToMove" */
            // Tuple (carToMove, posToMove)
        int index = undoList.Count - 1;

        carToMove = undoList[index].car;
        posToMoveCar = undoList[index].movePos;
        carToMove.transform.position = posToMoveCar;
        
        undoList.RemoveAt(index);
        carToMove = null;
        AbleToUndo();
    }
}

public class UndoTuple  {
    public Car car;
    public Vector2 movePos;

    public UndoTuple(Car carToAdd, Vector2 movePosToAdd) {
        car = carToAdd;
        movePos = movePosToAdd;
    }
}
