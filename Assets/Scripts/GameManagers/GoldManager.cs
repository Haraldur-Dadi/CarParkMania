﻿using UnityEngine;
using TMPro;

public class GoldManager : MonoBehaviour {

    public static GoldManager Instance;
    private SaveManager saveManager;

    private int gold;
    public TextMeshProUGUI goldTxt;

    public Animator goldAnim;
    public Animator goldTxtAnim;
    public TextMeshProUGUI goldTxtAnimTxt;

    private void Awake () {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        saveManager = SaveManager.Instance;

        gold = PlayerPrefs.GetInt("Gold", 0);
        goldTxt.text = gold.ToString();
    }

    public bool CanBuy(int amount) {
        if (amount <= gold) {
            return true;
        }
        return false;
    }

    public void AddGold(int goldToAdd, bool ad) {
        gold += goldToAdd;
        saveManager.SaveIntData("Gold", gold);

        goldTxt.text = gold.ToString();
        goldTxtAnim.SetTrigger("Receive");
        goldTxtAnimTxt.text = "+" + goldToAdd;

        if (ad)
            GetComponent<LevelSelector>().ToggleAdGoldConformation();
    }

    public void SubtractGold(int goldToSub) {
        gold -= goldToSub;
        saveManager.SaveIntData("Gold", gold);

        goldTxt.text = gold.ToString();
        goldAnim.SetTrigger("Buy");
        goldTxtAnim.SetTrigger("Buy");
        goldTxtAnimTxt.text = "-" + goldToSub;
    }
}
