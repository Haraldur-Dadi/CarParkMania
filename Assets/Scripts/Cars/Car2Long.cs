public class Car2Long : Car {
    // Movement x and y axis: -2.2, -1.1, 0, 1.1, 2.2
    // Stasis x and y axis: -2.75, -1.65, -0.55, 0.55, 1.65, 2.75

    public override void Start() {
        base.Start();

        length = 2;

        snapValue = 1.1f;
        maxValAxis = 2.2f;
    }

}
