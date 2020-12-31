using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarMovement : MonoBehaviour {
    public Vector2 startPosOfCar; // Vector positio, used for undo
    public Vector2 posOfCar; // Vector position, used to determine current position of car
    public Vector2 posToMoveCar; // Vector position, used to determine where to move the car

    public Car carToMove;
    float snapValue = 1.1f;

    public Button undoBtn;
    public List<UndoTuple> undoList;

    public void Refresh() {
        undoList = new List<UndoTuple>();
        AbleToUndo();
    }

    bool CarInWay() {
        /* Bool, checks if pathway to posToMoveCar is blocked by another car */
        bool carInWay = false;
        Vector2 raycastPos = posToMoveCar;

        if (carToMove.transform.eulerAngles.z == 0 || carToMove.transform.eulerAngles.z == 180) {
            raycastPos.y += (posToMoveCar.y > carToMove.transform.position.y) ? snapValue : -snapValue;
        } else {
            raycastPos.x += (posToMoveCar.x > carToMove.transform.position.x) ? snapValue : -snapValue;
        }

        RaycastHit2D[] hits = Physics2D.LinecastAll(raycastPos, carToMove.transform.position);
        foreach (RaycastHit2D hit in hits) {
            Car car = hit.transform.GetComponent<Car>();
            if (car != carToMove && car != null) {
                carInWay = true;
            }
        }
        return carInWay;
    }

    Car RaycastAtPosition(Vector2 posToCheck) {
        /* Raycast at position to see if we hit Car object */
        RaycastHit2D hit = Physics2D.Raycast(posToCheck, Vector2.zero);
        return hit.transform.GetComponent<Car>();
    }

    private void Update() {
        /* If touch on screen determine what to do */
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase) {
                case TouchPhase.Began:
                    // Check if player touched a car
                    carToMove = RaycastAtPosition(Camera.main.ScreenToWorldPoint(touch.position));
                    if (carToMove) { startPosOfCar = posOfCar = carToMove.transform.position; }
                    break;
                case TouchPhase.Moved:
                    // If we have a carToMove, determine where to move it on board
                    if (carToMove) {
                        // Get position for where to move the car
                        posToMoveCar = SnapPosToGrid(Camera.main.ScreenToWorldPoint(touch.position), carToMove.maxValAxis);
                        // If the snapPos of touchPos is not current position of car check if car in the way
                        if (posToMoveCar != posOfCar && !CarInWay()) {
                            carToMove.transform.position = posToMoveCar;
                            posOfCar = posToMoveCar;
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

    public Vector2 SnapPosToGrid(Vector2 snapPos, float maxValAxis) {
        /* Create grid like movement on board by rounding and clamping to within the boards border */
        if (carToMove.transform.eulerAngles.z == 0 || carToMove.transform.eulerAngles.z == 180) {
            // Car is facing up/down and is moving on y-axis
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
        } else {
            // Car is facing right/left and is moving on x-axis
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

    public void EndMove() {
        if (carToMove && posOfCar != startPosOfCar) {
            bool prevCar = false;
            if (undoList.Count > 0) { prevCar = undoList[undoList.Count - 1].car != carToMove; }
            if (!prevCar) {
                GameManager.Instance.ChangeMoves(true);
                AudioManager.Instance.PlayCarMoved();
                undoList.Add(new UndoTuple(carToMove, startPosOfCar));
                AbleToUndo();
            }
        }
        carToMove = null;
    }

    public void AbleToUndo() { undoBtn.interactable = undoList.Count > 0; }
    public void UndoMove() {
        AudioManager.Instance.PlayButtonClick();
        GameManager.Instance.ChangeMoves(false);
        int index = undoList.Count - 1;
        undoList[index].car.transform.position = undoList[index].movePos;
        undoList.RemoveAt(index);
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