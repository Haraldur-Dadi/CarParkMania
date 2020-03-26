public class Standard_GM : GameManager {

    public override void Start() {
        base.Start();

        if (levelBuildIndex <= 26) {
            stageIndex = 1;
        }

        switch (stageIndex) {
            case 1:
                lastLevelInStageIndex = 26;
                break;
            default:
                break;
        }
    }
}
