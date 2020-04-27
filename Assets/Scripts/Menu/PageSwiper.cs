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
    public About about;
    public bool showNext;

    public CanvasGroup canvasGroup;

    private void Start() {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        startPos = rectTransform.position;
        panelPos = startPos;
        if (shop) {
            swipeThreshold = 0.1f;
        } else {
            swipeThreshold = 0.25f;
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

        if ((Mathf.Abs(differenceX) / Screen.width) >= swipeThreshold)
            canvasGroup.alpha = Mathf.Clamp(1 - (Mathf.Abs(differenceX) / (Screen.width/2)), .05f, 1f);
    }

    public void OnEndDrag(PointerEventData data) {
        Vector2 newPos = panelPos;
        float xThreshold = (data.pressPosition.x - data.position.x) / Screen.width;

        if (Mathf.Abs(xThreshold) >= swipeThreshold) {
            if (xThreshold > 0) {
                if (shop) {
                    if (!shop.canShowNext)
                        return;
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
            canvasGroup.alpha = 1;
            StartCoroutine(SmoothSwipe(newPos));
        }
    }

    IEnumerator SmoothSwipe(Vector2 endPos) {
        float t = 0f;
        while(t <= 1) {
            t += Time.deltaTime / .4f;
            rectTransform.position = Vector2.Lerp(rectTransform.position, endPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }

    IEnumerator ShowAnotherItem(Vector2 newPos) {
        yield return new WaitForSeconds(.4f);
        canvasGroup.alpha = 1;

        if (showNext) {
            if (shop) {
                shop.ShowNextItem();
            } else if (about) {
                about.ShowNextTutImage();
            }
            rectTransform.position = panelPos + new Vector2(Screen.width * 2, 0);
        } else {
            if (shop) {
                shop.ShowPrevItem();
            } else if (about) {
                about.ShowPrevTutImage();
            }
            rectTransform.position = panelPos + new Vector2(-Screen.width * 2, 0);
        }

        StartCoroutine(SmoothSwipe(startPos));
    }
}
