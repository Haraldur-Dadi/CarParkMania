public class PlayerCar : Car {
    public override void Start() {
        base.Start();

        length = 0;
        snapValue = 1.1f;
        maxValAxis = 2.2f;
    }
}
