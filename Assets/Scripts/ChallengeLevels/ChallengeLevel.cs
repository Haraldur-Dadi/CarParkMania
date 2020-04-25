public class ChallengeLevel {
    public int ID;
    public int minMoves;

    public ChallengeLevel (int ID, int minMoves) {
        this.ID = ID;
        this.minMoves = minMoves;
    }

    public ChallengeLevel (ChallengeLevel level) {
        this.ID = level.ID;
        this.minMoves = level.minMoves;
    }
}
