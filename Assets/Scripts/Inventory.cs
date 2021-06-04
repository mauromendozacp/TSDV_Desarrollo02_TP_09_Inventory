﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Slot
{
    public int ID { get; set; }
    public int amount { get; set; }
    public bool used { get; set; }
    public Slot()
    {
        ID = -1;
        amount = 0;
        used = false;
    }
    public Slot(int ID, int amount)
    {
        this.ID = ID;
        this.amount = amount;
        used = true;
    }
    public void AddAmount(int amount)
    {
        this.amount += amount;
    }
    public void FillSlot(int ID, int amount)
    {
        this.ID = ID;
        this.amount = amount;
        used = true;
    }
    public void EmptySlot()
    {
        ID = -1;
        amount = 0;
        used = false;
    }
    public bool IsEmpty() { return used; }
}

public class Inventory : MonoBehaviour
{
    [SerializeField] List<Slot> CurrentItems;
    [SerializeField] int size = 10;
    Equipment equipmentComponent;

    private void Awake()
    {
        equipmentComponent = GetComponent<Equipment>();
    }

    void Start()
    {
        for (int i = 0; i < size; i++)
        {
            int index = GameplayManager.GetInstance().GetRandomItemID();
            int amount = GameplayManager.GetInstance().GetRandomAmmountOfItem(index);
            Slot newSlot = new Slot(index, amount);
            CurrentItems.Add(newSlot);
        }
    }

    public bool AddNewItem(int ID, int amount, int slotPos)
    {
        if (!CurrentItems[slotPos].IsEmpty())
        {
            CurrentItems[slotPos].FillSlot(ID, amount);
            return true;
        }
        else
        {
            if(ID == CurrentItems[slotPos].ID && GameplayManager.GetInstance().GetItemFromID(ID).maxStack >= CurrentItems[slotPos].amount + amount)
            {
                CurrentItems[slotPos].AddAmount(amount);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public bool AddNewItem(int ID, int amount)
    {
        for (int i = 0; i < size; i++)
        {
            if (!CurrentItems[i].used)
            {
                CurrentItems[i].FillSlot(ID, amount);
                return true;
            }
        }
        return false;
    }

    public void DeleteItem(int slotPos)
    {
        if (CurrentItems[slotPos].used)
        {
            CurrentItems[slotPos].EmptySlot();
        }
    }

    public void SwapItem(int slotPosFrom, int slotPosTo)
    {
        Slot temp = new Slot(CurrentItems[slotPosFrom].ID, CurrentItems[slotPosFrom].amount);
        CurrentItems[slotPosFrom] = CurrentItems[slotPosTo];
        CurrentItems[slotPosTo] = temp;
    }

    public void UseItem(int slotPos)
    {
        if(GameplayManager.GetInstance().GetItemFromID(CurrentItems[slotPos].ID).GetItemType() == ItemType.Consumible)
        {

        }
        else
        {
            CurrentItems[slotPos] = equipmentComponent.SwapEquipment(CurrentItems[slotPos]);
        }
    }

}