public class ChallengeLevel {
    public int ID;
    public int star1;
    public int star2;
    public int star3;

    public ChallengeLevel (int ID, int star1, int star2, int star3) {
        this.ID = ID;
        this.star1 = star1;
        this.star2 = star2;
        this.star3 = star3;
    }

    public ChallengeLevel (ChallengeLevel level) {
        this.ID = level.ID;
        this.star1 = level.star1;
        this.star2 = level.star2;
        this.star3 = level.star3;
    }
}
