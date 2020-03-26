using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarMovement : MonoBehaviour {

    public GameManager gameManager;

    public Vector2 startPosOfCar; // Vector positio, used for undo
    public Vector2 posOfCar; // Vector position, used to determine current position of car
    public Vector2 posToMoveCar; // Vector position, used to determine where to move the car

    public Car carToMove;
    public bool carMoveOnYAxis;

    public Button undoBtn;

    public Animator notificationPopUpAnim;

    public List<UndoTuple> undoList;

    private void Start() {
        gameManager = GameManager.instance;

        undoBtn = GameObject.Find("UndoButton").GetComponent<Button>();
        undoBtn.onClick.AddListener(delegate { UndoMove(); });
        undoList = new List<UndoTuple>();

        notificationPopUpAnim = GameObject.Find("NotificationPanel").GetComponent<Animator>();

        AbleToUndo();
    }

    bool CarInWay() {
        /* Bool, checks if pathway to posToMoveCar is blocked by another car */
        Vector2 colOrPos = carToMove.boxCol2D.offset;

        // Check if there is a Car object on the point
        if (carMoveOnYAxis) {
            if (posToMoveCar.x > posOfCar.x) {
                carToMove.boxCol2D.offset += new Vector2(carToMove.snapValue / 3, 0);
            } else {
                carToMove.boxCol2D.offset -= new Vector2(carToMove.snapValue / 3, 0);
            }
        } else {
            if (posToMoveCar.y > posOfCar.y) {
                carToMove.boxCol2D.offset += new Vector2(0, carToMove.snapValue / 3);
            } else {
                carToMove.boxCol2D.offset -= new Vector2(0, carToMove.snapValue / 3);
            }
        }

        GameObject carInWay = carToMove.carInWay;
        carToMove.boxCol2D.offset = colOrPos;

        if (carInWay) {
            // If car in the way is "above" (higher axis value), then we can't move "up"
            // Else car is below (lower axis value), then we can't move the car "down"
            if (carInWay.transform.position.y > carToMove.transform.position.y | carInWay.transform.position.x > carToMove.transform.position.x) {
                carToMove.canMoveUp = false;
                carToMove.canMoveDown = true;
            } else {
                carToMove.canMoveUp = true;
                carToMove.canMoveDown = false;
            }

            return true;
        }

        return false;
    }

    bool CanMoveCar() {
        // If car can't move "up" or "down"
        // - "up" check if axis value is greater than car values, return false
        // - "down" check if axis value is lower then car values, return fale
        if (!carToMove.canMoveUp) {
            if (posToMoveCar.y > posOfCar.y | posToMoveCar.x > posOfCar.x) {
                return false;
            }
        } else if (!carToMove.canMoveDown) {
            if (posToMoveCar.y < posOfCar.y | posToMoveCar.x < posOfCar.x) {
                return false;
            }
        }

        // Passed all test, path is clear
        return true;
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
        if (!(gameManager.finished || gameManager.paused)) {
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
                            carToMove.ActiveCar(true);
                            startPosOfCar = carToMove.transform.position;
                            posOfCar = startPosOfCar;
                        }
                        break;
                    case TouchPhase.Moved:
                        // If we have a carToMove, determine where to move it on board
                        if (carToMove) {
                            // Get position for where to move the car
                            Vector2 touchPosMoved = Camera.main.ScreenToWorldPoint(touch.position);
                            touchPosMoved = SnapPosToGrid(touchPosMoved, carToMove.snapValue, carToMove.maxValAxis);
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

    public Vector2 SnapPosToGrid(Vector2 snapPos, float snapValue, float maxValAxis) {
        /* Create grid like movement on board by rounding and clamping to within the boards border */

        if (carToMove.transform.rotation.z == 0) {
            // Round positions y axis and clamp it within range of board
            snapPos.y = Mathf.Clamp(Mathf.Round(snapPos.y / snapValue) * snapValue, -maxValAxis, maxValAxis);
            snapPos.x = carToMove.transform.position.x;

            if (carToMove.length == 3) {
                if (snapPos.y == 0) {
                    snapPos.y = carToMove.transform.position.y;
                } else {
                    snapPos.y = SnapMoveAxis3Long(snapPos.y);
                }
            }

            carMoveOnYAxis = true;
        } else {
            // Round positions x axis and clamp it within range of board
            snapPos.x = Mathf.Round(snapPos.x / snapValue) * snapValue;
            snapPos.y = carToMove.transform.position.y;

            if (carToMove.length == 0) {
                snapPos.x = Mathf.Clamp(snapPos.x, -maxValAxis, 10);
            } else {
                snapPos.x = Mathf.Clamp(snapPos.x, -maxValAxis, maxValAxis);
            }

            if (carToMove.length == 3) {
                if (snapPos.x == 0) {
                    snapPos.x = carToMove.transform.position.x;
                } else {
                    snapPos.x = SnapMoveAxis3Long(snapPos.x);
                }
            }

            carMoveOnYAxis = false;
        }

        return snapPos;
    }

    public float SnapMoveAxis3Long(float fToSnap) {
        if (fToSnap > 0) {
            fToSnap = fToSnap - 0.45f;
        } else {
            fToSnap = fToSnap + 0.45f;
        }

        return fToSnap;
    }

    public void MoveCar() {
        // If we can move car move the car
        if (CanMoveCar()) {
            // If there is no car in the way check if we can move car
            if (!CarInWay()) {
                carToMove.transform.position = posToMoveCar;
                posOfCar = posToMoveCar;
                carToMove.AllowMovement();
            }
        }
    }

    public void EndMove() {
        // If we have a carToMove, deactivate it and set variable to null
        if (carToMove) {
            UndoTuple tuple = new UndoTuple(carToMove, startPosOfCar);
            undoList.Add(tuple);

            carToMove.ActiveCar(false);
            carToMove = null;
            gameManager.IncreaseMoves(1);

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
        AbleToUndo();
        carToMove = null;

        gameManager.DecreaseMoves(1);
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
