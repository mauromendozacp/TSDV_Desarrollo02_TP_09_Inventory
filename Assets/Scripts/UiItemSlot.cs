using System;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiItemSlot : MonoBehaviour
{
    public enum PlayerList { Inventory, Outfit, Arms, None }

    [SerializeField] private UiInventory inv;
    [SerializeField] private PlayerList playerList = PlayerList.Inventory;
    [SerializeField] private int indexList;
    [SerializeField] private int id;

    public int GetID() => id;
    public int GetIndex() => indexList;
    public PlayerList GetPlayerList() => playerList;

    void Start()
    {
        inv.RefreshAllButtons += RefreshButton;
    }

    public void SetButton(int indexList, int id)
    {
        if (playerList == PlayerList.None)
        {
            id = -1;
            return;
        }
        this.indexList = indexList;
        this.id = id;

        if (id < 0)
        {
            transform.GetChild(0).GetComponent<Image>().sprite = inv.prefaButtonSlot.transform.GetChild(0).GetComponent<Image>().sprite;
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            Sprite sprite = GameplayManager.GetInstance().GetItemFromID(id).icon;
            transform.GetChild(0).GetComponent<Image>().sprite = sprite;

            if (GameplayManager.GetInstance().GetItemFromID(id).maxStack > 1)
            {
                gameObject.transform.GetChild(1).gameObject.SetActive(true);
                switch (playerList)
                {
                    case UiItemSlot.PlayerList.Arms:
                    case UiItemSlot.PlayerList.Outfit:
                        gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = inv.equipment.GetSlot(indexList).amount.ToString();
                        break;
                    case UiItemSlot.PlayerList.Inventory:
                        gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = inv.inventory.GetSlot(indexList).amount.ToString();
                        break;
                }
            }
            else
            {
                gameObject.transform.GetChild(1).gameObject.SetActive(false);
            }
        }

        Player.OnRefreshMeshAsStatic();
    }

    void Refresh(PlayerList playerlist)
    {
        switch (playerlist)
        {
            case PlayerList.Arms:
            case PlayerList.Outfit:
                id = inv.equipment.GetID(indexList);
                break;
            case PlayerList.Inventory:
                id = inv.inventory.GetID(indexList);
                break;
        }

        SetButton(indexList, id);
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

    public void RefreshButton()
    {
        Refresh(playerList);
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
        if (playerList != PlayerList.None)
        {
            inv.toolTip.gameObject.SetActive(true);
            inv.MouseEnterOver(btn);
        }
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