using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour {
    public SaveManager saveManager;
    public ItemDb itemDb;
    public GoldManager goldManager;

    public Item selectedItem;
    public Button selectedCatagoryBtn;

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

    private void Awake() {
        // Set data for first time load
        if (!PlayerPrefs.HasKey("PlayerCar0Unlocked")) {
            PlayerPrefs.SetInt("PlayerCar0Unlocked", 1);
            PlayerPrefs.SetInt("PlayerCarEquipped", 0);
            
            PlayerPrefs.SetInt("2LongCar0Unlocked", 1);
            PlayerPrefs.SetInt("2LongCarEquipped", 0);

            PlayerPrefs.SetInt("3LongCar0Unlocked", 1);
            PlayerPrefs.SetInt("3LongCarEquipped", 0);
        }
    }

    private void Start() {
        saveManager = SaveManager.Instance;
        itemDb = ItemDb.Instance;
        goldManager = GoldManager.Instance;
        selectedCatagoryBtn.interactable = false;
        selectedItem = itemDb.GetItem("PlayerCar", 0);

        UpdateShopUI();
    }

    public void UpdateShopUI() {
        /* Updates name, img and display ui/btns for cars in shop */
        itemNameTxt.text = selectedItem.name;
        itemImg.sprite = selectedItem.sprite;

        indexTxt.text = (selectedItem.ID + 1) + "/" + itemDb.GetLengthOfCat(selectedItem.catagory);

        if (PlayerPrefs.GetInt(selectedItem.catagory + selectedItem.ID + "Unlocked", 0) == 1) {
            ShowEquip();
        } else if (selectedItem.needBuy) {
            ShowStandard(true);
        } else {
            ShowStandard(false);
        }

        UpdateNavButtons();
    }

    public void UpdateNavButtons() {
        // Display button based on what item looking at
        if (selectedItem.ID == 0 && selectedItem.ID == itemDb.GetLengthOfCat(selectedItem.catagory) - 1) {
            // First and last item, no prev or next item
            prevBtn.SetActive(false);
            nextBtn.SetActive(false);
        } else if (selectedItem.ID == 0) {
            // First item, no prev item
            prevBtn.SetActive(false);
            nextBtn.SetActive(true);
        } else if (selectedItem.ID == itemDb.GetLengthOfCat(selectedItem.catagory) - 1) {
            // Last item, no next item
            prevBtn.SetActive(true);
            nextBtn.SetActive(false);
        } else {
            // Somewhere in middle, prev and next item
            prevBtn.SetActive(true);
            nextBtn.SetActive(true);
        }
    }

    public void SelectCatagory(string cat) {
        // Switches catagory and automatically selects first item
        selectedCatagoryBtn.interactable = true;
        selectedCatagoryBtn = GameObject.Find("Select" + cat).GetComponent<Button>();
        selectedCatagoryBtn.interactable = false;
        selectedItem = itemDb.GetItem(cat, 0);
        UpdateShopUI();
        AudioManager.Instance.PlayButtonClick();
    }

    public void ShowNextItem() {
        // Switches to next item (item with 1 higher ID)
        selectedItem = itemDb.GetItem(selectedItem.catagory, selectedItem.ID + 1);
        UpdateShopUI();
        AudioManager.Instance.PlayButtonClick();
    }

    public void ShowPrevItem() {
        // Switches to prev item (item with 1 lower ID)
        selectedItem = itemDb.GetItem(selectedItem.catagory, selectedItem.ID - 1);
        UpdateShopUI();
        AudioManager.Instance.PlayButtonClick();
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
        if (PlayerPrefs.GetInt(selectedItem.catagory + "Equipped") == selectedItem.ID) {
            equipBtn.interactable = false;
            equipTxt.text = "Equipped";
        } else {
            equipBtn.interactable = true;
            equipTxt.text = "Equip";
        }
    }

    public void BuyItem() {
        // Change from shop ui to equip ui and save that player has unlocked the item
        saveManager.SaveIntData(selectedItem.catagory + selectedItem.ID + "Unlocked", 1);
        saveManager.IncreaseAchivementProgress(2);
        goldManager.SubtractGold(selectedItem.cost);
        ShowEquip();
        AudioManager.Instance.PlayBuySound();
    }

    public void EquipItem() {
        // Change equippedCar id 
        PlayerPrefs.SetInt(selectedItem.catagory + "Equipped", selectedItem.ID);
        UpdateShopUI();
        AudioManager.Instance.PlayButtonClick();
    }
}
