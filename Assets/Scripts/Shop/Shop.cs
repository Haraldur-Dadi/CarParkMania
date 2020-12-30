using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour {
    public CanvasGroup canvas;
    public Item selectedItem;

    public TextMeshProUGUI itemNameTxt;
    public Image itemImg;

    public TextMeshProUGUI indexTxt;
    public GameObject prevBtn;
    public GameObject nextBtn;

    public Button buyBtn;
    public TextMeshProUGUI buyAmountTxt;
    public Button equipBtn;
    public TextMeshProUGUI equipTxt;
    public GameObject unlock;

    void Start() {
        // Set data for first time load
        if (!PlayerPrefs.HasKey("PlayerCar0Unlocked")) {
            PlayerPrefs.SetInt("PlayerCar0Unlocked", 1);
            PlayerPrefs.SetInt("PlayerCarEquipped", 0);
        }
        ShowFirst();
    }

    public void ShowFirst() {
        selectedItem = ItemDb.Instance.GetItem(0);
        UpdateShopUI(false);
    }

    public void UpdateShopUI(bool fadeOut) {
        /* Updates name, img and display ui/btns for cars in shop */
        StartCoroutine(UpdateShop(fadeOut));
    }

    public void UpdateNavButtons() {
        // Display button based on what item looking at
        prevBtn.SetActive(selectedItem.ID > 0);
        nextBtn.SetActive(selectedItem.ID < ItemDb.Instance.GetLengthOfCat() - 1);
    }

    public void ShowNextItem(bool fadeOut) {
        // Switches to next item
        selectedItem = ItemDb.Instance.GetItem(selectedItem.ID + 1);
        UpdateShopUI(fadeOut);
    }

    public void ShowPrevItem(bool fadeOut) {
        // Switches to prev item
        selectedItem = ItemDb.Instance.GetItem(selectedItem.ID - 1);
        UpdateShopUI(fadeOut);
    }

    public void ShowStandard(bool buyItem) {
        buyBtn.gameObject.SetActive(buyItem);
        buyBtn.interactable = GoldManager.Instance.CanBuy(selectedItem.cost);
        buyAmountTxt.text = selectedItem.cost.ToString();
        unlock.SetActive(!buyItem);
        equipBtn.gameObject.SetActive(false);
    }

    public void ShowEquip() {
        buyBtn.gameObject.SetActive(false);
        unlock.SetActive(false);
        equipBtn.gameObject.SetActive(true);
        if (PlayerPrefs.GetInt("PlayerCarEquipped") == selectedItem.ID) {
            equipBtn.interactable = false;
            equipTxt.text = "Equipped";
        } else {
            equipBtn.interactable = true;
            equipTxt.text = "Equip";
        }
    }

    public void BuyItem() {
        // Change from shop ui to equip ui and save that player has unlocked the item
        SaveManager.Instance.SaveIntData("PlayerCar" + selectedItem.ID + "Unlocked", 1);
        SaveManager.Instance.IncreaseAchivementProgress(2);
        GoldManager.Instance.SubtractGold(selectedItem.cost);
        ShowEquip();
        AudioManager.Instance.PlayBuySound();
    }

    public void EquipItem() {
        // Change equippedCar id 
        SaveManager.Instance.SaveIntData("PlayerCar" + "Equipped", selectedItem.ID);
        UpdateShopUI(false);
    }

    public IEnumerator UpdateShop(bool fadeOut) {
        if (fadeOut) {
            float t = 1f;
            while (t > 0f) {
                t -= Time.deltaTime * 2;
                canvas.alpha = t;
                yield return null;
            }
        }
        itemNameTxt.text = selectedItem.name;
        itemImg.sprite = selectedItem.sprite;
        indexTxt.text = (selectedItem.ID + 1) + "/" + ItemDb.Instance.GetLengthOfCat();

        if (PlayerPrefs.GetInt("PlayerCar" + selectedItem.ID + "Unlocked", 0) == 1) {
            ShowEquip();
        } else {
            ShowStandard(selectedItem.needBuy);
        }

        UpdateNavButtons();
        if (fadeOut) {
            float t = 0;
            while (t < 1f) {
                t += Time.deltaTime * 2;
                canvas.alpha = t;
                yield return null;
            }
        }
    }
}