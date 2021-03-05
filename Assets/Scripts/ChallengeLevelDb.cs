using System.Collections.Generic;
using UnityEngine;

public class ChallengeLevelDb : MonoBehaviour {
    public List<int> ChallengeLevels = new List<int> () {
            6, 5, 7, 7, 5, 6, 6, 7, 5, 7, 7, 6, 5, 6, 8, 7, 8, 6, 8, 8, 8, 8, 8, 8, 8, // easy
            11, 12, 11, 9, 9, 12, 10, 10, 11, 10, 11, 12, 12, 11, 11, 11, 12, 12, 11, 12, 11, 11, 12, 12, 11, // medium
            15, 13, 15, 14, 16, 15, 16, 15, 14, 16, 15, 13, 13, 14, 15, 16, 14, 15, 15, 15, 16, 15, 15, 16, 16, // hard
            16, 16, 16, 17, 16, 18, 17, 18, 16, 17, 19, 20, 20, 18, 19, 18, 20, 19, 21, 21, 19, 22, 20, 22, 21 // expert
        };
    public List<int> Challenge8x8Levels = new List<int> () {
            8, 5, 6, 6, 5, 6, 6, 8, 8, 5, 7, 9, 9, 8, 9, 10, 9, 10, 9, 9, 10, 10, 10, 10, 11, // easy
            10, 10, 10, 11, 10, 13, 11, 11, 14, 11, 14, 14, 11, 14, 15, 13, 14, 14, 15, 16, 15, 15, 16, 16, 16, // medium
            16, 16, 17, 16, 17, 17, 17, 18, 19, 18, 19, 19, 18, 18, 20, 19, 19, 18, 21, 19, 20, 21, 21, 22, 22, // hard
            20, 21, 21, 22, 22, 22, 20, 21, 22, 21, 20, 22, 23, 23, 21, 23, 22, 24, 24, 25, 27, 26, 28, 27, 31 // expert
        };

    public int GetChallengeLevel (int ID) { 
        if (CrossSceneManager.Instance.gameModeNr > 2) {
            return Challenge8x8Levels[ID];
        } else {
            return ChallengeLevels[ID];
        }
    }
}