using System.Collections.Generic;
using UnityEngine;

public class ChallengeLevelDb : MonoBehaviour {
    
    public List<ChallengeLevel> ChallengeLevels;

    private void Awake() {
        BuildChallengeLevelDb();
    }

    public ChallengeLevel GetChallengeLevel(int ID) {
        return ChallengeLevels[ID];
    }

    void BuildChallengeLevelDb() {
        ChallengeLevels = new List<ChallengeLevel> () {
            new ChallengeLevel(0, 6), // easy
            new ChallengeLevel(1, 5),
            new ChallengeLevel(2, 7),
            new ChallengeLevel(3, 7),
            new ChallengeLevel(4, 5),
            new ChallengeLevel(5, 6),
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
            new ChallengeLevel(24, 8),
            new ChallengeLevel(25, 11), // medium
            new ChallengeLevel(26, 12),
            new ChallengeLevel(27, 11),
            new ChallengeLevel(28, 9),
            new ChallengeLevel(29, 9),
            new ChallengeLevel(30, 12),
            new ChallengeLevel(31, 10),
            new ChallengeLevel(32, 10),
            new ChallengeLevel(33, 11),
            new ChallengeLevel(34, 10),
            new ChallengeLevel(35, 11),
            new ChallengeLevel(36, 12),
            new ChallengeLevel(37, 12),
            new ChallengeLevel(38, 11),
            new ChallengeLevel(39, 11),
            new ChallengeLevel(40, 11),
            new ChallengeLevel(41, 12),
            new ChallengeLevel(42, 12),
            new ChallengeLevel(43, 11),
            new ChallengeLevel(44, 12),
            new ChallengeLevel(45, 11),
            new ChallengeLevel(46, 11),
            new ChallengeLevel(47, 12),
            new ChallengeLevel(48, 12),
            new ChallengeLevel(49, 11),
            new ChallengeLevel(50, 15), // hard
            new ChallengeLevel(51, 13),
            new ChallengeLevel(52, 15),
            new ChallengeLevel(53, 14),
            new ChallengeLevel(54, 16),
            new ChallengeLevel(55, 15),
            new ChallengeLevel(56, 16),
            new ChallengeLevel(57, 15),
            new ChallengeLevel(58, 14),
            new ChallengeLevel(59, 16),
            new ChallengeLevel(60, 15),
            new ChallengeLevel(61, 13),
            new ChallengeLevel(62, 13),
            new ChallengeLevel(63, 14),
            new ChallengeLevel(64, 15),
            new ChallengeLevel(65, 16),
            new ChallengeLevel(66, 14),
            new ChallengeLevel(67, 15),
            new ChallengeLevel(68, 15),
            new ChallengeLevel(69, 15),
            new ChallengeLevel(70, 16),
            new ChallengeLevel(71, 15),
            new ChallengeLevel(72, 15),
            new ChallengeLevel(73, 16),
            new ChallengeLevel(74, 16),
            new ChallengeLevel(75, 16), // expert
            new ChallengeLevel(76, 16),
            new ChallengeLevel(77, 16),
            new ChallengeLevel(78, 17),
            new ChallengeLevel(79, 16),
            new ChallengeLevel(80, 18),
            new ChallengeLevel(81, 17),
            new ChallengeLevel(82, 18),
            new ChallengeLevel(83, 16),
            new ChallengeLevel(84, 17),
            new ChallengeLevel(85, 19),
            new ChallengeLevel(86, 20),
            new ChallengeLevel(87, 20),
            new ChallengeLevel(88, 18),
            new ChallengeLevel(89, 19),
            new ChallengeLevel(90, 18),
            new ChallengeLevel(91, 20),
            new ChallengeLevel(92, 19),
            new ChallengeLevel(93, 21),
            new ChallengeLevel(94, 21),
            new ChallengeLevel(95, 19),
            new ChallengeLevel(96, 22),
            new ChallengeLevel(97, 20),
            new ChallengeLevel(98, 22),
            new ChallengeLevel(99, 21)
        };
    }
}
