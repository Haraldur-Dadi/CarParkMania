using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler {

    private Vector3 panelPos;
    public float swipeThreshold;

    public Shop shop;
    public bool showNext;

    public CanvasGroup canvasGroup;

    private void Start() {
        panelPos = transform.position;
        swipeThreshold = 0.2f;
    }

    public void OnDrag(PointerEventData data) {
        float difference = data.pressPosition.x - data.position.x;

        if (difference > 0) {
            if (!shop.canShowNext)
                return;
        } else if (difference < 0) {
            if (!shop.canShowPrev)
                return;
        }

        float alphaValue = 1 - (Mathf.Abs(difference) / ((Screen.width + 212.5f)/2));
        canvasGroup.alpha = Mathf.Clamp(alphaValue, 0, 1);
        transform.position = panelPos - new Vector3(difference, 0, 0);
    }

    public void OnEndDrag(PointerEventData data) {
        float threshold = (data.pressPosition.x - data.position.x) / Screen.width;

        if (Mathf.Abs(threshold) >= swipeThreshold) {
            Vector3 newPos = panelPos;
            if (threshold > 0) {
                // Show next item
                if (!shop.canShowNext)
                    return;

                newPos += new Vector3(-Screen.width, 0, 0);
                showNext = true;
            } else if (threshold < 0) {
                // Show prev item
                if (!shop.canShowPrev)
                    return;
                
                newPos += new Vector3(Screen.width, 0, 0);
                showNext = false;
            }
            StartCoroutine(SmoothSwipe(newPos));
            StartCoroutine(ShowAnotherItem(newPos));
        } else {
            canvasGroup.alpha = 1;
            StartCoroutine(SmoothSwipe(panelPos));
        }
    }

    IEnumerator SmoothSwipe(Vector3 endPos) {
        float t = 0f;
        while(t <= 1) {
            t += Time.deltaTime / .4f;
            transform.position = Vector3.Lerp(transform.position, endPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }

    IEnumerator ShowAnotherItem(Vector3 newPos) {
        yield return new WaitForSeconds(.4f);
        
        StopCoroutine(SmoothSwipe(newPos));
        canvasGroup.alpha = 1;

        if (showNext) {
            shop.ShowNextItem();
            transform.position += new Vector3(Screen.width * 2, 0, 0);
        } else {
            shop.ShowPrevItem();
            transform.position += new Vector3(-Screen.width * 2, 0, 0);
        }

        StartCoroutine(SmoothSwipe(panelPos));
    }
}
