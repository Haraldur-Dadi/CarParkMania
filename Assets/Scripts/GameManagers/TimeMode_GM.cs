using System.Collections;
using UnityEngine;
using TMPro;

public class TimeMode_GM : GameManager {

    // Timed gamemode, moves don't count
    // 3 stars, 2 stars, 1 star
    // TBD.

    public TextMeshProUGUI clockTxt;
    public GameObject pausedImg;

    public int min;
    public int sec;

    public override void Start() {
        base.Start();

        clockTxt = topUI.transform.Find("Clock").GetComponentInChildren<TextMeshProUGUI>();
        pausedImg = GameObject.Find("PausedImg").gameObject;

        min = 0;
        sec = -1;

        StartCoroutine("Timer");
    }

    public void LevelFailed() {
        finished = true;
    }

    IEnumerator Timer() {
        /* Timer, times how long it takes for you to complete level.
           If it reaches max limit you fail */

        while (!finished) {
            sec += 1;
            if (sec >= 60) {
                sec = 0;
                min += 1;
            }

            if (min == 60) {
                LevelFailed();
            }

            if (sec < 10 && min < 10) {
                clockTxt.text = string.Format("0{0}:0{1}", min, sec);
            } else if (sec < 10) {
                clockTxt.text = string.Format("{0}:0{1}", min, sec);
            } else if (min < 10) {
                clockTxt.text = string.Format("0{0}:{1}", min, sec);
            } else {
                clockTxt.text = string.Format("{0}:{1}", min, sec);
            }

            yield return new WaitForSeconds(1);
        }
        yield return null;
    }
}
