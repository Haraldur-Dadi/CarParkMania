using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler {

    private Vector3 panelPos;
    public float swipeThreshold;

    public CanvasGroup canvasGroup;

    private void Start() {
        panelPos = transform.position;
        swipeThreshold = 0.2f;
    }

    public void OnDrag(PointerEventData data) {
        transform.position = panelPos - new Vector3(data.pressPosition.x - data.position.x, 0, 0);
    }

    public void OnEndDrag(PointerEventData data) {
        float threshold = (data.pressPosition.x - data.position.x) / Screen.width;
        if (Mathf.Abs(threshold) >= swipeThreshold) {
            Vector3 newPos = panelPos;
            if (threshold > 0) {
                // Show next item
                newPos += new Vector3(-Screen.width, 0, 0);
            } else if (threshold < 0) {
                // Show prev item
                newPos += new Vector3(Screen.width, 0, 0);
            }
            StartCoroutine(SmoothSwipe(newPos));
            panelPos = newPos;
        } else {
            StartCoroutine(SmoothSwipe(panelPos));
        }
    }

    IEnumerator SmoothSwipe(Vector3 endPos) {
        float t = 0f;
        while(t <= 1) {
            t += Time.deltaTime / .5f;
            transform.position = Vector3.Lerp(panelPos, endPos, Mathf.SmoothStep(0f, 1f, t));
            canvasGroup.alpha = 1 - t;
            yield return null;
        }
    }
}
