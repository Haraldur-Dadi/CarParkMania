using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {

    public bool run = false;

    public SceneFader sceneFader;

    public GameObject stageButtonsParent;
    public Button[] stageButtons;

    public GameObject[] stages;

    public List<GameObject> levelButtonParents;
    public List<Button> levelButtons;

    public void Start() {
        run = true;
        sceneFader = SceneFader.Instance;

        // GameObjects
        stages = GameObject.FindGameObjectsWithTag("StagesViewport");

        // Buttons
            // Stages
        stageButtonsParent = GameObject.Find("/Canvas/LevelSelector/Stages/StagesViewport/StagesParent");
        stageButtons = stageButtonsParent.GetComponentsInChildren<Button>();

            // Levels
        levelButtonParents.Add(GameObject.Find("/Canvas/LevelSelector/StageParent/6x6/Levels/LevelsViewport/LevelsParent"));
        //levelButtonParents.Add(GameObject.Find("/Canvas/LevelSelector/StageParent/10x10/Levels/LevelsViewport/LevelsParent"));
        //levelButtonParents.Add(GameObject.Find("/Canvas/LevelSelector/StageParent/15x15/Levels/LevelsViewport/LevelsParent"));
        // levelButtonParents.Add(GameObject.Find("/Canvas/LevelSelector/StageParent/Stage4/Levels/LevelsViewport/LevelsParent"));
        // levelButtonParents.Add(GameObject.Find("/Canvas/LevelSelector/StageParent/Stage5/Levels/LevelsViewport/LevelsParent"));
        // levelButtonParents.Add(GameObject.Find("/Canvas/LevelSelector/StageParent/Stage6/Levels/LevelsViewport/LevelsParent"));
        // levelButtonParents.Add(GameObject.Find("/Canvas/LevelSelector/StageParent/Stage7/Levels/LevelsViewport/LevelsParent"));
        // levelButtonParents.Add(GameObject.Find("/Canvas/LevelSelector/StageParent/Stage8/Levels/LevelsViewport/LevelsParent"));

        for (int i = 0; i < levelButtonParents.Count; i++) {
            int buildIndex = i + 2;

            Button[] buttons = levelButtonParents[i].GetComponentsInChildren<Button>();
            for (int x = 0; x < buttons.Length; x++) {
                int level = x;

                levelButtons.Add(buttons[x]);
                buttons[x].onClick.AddListener(() => SelecteLevel(buildIndex, level));
            }
        }

        if (run) {
            for (int i = 0; i < stages.Length; i++) {
                stages[i].SetActive(false);
            }

            int stageCompleted = PlayerPrefs.GetInt("StageCompleted", 1);
            int levelReached = PlayerPrefs.GetInt("LevelReached", 1);

            for (int i = 0; i < stageButtons.Length; i++) {
                if (i > stageCompleted && stageCompleted < PlayerPrefs.GetInt("LockedLevel", 6)) {
                    stageButtons[i].interactable = false;
                }
            }
            for (int i = 0; i < levelButtons.Count; i++) {
                if (i + 1 > levelReached) {
                    levelButtons[i].interactable = false;
                }
            }
        }
    }

    public void SelectStage(int stageNr) {
        stages[stageNr - 1].SetActive(!stages[stageNr].gameObject.activeSelf);
    }

    public void SelecteLevel(int buildIndex, int level) {
        sceneFader.FadeToBuildIndex(buildIndex);
        PlayerPrefs.SetInt("boardToLoad", level);

        Debug.Log(buildIndex + " " + level);
    }
}
