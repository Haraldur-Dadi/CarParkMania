using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler {

    private Vector2 startPos;
    private Vector2 panelPos;
    private RectTransform rectTransform;
    public float swipeThreshold;

    public Shop shop;
    public GameModePanel gameMode;
    public About about;
    public bool showNext;

    public CanvasGroup canvasGroup;

    private void Start() {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        startPos = rectTransform.position;
        panelPos = startPos;
        if (gameMode) {
            swipeThreshold = 0.25f;
        } else if (shop) {
            swipeThreshold = 0.1f;
        }
    }

    public void OnDrag(PointerEventData data) {
        float differenceX = data.pressPosition.x - data.position.x;

        if (differenceX > 0) {
            if (shop) {
                if (!shop.canShowNext)
                    return;
            } else if (about) {
                if (!about.nextButton.activeSelf)
                    return;
            }
        } else if (differenceX < 0) {
            if (shop) {
                if (!shop.canShowPrev)
                    return;
            } else if (about) {
                if (!about.prevButton.activeSelf)
                    return;
            }
        }
        rectTransform.position = panelPos - new Vector2(differenceX, 0);
        
        if (gameMode) {
            float differenceY = data.pressPosition.y - data.position.y;
            rectTransform.position = panelPos - new Vector2(0, differenceY);
        }

        if (Mathf.Abs(differenceX / Screen.width) >= swipeThreshold)
            canvasGroup.alpha = Mathf.Clamp(1 - (Mathf.Abs(differenceX) / (Screen.width/2)), .05f, 1f);
    }

    public void OnEndDrag(PointerEventData data) {
        Vector2 newPos = panelPos;
        float xThreshold = (data.pressPosition.x - data.position.x) / Screen.width;

        if (gameMode) {
            Vector2 yDifference = new Vector2(0, data.pressPosition.y - data.position.y);
            newPos -= yDifference;
        }

        if (Mathf.Abs(xThreshold) >= swipeThreshold) {
            if (xThreshold > 0) {
                if (shop) {
                    if (!shop.canShowNext)
                        return;
                } else if (gameMode) {
                    if (CrossSceneManager.Instance.difficulty == 3) {
                        ResetPanel(newPos);
                        return;
                    }
                } else if (about) {
                    if (!about.nextButton.activeSelf)
                        return;
                }

                newPos += new Vector2(-Screen.width, 0);
                showNext = true;
            } else if (xThreshold < 0) {
                if (shop) {
                    if (!shop.canShowPrev)
                        return;
                } else if (gameMode) {
                    if (CrossSceneManager.Instance.difficulty == 0) {
                        ResetPanel(newPos);
                        return;
                    }
                } else if (about) {
                    if (!about.prevButton.activeSelf)
                        return;
                }
                
                newPos += new Vector2(Screen.width, 0);
                showNext = false;
            }
            StartCoroutine(ShowAnotherItem(newPos));
            StartCoroutine(SmoothSwipe(newPos));
        } else {
            ResetPanel(newPos);
        }
    }

    public void ResetPanel(Vector2 newPos) {
        canvasGroup.alpha = 1;
        if (gameMode) {
            newPos.y = Mathf.Clamp(newPos.y, startPos.y, startPos.y + (rectTransform.sizeDelta.y / 2));
        }
        StartCoroutine(SmoothSwipe(newPos));
    }

    IEnumerator SmoothSwipe(Vector2 endPos) {
        float t = 0f;
        while(t <= 1) {
            t += Time.deltaTime / .4f;
            rectTransform.position = Vector2.Lerp(rectTransform.position, endPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        if (gameMode)
            panelPos = rectTransform.position;
    }

    IEnumerator ShowAnotherItem(Vector2 newPos) {
        yield return new WaitForSeconds(.4f);
        canvasGroup.alpha = 1;

        if (showNext) {
            if (shop) {
                shop.ShowNextItem();
            } else if (gameMode) {
                gameMode.SelectDifficulty(CrossSceneManager.Instance.difficulty + 1);
            } else if (about) {
                about.ShowNextTutImage();
            }

            rectTransform.position = panelPos + new Vector2(Screen.width * 2, 0);
        } else {
            if (shop) {
                shop.ShowPrevItem();
            } else if (gameMode) {
                gameMode.SelectDifficulty(CrossSceneManager.Instance.difficulty - 1);
            } else if (about) {
                about.ShowPrevTutImage();
            }

            rectTransform.position = panelPos + new Vector2(-Screen.width * 2, 0);
        }

        StartCoroutine(SmoothSwipe(startPos));
    }
}
