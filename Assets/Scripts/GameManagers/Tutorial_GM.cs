﻿using UnityEngine;

public class Tutorial_GM : MonoBehaviour {

    // How To Play, what is goal
    // Displays instructions to player
    // NO Timer

    public int tutDispAtIndex;
    public GameObject[] tutDisplays;

    public void Start() {
        tutDispAtIndex = 0;
    }

    public void ShowNextTutDisplay() {
        // If we have other displays to display
        if (tutDispAtIndex < tutDisplays.Length) {

            // Hide display
            tutDisplays[tutDispAtIndex].SetActive(false);

            tutDispAtIndex += 1;

            // Show next display
            tutDisplays[tutDispAtIndex].SetActive(true);
        }
    }
}
