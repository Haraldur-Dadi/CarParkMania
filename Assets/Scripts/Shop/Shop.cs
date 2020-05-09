using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour {
    public SaveManager saveManager;
    public ItemDb itemDb;
    public GoldManager goldManager;

    public CanvasGroup canvas;
    public Item selectedItem;

    public TextMeshProUGUI itemNameTxt;
    public Image itemImg;

    public TextMeshProUGUI indexTxt;
    public bool canShowPrev;
    public bool canShowNext;
    public GameObject prevBtn;
    public GameObject nextBtn;

    public Button buyBtn;
    public TextMeshProUGUI buyAmountTxt;
    public Button equipBtn;
    public TextMeshProUGUI equipTxt;
    public GameObject unlock;

    private void Awake() {
        // Set data for first time load
        if (!PlayerPrefs.HasKey("PlayerCar0Unlocked")) {
            PlayerPrefs.SetInt("PlayerCar0Unlocked", 1);
            PlayerPrefs.SetInt("PlayerCarEquipped", 0);
        }
    }

    private void Start() {
        saveManager = SaveManager.Instance;
        itemDb = ItemDb.Instance;
        goldManager = GoldManager.Instance;
        ShowFirst();
    }

    public void ShowFirst() {
        selectedItem = itemDb.GetItem(0);
        UpdateShopUI(false);
    }

    public void UpdateShopUI(bool fadeOut) {
        /* Updates name, img and display ui/btns for cars in shop */
        StartCoroutine(UpdateShop(fadeOut));
    }

    public void UpdateNavButtons() {
        // Display button based on what item looking at
        if (selectedItem.ID == 0) {
            // First item, no prev item
            prevBtn.SetActive(false);
            nextBtn.SetActive(true);
            canShowPrev = false;
            canShowNext = true;
        } else if (selectedItem.ID == itemDb.GetLengthOfCat() - 1) {
            // Last item, no next item
            prevBtn.SetActive(true);
            nextBtn.SetActive(false);
            canShowPrev = true;
            canShowNext = false;
        } else {
            // Somewhere in middle, prev and next item
            prevBtn.SetActive(true);
            nextBtn.SetActive(true);
            canShowPrev = true;
            canShowNext = true;
        }
    }

    public void ShowNextItem(bool fadeOut) {
        // Switches to next item (item with 1 higher ID)
        selectedItem = itemDb.GetItem(selectedItem.ID + 1);
        UpdateShopUI(fadeOut);
    }

    public void ShowPrevItem(bool fadeOut) {
        // Switches to prev item (item with 1 lower ID)
        selectedItem = itemDb.GetItem(selectedItem.ID - 1);
        UpdateShopUI(fadeOut);
    }

    public void ShowStandard(bool buyItem) {
        if (buyItem) {
            buyBtn.gameObject.SetActive(true);
            if (goldManager.CanBuy(selectedItem.cost)) {
                buyBtn.interactable = true;
            } else {
                buyBtn.interactable = false;
            }
            buyAmountTxt.text = selectedItem.cost.ToString();
            unlock.SetActive(false);
        } else {
            buyBtn.gameObject.SetActive(false);
            unlock.SetActive(true);
        }
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
        saveManager.SaveIntData("PlayerCar" + selectedItem.ID + "Unlocked", 1);
        saveManager.IncreaseAchivementProgress(2);
        goldManager.SubtractGold(selectedItem.cost);
        ShowEquip();
        AudioManager.Instance.PlayBuySound();
    }

    public void EquipItem() {
        // Change equippedCar id 
        saveManager.SaveIntData("PlayerCar" + "Equipped", selectedItem.ID);
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

        indexTxt.text = (selectedItem.ID + 1) + "/" + itemDb.GetLengthOfCat();

        if (PlayerPrefs.GetInt("PlayerCar" + selectedItem.ID + "Unlocked", 0) == 1) {
            ShowEquip();
        } else if (selectedItem.needBuy) {
            ShowStandard(true);
        } else {
            ShowStandard(false);
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
