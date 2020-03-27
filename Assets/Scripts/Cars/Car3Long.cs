public class Car3Long : Car {
    // Movement x and y axis: -1.65, -0.55, 0.55, 1.65
    // Stasis x and y axis: -2.75, -1.65, -0.55, 0.55, 1.65, 2.75


    public override void Start() {
        base.Start();

        length = 3;

        snapValue = 1.1f;
        maxValAxis = 1.65f;
    }

}