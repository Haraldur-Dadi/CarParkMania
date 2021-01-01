using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler {
    public Shop shop;
    public About about;
    CanvasGroup canvasGroup;
    RectTransform rectTransform;
    Vector2 startPos;
    Vector2 panelPos;
    float swipeThreshold;

    void Start() {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        startPos = panelPos = rectTransform.position;
        swipeThreshold = shop ? 0.1f : 0.25f;
    }

    public void OnDrag(PointerEventData data) {
        float differenceX = data.pressPosition.x - data.position.x;
        rectTransform.position = panelPos - new Vector2(differenceX, 0);

        if ((Mathf.Abs(differenceX) / Screen.width) >= swipeThreshold)
            canvasGroup.alpha = Mathf.Clamp(1 - (Mathf.Abs(differenceX) / (Screen.width/2)), .05f, 1f);
    }

    public void OnEndDrag(PointerEventData data) {
        Vector2 newPos = panelPos;
        float xThreshold = (data.pressPosition.x - data.position.x) / Screen.width;
        if (xThreshold >= swipeThreshold) {
            if (shop && shop.nextBtn.activeSelf || about && about.nextButton.activeSelf) {
                newPos += new Vector2(-Screen.width, 0);
                StartCoroutine(ShowAnotherItem(true));
            }
        } else if (xThreshold <= -swipeThreshold) {
            if (shop && shop.prevBtn.activeSelf || about && about.prevButton.activeSelf) {
                newPos += new Vector2(Screen.width, 0);
                StartCoroutine(ShowAnotherItem(false));
            }
        }
        StartCoroutine(SmoothSwipe(newPos));
    }

    IEnumerator SmoothSwipe(Vector2 endPos) {
        float t = 0f;
        while (t <= 1) {
            t += Time.deltaTime / .4f;
            rectTransform.position = Vector2.Lerp(rectTransform.position, endPos, Mathf.SmoothStep(0f, 1f, t));
            canvasGroup.alpha = t;
            yield return null;
        }
    }

    IEnumerator ShowAnotherItem(bool showNext) {
        yield return new WaitForSeconds(.4f);

        if (showNext) {
            if (shop) {
                shop.ShowNextItem(false);
            } else {
                about.ChangeTutImage(true);
            }
        } else {
            if (shop) {
                shop.ShowPrevItem(false);
            } else {
                about.ChangeTutImage(false);
            }
        }

        rectTransform.position = showNext ? panelPos + new Vector2(Screen.width * 2, 0) : panelPos + new Vector2(-Screen.width * 2, 0);
        StartCoroutine(SmoothSwipe(startPos));
    }
}