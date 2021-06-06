using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiItemSlot : MonoBehaviour
{
    public enum PlayerList { Inventory, Outfit, Arms }

    [SerializeField] private UiInventory inv;
    [SerializeField] private PlayerList playerList = PlayerList.Inventory;
    [SerializeField] private int indexList;
    [SerializeField] private int id;

    public int GetID() => id;
    public int GetIndex() => indexList;
    public PlayerList GetPlayerList() => playerList;
    
    public void SetButton(int indexList, int id)
    {
        this.indexList = indexList;
        this.id = id;
        if (id < 0)
        {
            GetComponent<Image>().sprite = inv.prefaButtonSlot.GetComponent<Image>().sprite;
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            Sprite sprite = GameplayManager.GetInstance().GetItemFromID(id).icon;
            GetComponent<Image>().sprite = sprite;

            if (GameplayManager.GetInstance().GetItemFromID(id).maxStack > 1)
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                switch (playerList)
                {
                    case UiItemSlot.PlayerList.Arms:
                    case UiItemSlot.PlayerList.Outfit:
                        gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = inv.equipment.GetSlot(indexList).amount.ToString();
                        break;
                    case UiItemSlot.PlayerList.Inventory:
                        gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = inv.inventory.GetSlot(indexList).amount.ToString();
                        break;
                }
                
            }
            else
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    private void Awake()
    {
        inv = FindObjectOfType<UiInventory>();
    }

    public void MouseDown(RectTransform btn)
    {
        if (id < 0)
            return;

        inv.MouseDown(btn);
    }

    public void MouseDrag()
    {
        inv.MouseDrag();
    }

    public void MouseUp(RectTransform btn)
    {
        inv.MouseUp(btn);
    }

    public void MouseEnterOver(RectTransform btn)
    {
        if (inv.secondParameter)
        {
            Vector2 mousePos = Input.mousePosition;
            if (inv.mousePos == mousePos)
            {
                inv.slotDrop = btn.GetComponent<UiItemSlot>();
                inv.SwapButtonsIDs();
            }

            inv.secondParameter = false;
        }

        if (id < 0) 
            return;

        Debug.Log("EnterOver");
        inv.toolTip.gameObject.SetActive(true);
        inv.MouseEnterOver(btn);
    }

    public void MouseExitOver()
    {
        Debug.Log("OverExit");
        inv.toolTip.gameObject.SetActive(false);
    }

    public void Arrastrando()
    {
        Debug.Log("Moviendo");
    }
}