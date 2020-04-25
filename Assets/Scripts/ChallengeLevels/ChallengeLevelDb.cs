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
            new ChallengeLevel(0, 6),
            new ChallengeLevel(1, 5),
            new ChallengeLevel(2, 7),
            new ChallengeLevel(3, 7),
            new ChallengeLevel(4, 5),
            new ChallengeLevel(5, 5),
            new ChallengeLevel(6, 6),
            new ChallengeLevel(7, 7),
            new ChallengeLevel(8, 5),
            new ChallengeLevel(9, 7),
            new ChallengeLevel(10, 7),
            new ChallengeLevel(11, 6),
            new ChallengeLevel(12, 5),
            new ChallengeLevel(13, 6),
            new ChallengeLevel(14, 8),
            new ChallengeLevel(15, 7),
            new ChallengeLevel(16, 8),
            new ChallengeLevel(17, 6),
            new ChallengeLevel(18, 8),
            new ChallengeLevel(19, 8),
            new ChallengeLevel(20, 8),
            new ChallengeLevel(21, 8),
            new ChallengeLevel(22, 8),
            new ChallengeLevel(23, 8),
            new ChallengeLevel(24, 8)
        };
    }
}
