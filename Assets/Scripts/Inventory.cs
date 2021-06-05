using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Slot
{
    public int ID;
    public int amount;
    public bool used;
    public Item test;
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
    public int AddAmount(int amount)
    {
        this.amount += amount;
        int maxAmount = GameplayManager.GetInstance().GetItemFromID(ID).maxStack;
        if (amount > maxAmount)
        {
            int difference = amount - maxAmount;
            this.amount = maxAmount;
            return difference;
        }
        else if (amount <= 0)
        {
            EmptySlot();
        }
        return 0;
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

    int SortSlotsByName(string str1, string str2)
    {
        return str1.CompareTo(str2);
    }
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
            newSlot.test = GameplayManager.GetInstance().GetItemFromID(index);
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
            CurrentItems[slotPos].AddAmount(-1);
        }
        else
        {
            CurrentItems[slotPos] = equipmentComponent.SwapEquipment(CurrentItems[slotPos]);
        }
    }

    public void Divide(int slotPos)
    {
        if(CurrentItems[slotPos].amount > 1)
        {
            int dividedAmount = (CurrentItems[slotPos].amount / 2) + 1;
            if (AddNewItem(CurrentItems[slotPos].ID, dividedAmount))
            {
                CurrentItems[slotPos].amount /= 2;
            }
        }
        else
        {
            Debug.Log("No puedo dividir");
        }
    }

    public enum SortType { Type, Name, Level }

    public void Sort(int type)
    {
        switch ((SortType)type)
        {
            case SortType.Type:
                SortByType sortType = new SortByType();
                CurrentItems.Sort(sortType);
                break;
            case SortType.Name:
                SortByName sortName = new SortByName();
                CurrentItems.Sort(sortName);
                break;
            case SortType.Level:
                SortByLevel sortLevel = new SortByLevel();
                CurrentItems.Sort(sortLevel);
                break;
            default:
                Debug.Log("Wrong Sort int from button, can't Sort.");
                break;
        }
    }

    public int GetSize()
    {
        return size;
    }
    public Slot GetSlot(int index)
    {
        return CurrentItems[index];
    }

    class SortByName : IComparer<Slot>
    {
        public int Compare(Slot x, Slot y)
        {
            return GameplayManager.GetInstance().GetItemFromID(x.ID).name.CompareTo(GameplayManager.GetInstance().GetItemFromID(y.ID).name);
        }
    }
    class SortByLevel : IComparer<Slot>
    {
        public int Compare(Slot x, Slot y)
        {
            return GameplayManager.GetInstance().GetItemFromID(x.ID).level.CompareTo(GameplayManager.GetInstance().GetItemFromID(y.ID).level);
        }
    }
    class SortByType : IComparer<Slot>
    {
        public int Compare(Slot x, Slot y)
        {
            return GameplayManager.GetInstance().GetItemFromID(x.ID).GetItemType().CompareTo(GameplayManager.GetInstance().GetItemFromID(y.ID).GetItemType());
        }
    }
}
