using System.Linq;
using TMPro;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class UiInventory : MonoBehaviour
{
    public Button prefaButtonSlot;
    public Image toolTip;
    public Inventory inventory;
    public Equipment equipment;

    public Image slotAux;
    public RectTransform content;
    private GridLayoutGroup gridLayout;

    public bool secondParameter;

    private void Awake()
    {
        gridLayout = content.GetComponent<GridLayoutGroup>();
    }

    void Start()    //   Carrera de start con Inventory
    {
        Invoke("IniciarInventarioUI", 1);       // ver como iniciar despues de la lógica de inventario.
    }

    void IniciarInventarioUI()
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
            
            newButton.GetComponent<UiItemSlot>().SetButton(i, slot.ID);
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

    public void MouseDown(RectTransform btn)
    {
        Debug.Log("Down.");
        slotAux.transform.position = Input.mousePosition;
        slotAux.GetComponent<Image>().sprite = btn.GetComponent<Image>().sprite;
        slotAux.gameObject.SetActive(true);
        toolTip.gameObject.SetActive(false);
    }

    public void MouseEnterOver(RectTransform btn)
    {
        UiItemSlot uiItem = btn.GetComponent<UiItemSlot>();

        toolTip.transform.position = new Vector3(btn.transform.position.x, btn.transform.position.y, btn.transform.position.z);
        int id = uiItem.GetID();
        
        Debug.Log("ID over: " + id);

        string text = TextFormatter(uiItem, id, uiItem.GetPlayerList());
        TextMeshProUGUI textMesh = toolTip.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        textMesh.text = text;

        int lines = 0;
        int chars = 0;
        int maxChar = 0;
        float offset = 51;
        float margin = 10;

        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '\n')
            {
                lines++;
                if (maxChar < chars)
                    maxChar = chars;
                chars = 0;
            }
            else
            {
                chars++;
            }
        }
        toolTip.rectTransform.sizeDelta = new Vector2(toolTip.rectTransform.sizeDelta.x, lines * offset + margin);

        Debug.Log("Count del For: " + lines);
    }

    string TextFormatter(UiItemSlot UiSlot, int idItem, UiItemSlot.PlayerList playerList)
    {
        int index = UiSlot.GetIndex();
        Slot slot;
        switch (playerList)
        {
            case UiItemSlot.PlayerList.Arms:
            case UiItemSlot.PlayerList.Outfit:
                slot = equipment.GetSlot(index);
                break;
            case UiItemSlot.PlayerList.Inventory:
                slot = inventory.GetSlot(index);
                break;
            default:
                slot = inventory.GetSlot(index);
                break;
        }
        
        Item myItem = GameplayManager.GetInstance().GetItemFromID(idItem);

        string text = myItem.ItemToString();
        if (myItem.maxStack > 1)
        {
            text += "\nAmount: " + slot.amount;
        }
        return text;
    }

    private UiItemSlot slotPick;
    public UiItemSlot slotDrop;
    public Vector2 mousePos;

    public void MouseUp(RectTransform btn)
    {
        Debug.Log("Up.", gameObject);
        slotAux.transform.position = Input.mousePosition;
        slotAux.gameObject.SetActive(false);

        mousePos = Input.mousePosition;
        secondParameter = true;
        slotPick = btn.GetComponent<UiItemSlot>();
    }

    public void SwapButtonsIDs()
    {
        if ((slotPick.GetPlayerList() == UiItemSlot.PlayerList.Inventory) && (slotDrop.GetPlayerList() == UiItemSlot.PlayerList.Inventory))
        {
            inventory.SwapItem(slotPick.GetIndex(), slotDrop.GetIndex());

            int slotid1 = slotPick.GetID();
            slotPick.SetButton(slotPick.GetIndex(), slotDrop.GetID());
            slotDrop.SetButton(slotDrop.GetIndex(), slotid1);
        }
        else if ((slotPick.GetPlayerList() == UiItemSlot.PlayerList.Inventory && slotDrop.GetPlayerList() != UiItemSlot.PlayerList.Inventory)|| slotPick.GetPlayerList() != UiItemSlot.PlayerList.Inventory && slotDrop.GetPlayerList() == UiItemSlot.PlayerList.Inventory)
        {
            if (equipment.TrySwapCross(slotPick.GetIndex(), slotDrop.GetIndex(), slotPick.GetPlayerList() == UiItemSlot.PlayerList.Inventory))
            {
                int slotid1 = slotPick.GetID();
                slotPick.SetButton(slotPick.GetIndex(), slotDrop.GetID());
                slotDrop.SetButton(slotDrop.GetIndex(), slotid1);
            }
        }
        else if(equipment.SwapItem(slotPick.GetIndex(), slotDrop.GetIndex()))
        {
            int slotid1 = slotPick.GetID();
            slotPick.SetButton(slotPick.GetIndex(), slotDrop.GetID());
            slotDrop.SetButton(slotDrop.GetIndex(), slotid1);
        }
    }

    public void MouseDrag()
    {
        Debug.Log("Arrastrando.");
        slotAux.transform.position = Input.mousePosition;
    }
}