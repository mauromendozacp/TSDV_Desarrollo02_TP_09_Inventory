using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiItemSlot : MonoBehaviour
{
    private UiInventory inv;
    private int indexList;
    private int id;
    private int amount;
    private bool used;

    public int GetID() => id;
    public int GetIndex() => indexList;
    public int GetAmount() => amount;
    
    public void SetButton(int indexList, int id, int amount, bool used)
    {
        this.indexList = indexList;
        this.id = id;
        this.amount = amount;
        this.used = used;
    }

    private void Awake()
    {
        inv = FindObjectOfType<UiInventory>();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void MouseDown()
    {
        inv.MouseDown();
    }

    public void MouseDrag()
    {
        inv.MouseDrag();
    }

    public void MouseUp()
    {
        inv.MouseUp();
    }

    public void MouseEnterOver(RectTransform btn)
    {
        Debug.Log("OverEnter");
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