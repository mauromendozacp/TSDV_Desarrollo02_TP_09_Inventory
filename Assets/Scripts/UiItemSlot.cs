﻿using System;
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
    [SerializeField] private int idDefaultSprite;

    public int GetID() => id;
    public int GetIndex() => indexList;
    public PlayerList GetPlayerList() => playerList;

    void Start()
    {
        inv.RefreshAllButtonsEvent += RefreshButton;
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
            if (playerList == PlayerList.Inventory)
                transform.GetChild(0).GetComponent<Image>().sprite = inv.defaultSprites[0];
            else
            {
                transform.GetChild(0).GetComponent<Image>().sprite = inv.defaultSprites[idDefaultSprite];
            }
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

        Player.OnRefreshMeshAsStatic?.Invoke();
    }

    private void Refresh(PlayerList playerlist)
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

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(1))
        {
            if (playerList == PlayerList.Inventory)
            {
                inv.inventory.Divide(indexList);
                inv.RefreshAllButtons();
                Debug.Log("Divide Item Inventory.");
            }
        }
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(1))
        {
            if (playerList == PlayerList.Inventory)
            {
                inv.inventory.DeleteItem(indexList);
                Refresh(playerList);
                Debug.Log("Delete Item Inventory.");
            }
        }
        else if (Input.GetMouseButton(0))
        {
            inv.MouseDown(btn);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            switch (playerList)
            {
                case PlayerList.Inventory:
                    if (inv.inventory.UseItem(indexList))
                    {
                        inv.RefreshAllButtons();
                        inv.RefreshToolTip(btn);
                        Debug.Log("Equip Item or Consumible Used.");
                    }
                    else
                    {
                        Refresh(playerList);
                        if (id < 0)
                        {
                            inv.toolTip.gameObject.SetActive(false);
                        }
                    }
                    break;
                case PlayerList.Outfit:
                case PlayerList.Arms:
                    if (inv.equipment.RemoveEquipment(indexList))
                    {
                        inv.RefreshAllButtons();
                        inv.RefreshToolTip(btn);
                        Debug.Log("Unequip");
                    }
                    break;
                case PlayerList.None:
                default:
                    break;
            }
        }
    }

    public void RefreshButton()
    {
        Refresh(playerList);
    }

    private void RefreshTooltipText()
    {

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

        //Debug.Log("EnterOver");
        if (playerList != PlayerList.None)
        {
            inv.toolTip.gameObject.SetActive(true);
            inv.MouseEnterOver(btn);
        }
    }

    public void MouseExitOver()
    {
        //Debug.Log("OverExit");
        inv.toolTip.gameObject.SetActive(false);
    }

    public void Arrastrando()
    {
        //Debug.Log("Moviendo");
    }
}