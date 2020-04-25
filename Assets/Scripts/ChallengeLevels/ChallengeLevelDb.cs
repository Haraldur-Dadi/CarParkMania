using System.Collections.Generic;
using UnityEngine;

public class ChallengeLevelDb : MonoBehaviour {
    
    public List<ChallengeLevel> ChallengeLevels;

    private void Start() {
        BuildLevelDb();
    }

    public ChallengeLevel GetChallengeLevel(int ID) {
        return ChallengeLevels[ID];
    }

    void BuildLevelDb() {
        ChallengeLevels = new List<ChallengeLevel> () {
            new ChallengeLevel(0, 5, 7, 10)
        };
    }
}
