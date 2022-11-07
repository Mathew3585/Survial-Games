using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;

public class Inventory : MonoBehaviour
{

    [Header("Incentory Panel Refences")]
    [SerializeField]
    private List<ItemData> content = new List<ItemData>();
    [SerializeField]
    private GameObject inventoryPanel;
    private StarterAssetsInputs starterAssets;

    [SerializeField]
    private Transform inventorySlotParent;

    const int InventorySize = 24;

    [SerializeField]
    private Transform DropPoint;

    [Header("Action Panel Refences")]
    [SerializeField]
    private GameObject actionPanel;
    [SerializeField]
    private GameObject useItemButton;
    [SerializeField]
    private GameObject equipItemButton;
    [SerializeField]
    private GameObject dropItemButton;
    [SerializeField]
    private GameObject destroyItemButton;
    [SerializeField]
    private Sprite TranslucentTextrure;


    [Header("Equipement Panel Refences")]
    [SerializeField]
    private EquipementLibrary equipementLibrary;


    [SerializeField]
    private Image headSlotImage;
    [SerializeField]
    private Image chestSlotImage;
    [SerializeField]
    private Image weaponSlotImage;
    [SerializeField]
    private Image legsSlotImage;
    [SerializeField]
    private Image BagPackSlotImage;

    //Trace des equipements Actuels
    private ItemData equipedHeadIteam;
    private ItemData equipedChestIteam;
    private ItemData equipedWeaponIteam;
    private ItemData equipedLegsIteam;
    private ItemData equipedBagPackIteam;

    //Ref Button deséquiper
    [SerializeField]
    private Button HeadSlotDesequipButton;
    [SerializeField]
    private Button ChestSlotDesequipButton;
    [SerializeField]
    private Button WeaponSlotDesequipButton;
    [SerializeField]
    private Button LegslotDesequipButton;
    [SerializeField]
    private Button BagPackSlotDesequipButton;


    private bool isOpen = false;
    

    private ItemData itemCurrentlySelected;

    public static Inventory instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()    
    {
        inventoryPanel.SetActive(false);
        starterAssets = GetComponentInParent<StarterAssetsInputs>();
        RefreshContent();
    }

    public void Update()
    {
        //Ouvrire L'inventaire
        if (starterAssets.OpenInventory == true)
        {
            OpenInventory();
        }
        if(starterAssets.OpenInventory == false)
        {

        }
    }

    public void AddItem(ItemData item)
    {
        //Ajouter un item
        content.Add(item);
        RefreshContent();
    }

    void OpenInventory()
    {
        inventoryPanel.SetActive(true);
        isOpen = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseInventory()
    {
        starterAssets.OpenInventory = false;
        inventoryPanel.SetActive(false);
        actionPanel.SetActive(false);
        isOpen = false;
        ToolTipUi.instance.Hide();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void RefreshContent()
    {
        //on vide tous les slots / visuel
        for (int i = 0; i < inventorySlotParent.childCount; i++)
        {
            Slot currentSlot = inventorySlotParent.GetChild(i).GetComponent<Slot>();
            currentSlot.item = null;
            currentSlot.itemVisual.sprite = TranslucentTextrure;
        }


        // On remplie le visuel des slots selon le contenue réel de l'inventaire
        for (int i = 0; i < content.Count; i++)
        {
            Slot currentSlot = inventorySlotParent.GetChild(i).GetComponent<Slot>();
            currentSlot.item = content[i];
            currentSlot.itemVisual.sprite = content[i].visual;
        }

        UpadateEquipementDesequipButtons();
    }

    public bool IsFull()
    {
        //Verfifer si l'inventaire et full
        return InventorySize == content.Count;
    }

    public void OpenActionPanel(ItemData item , Vector3 slotPosition)
    {
        itemCurrentlySelected = item;
        //Verifier si l'objet et null
        if (item == null)
        {
            actionPanel.SetActive(false);
            return;
        }

        switch (item.ItemType)
        {
            //Permet d'afficher/Masquer les buttons en fonctions du type de l'item
            case ItemType.Ressource:
                useItemButton.SetActive(false);
                equipItemButton.SetActive(false);
                break;
            case ItemType.Equipement:
                useItemButton.SetActive(false);
                equipItemButton.SetActive(true);
                break;
            case ItemType.Consuamble:
                useItemButton.SetActive(true);
                equipItemButton.SetActive(false);
                break;
        }
        actionPanel.transform.position =  slotPosition;
        actionPanel.SetActive(true);
    }

    public void CloseActionPanel()
    {
        //Permet de Femer le Action Panel
        actionPanel.SetActive(false);
        itemCurrentlySelected = null;
    }


    public void UseActionButton()
    {
        Debug.Log("Use Item : " + itemCurrentlySelected.name);
        CloseActionPanel();
    }
    public void EquipActionButton()
    {
        Debug.Log("Equip Item : " + itemCurrentlySelected.name);

        EquipementLibraryItem equipementLibraryItem = equipementLibrary.content.Where(elem => elem.itemData == itemCurrentlySelected).First();

        if(equipementLibraryItem != null)
        {
            for (int i = 0; i < equipementLibraryItem.elementToDisable.Length; i++)
            {
                equipementLibraryItem.elementToDisable[i].SetActive(false);
            }

            equipementLibraryItem.itemPrefab.SetActive(true);

            switch (itemCurrentlySelected.equipementType)
            {
                case EquipmentType.Head:
                    headSlotImage.sprite = itemCurrentlySelected.visual;
                    equipedHeadIteam = itemCurrentlySelected;
                    break;
                case EquipmentType.Chest:
                    chestSlotImage.sprite = itemCurrentlySelected.visual;
                    equipedChestIteam = itemCurrentlySelected;
                    break;
                case EquipmentType.Weapon:
                    weaponSlotImage.sprite = itemCurrentlySelected.visual;
                    equipedWeaponIteam = itemCurrentlySelected;
                    starterAssets.WeaponHand = true;
                    break;
                case EquipmentType.Legs:
                    legsSlotImage.sprite = itemCurrentlySelected.visual;
                    equipedLegsIteam = itemCurrentlySelected;
                    break;
                case EquipmentType.BagPack:
                    BagPackSlotImage.sprite = itemCurrentlySelected.visual;
                    equipedBagPackIteam = itemCurrentlySelected;
                    break;
            }

            content.Remove(itemCurrentlySelected);
            RefreshContent();
        }
        else
        {
            Debug.LogError("Equipement : " + itemCurrentlySelected + "non existant dans la librairie des équipement");
        }

        CloseActionPanel();
    }
    public void DropActionButton()
    {
        GameObject instantiatedItem = Instantiate(itemCurrentlySelected.prefabs);
        instantiatedItem.transform.position = DropPoint.position;
        content.Remove(itemCurrentlySelected);
        RefreshContent();
        CloseActionPanel();
    }

    public void DestroyActionButton()
    {
        //Permet de Detruire Des objects dans l'inventaire
        content.Remove(itemCurrentlySelected);
        RefreshContent();
        CloseActionPanel();
    }

    void UpadateEquipementDesequipButtons()
    {
        HeadSlotDesequipButton.onClick.RemoveAllListeners();
        HeadSlotDesequipButton.onClick.AddListener(delegate { DesequipEquimepent(EquipmentType.Head); });
        HeadSlotDesequipButton.gameObject.SetActive(equipedHeadIteam);

        ChestSlotDesequipButton.onClick.RemoveAllListeners();
        ChestSlotDesequipButton.onClick.AddListener(delegate { DesequipEquimepent(EquipmentType.Chest); });
        ChestSlotDesequipButton.gameObject.SetActive(equipedChestIteam);

        WeaponSlotDesequipButton.onClick.RemoveAllListeners();
        WeaponSlotDesequipButton.onClick.AddListener(delegate { DesequipEquimepent(EquipmentType.Weapon); });
        WeaponSlotDesequipButton.gameObject.SetActive(equipedWeaponIteam);

        LegslotDesequipButton.onClick.RemoveAllListeners();
        LegslotDesequipButton.onClick.AddListener(delegate { DesequipEquimepent(EquipmentType.Legs); });
        LegslotDesequipButton.gameObject.SetActive(equipedLegsIteam);

        BagPackSlotDesequipButton.onClick.RemoveAllListeners();
        BagPackSlotDesequipButton.onClick.AddListener(delegate { DesequipEquimepent(EquipmentType.BagPack); });
        BagPackSlotDesequipButton.gameObject.SetActive(equipedBagPackIteam);

    }

    public void DesequipEquimepent(EquipmentType equipmentType)
    {
        if (IsFull())
        {
            Debug.Log("Inventaire Full");
            return;
        }

        ItemData currentItem = null;

        switch (equipmentType)
        {
            case EquipmentType.Head:
                currentItem = equipedHeadIteam;
                equipedHeadIteam = null;
                headSlotImage.sprite = TranslucentTextrure;
                break;
            case EquipmentType.Chest:
                currentItem = equipedChestIteam;
                equipedChestIteam = null;
                chestSlotImage.sprite = TranslucentTextrure;
                break;
            case EquipmentType.Weapon:
                currentItem = equipedWeaponIteam;
                equipedWeaponIteam = null;
                weaponSlotImage.sprite = TranslucentTextrure;
                break;
            case EquipmentType.Legs:
                currentItem = equipedLegsIteam;
                equipedLegsIteam = null;
                legsSlotImage.sprite = TranslucentTextrure;
                break;
            case EquipmentType.BagPack:
                currentItem = equipedBagPackIteam;
                equipedBagPackIteam = null;
                BagPackSlotImage.sprite = TranslucentTextrure;
                break;

        }

        EquipementLibraryItem equipementLibraryItem = equipementLibrary.content.Where(elem => elem.itemData == currentItem).First();

        if (equipementLibraryItem != null)
        {
            for (int i = 0; i < equipementLibraryItem.elementToDisable.Length; i++)
            {
                equipementLibraryItem.elementToDisable[i].SetActive(true);
            }

            equipementLibraryItem.itemPrefab.SetActive(false);

        }

        AddItem(currentItem);
        RefreshContent();
    }
}
