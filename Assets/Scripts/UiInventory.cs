using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiInventory : MonoBehaviour
{
    public Button prefaButtonSlot;
    public Image toolTip;
    public Inventory inventory;

    public Image slotAux;
    public RectTransform content;
    private GridLayoutGroup gridLayout;

    private void Awake()
    {
        gridLayout = content.GetComponent<GridLayoutGroup>();
    }

    void Start()
    {
        CreateButtonsSlots();
        ResizeContent();
    }

    void CreateButtonsSlots()
    {
        int invSize = inventory.GetSize();
        for (int i = 0; i < invSize; i++)
        {
            Slot slot = inventory.GetSlot(i);
            Button newButton = Instantiate(prefaButtonSlot, content.transform);
            newButton.name = ("Slot" + i);
            newButton.GetComponent<UiItemSlot>().SetButton(i, slot.ID, slot.amount, slot.used);
        }
    }

    void ResizeContent()
    {
        int cantChild = content.transform.childCount;

        float cellSize = gridLayout.cellSize.y;
        cellSize += gridLayout.spacing.y;
        int columns = gridLayout.constraintCount;

        int currentColumn = 0;
        while (cantChild % columns != 0)
        {
            cantChild++;
            currentColumn++;
            if (currentColumn > columns)
            {
                Debug.LogError("Supera el Maximo de Columnas ", gameObject);
                break; // Salida de de emergencia de While
            }
        }

        content.sizeDelta = new Vector2(content.sizeDelta.x, cantChild * cellSize / columns);
    }

    void Update()
    {
        ResizeContent();
    }

    public void MouseDown()
    {
        Debug.Log("Down.");
        slotAux.transform.position = Input.mousePosition;
        slotAux.gameObject.SetActive(true);
        toolTip.gameObject.SetActive(false);
    }

    public void MouseEnterOver(RectTransform btn)
    {
        toolTip.transform.position = new Vector3(btn.transform.position.x, btn.transform.position.y, btn.transform.position.z);


        int id = btn.GetComponent<UiItemSlot>().GetID();
        
        Debug.Log("ID over: " + id);

        string text = TextFormatter(btn.GetComponent<UiItemSlot>(), id);
        toolTip.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = text;
    }

    string TextFormatter(UiItemSlot slot, int idItem)
    {
        int index = slot.GetIndex();
        Item myItem = GameplayManager.GetInstance().GetItemFromID(idItem);
        string name = myItem.itemName;
        string type = "-";
        float weight = myItem.weight;
        int amount = slot.GetAmount();
        int price = myItem.price;


        switch (myItem.GetItemType())
        {
            case ItemType.Arms:
                type = "Arms";
                /*switch (((WeaponType)myItem).type)
                {
                    case Arms.WeaponType.Bow:
                        break;
                }*/

                break;
            case ItemType.Consumible:
                type = "Consumible";

                break;
            case ItemType.Outfit:

                switch (((Outfit) myItem).type)
                {
                    case OutfitSlotPosition.Armor:
                        type = "Armor";
                        break;
                    case OutfitSlotPosition.Boots:
                        type = "Boots";
                        break;
                    case OutfitSlotPosition.Gloves:
                        type = "Gloves";
                        break;
                    case OutfitSlotPosition.Helmet:
                        type = "Helmet";
                        break;
                    case OutfitSlotPosition.Shoulder:
                        type = "Shoulder";
                        break;
                }
                break;
        }

        string text = "Name: " + name + "\nType: " + type + "\nWeight: " + weight + "\nAmount: " + amount + "\nPrice: " + price;
        return text;
    }

    public void MouseUp()
    {
        Debug.Log("Up.");
        slotAux.transform.position = Input.mousePosition;
        slotAux.gameObject.SetActive(false);
    }

    public void MouseDrag()
    {
        Debug.Log("Arrastrando.");
        slotAux.transform.position = Input.mousePosition;
    }
}