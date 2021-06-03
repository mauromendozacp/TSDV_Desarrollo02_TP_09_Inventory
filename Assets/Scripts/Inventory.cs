using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
class Slot
{
    public int id;
    public int amount;
    public bool used;
    public Slot()
    {
        id = -1;
        amount = 0;
        used = false;
    }
    public Slot(int id, int amount)
    {
        this.id = id;
        this.amount = amount;
        used = true;
    }
}

public class Inventory : MonoBehaviour
{
    [SerializeField] List<Slot> CurrentItems;
    [SerializeField] int size = 10;

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

    bool AddNewItem(int id, int amount, int slotPos)
    {
        if (!CurrentItems[slotPos].used)
        {
            CurrentItems[slotPos].amount = amount;
            CurrentItems[slotPos].id = id;
            CurrentItems[slotPos].used = true;
            return true;
        }
        else
        {
            if(id == CurrentItems[slotPos].id && GameplayManager.GetInstance().GetItemFromID(id).maxStack >= CurrentItems[slotPos].amount + amount)
            {
                CurrentItems[slotPos].amount += amount;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public void DeleteItem(int slotPos)
    {
        if (CurrentItems[slotPos].used)
        {
            CurrentItems[slotPos].amount = 0;
            CurrentItems[slotPos].id = -1;
            CurrentItems[slotPos].used = false;
        }
    }

    public void SwapItem(int slotPosFrom, int slotPosTo)
    {
        Slot temp = new Slot(CurrentItems[slotPosFrom].id, CurrentItems[slotPosFrom].amount);
        CurrentItems[slotPosFrom] = CurrentItems[slotPosTo];
        CurrentItems[slotPosTo] = temp;
    }

}
