﻿using UnityEngine;
using TMPro;

public class GoldManager : MonoBehaviour {

    public static GoldManager Instance;
    private SaveManager saveManager;

    private int gold;
    public TextMeshProUGUI goldTxt;

    public GameObject icon;
    public GameObject adForGoldCon;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }

    private void Start() {
        saveManager = SaveManager.Instance;

        gold = PlayerPrefs.GetInt("Gold", 0);
        adForGoldCon.SetActive(false);
    }

    public void AddGold(int goldToAdd, bool ad) {
        gold += goldToAdd;
        saveManager.SaveIntData("Gold", gold);

        if (ad)
            ToggleAdGoldConformation();
    }

    public void SubtractGold(int goldToSub) {
        gold -= goldToSub;
        saveManager.SaveIntData("Gold", gold);
    }

    public void ToggleAdGoldConformation() {
        icon.SetActive(!icon.activeSelf);
        adForGoldCon.SetActive(!adForGoldCon.activeSelf);
    }
}
