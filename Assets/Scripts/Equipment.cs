using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    /*enum SlotsOutfit
    {
        Helmet,
        Gloves,
        Boots,
        Shoulder,
        Armor,
        Size
    }*/

    [SerializeField] int weaponSlotsAmount = 4;
    [SerializeField] List<Slot> currentEquipment = null;
    Inventory inventory;

    private void Start()
    {
        inventory = GetComponent<Inventory>();
        
        for (int i = 0; i < weaponSlotsAmount; i++)
        {
            Slot newSlot = new Slot();
            currentEquipment.Add(newSlot);
        }

        for (int i = 0; i <= (int) OutfitSlotPosition.Armor; i++)
        {
            Slot newSlot = new Slot();
            currentEquipment.Add(newSlot);
        }
    }

    public void SetNewEquipment(List<Slot> newEquipment)
    {
        currentEquipment.Clear();
        foreach  (Slot slot in newEquipment)
        {
            currentEquipment.Add(slot);
        }
    }

    public int GetEquipmentAmount()
    {
        return weaponSlotsAmount + (int)OutfitSlotPosition.Armor + 1;
    }

    // Rechequear Funcion
    public Slot SwapEquipment(Slot newItemSlot)
    {
        Item itemToSwap = GameplayManager.GetInstance().GetItemFromID(newItemSlot.ID);
        if (itemToSwap.GetItemType() == ItemType.Outfit)
        {
            int index = 0;
            switch (((Outfit) itemToSwap).type)
            {
                case OutfitSlotPosition.Helmet:
                    index = (int) OutfitSlotPosition.Helmet;
                    break;
                case OutfitSlotPosition.Gloves:
                    index = (int) OutfitSlotPosition.Gloves;
                    break;
                case OutfitSlotPosition.Boots:
                    index = (int) OutfitSlotPosition.Boots;
                    break;
                case OutfitSlotPosition.Shoulder:
                    index = (int) OutfitSlotPosition.Shoulder;
                    break;
                case OutfitSlotPosition.Armor:
                    index = (int) OutfitSlotPosition.Armor;
                    break;
            }

            Slot temp = currentEquipment[index];
            currentEquipment[index] = newItemSlot;
            return temp;
        }
        else // if(itemToSwap.GetItemType() == ItemType.Arms)
        {
            Slot returnSlot = currentEquipment[0];
            currentEquipment[0] = newItemSlot;
            Slot temp = currentEquipment[currentEquipment.Count - 1];
            currentEquipment[currentEquipment.Count - 1] = currentEquipment[0];
            currentEquipment[0] = temp;
            return returnSlot;
        }
    }
    // Rechequear Funcion
    public Slot SwapEquipment(Slot newItemSlot, int slotPos) // Arrastrar a un lugar
    {
        Item itemToSwap = GameplayManager.GetInstance().GetItemFromID(newItemSlot.ID);
        if (itemToSwap.GetItemType() == ItemType.Outfit)
        {
            Item itemInSlot = GameplayManager.GetInstance().GetItemFromID(currentEquipment[slotPos].ID);
            if (((Outfit) itemToSwap).type == ((Outfit) itemInSlot).type)
            {
                Slot temp = currentEquipment[slotPos];
                currentEquipment[slotPos] = newItemSlot;
                return temp;
            }
        }
        else if (itemToSwap.GetItemType() == ItemType.Arms)
        {
            Item itemInSlot = GameplayManager.GetInstance().GetItemFromID(currentEquipment[slotPos].ID);
            Slot temp = currentEquipment[slotPos];
            currentEquipment[slotPos] = newItemSlot;
            return temp;
        }

        return newItemSlot;
    }
    

    // Inventario - Equipment
    // Index1 es el indice en la lista de inventario     
    // Index2 es el indice en la lista de equipment      
    public bool SwapItem(int index1, int index2)
    {
        if (index1 < weaponSlotsAmount && index2 < weaponSlotsAmount)   // Ambas son armas
        {
            if (!currentEquipment[index1].IsEmpty() && !currentEquipment[index2].IsEmpty())
            {
                Item fromItem = GameplayManager.GetInstance().GetItemFromID(currentEquipment[index1].ID);
                Item toItem = GameplayManager.GetInstance().GetItemFromID(currentEquipment[index2].ID);
                if (fromItem.GetItemType() == toItem.GetItemType() && toItem.maxStack > 1)
                {
                    currentEquipment[index1].amount = currentEquipment[index2].AddAmount(currentEquipment[index2].amount);
                    if (currentEquipment[index1].amount <= 0)
                    {
                        currentEquipment[index1].EmptySlot();
                    }
                    return true;
                }
            }
            Slot temp = currentEquipment[index1];
            currentEquipment[index1] =  currentEquipment[index2];
            currentEquipment[index2] = temp;
            return true;
        }
        return false;
    }
    
    public bool TrySwapCross(int index1, int index2, bool InvententoryToEquipment)
    {
        Item itemToSwap;
        int indexInventory = 0;
        int indexOutfit = 0;
        
        if (InvententoryToEquipment)
        {
            indexInventory = index1;
            indexOutfit = index2;
            itemToSwap = GameplayManager.GetInstance().GetItemFromID(inventory.GetSlot(indexInventory).ID);
            if (!currentEquipment[indexOutfit].IsEmpty())
            {
                if (currentEquipment[indexOutfit].ID == inventory.GetSlot(indexInventory).ID && itemToSwap.maxStack > 1)
                {
                    inventory.GetSlot(indexInventory).amount = currentEquipment[indexOutfit].AddAmount(inventory.GetSlot(indexInventory).amount);
                    if (inventory.GetSlot(indexInventory).amount <= 0)
                    {
                        inventory.GetSlot(indexInventory).EmptySlot();
                    }
                    return true;
                }
            }
        }
        else
        {
            indexInventory = index2;
            indexOutfit = index1;
            itemToSwap = GameplayManager.GetInstance().GetItemFromID(GetSlot(indexOutfit).ID);
            if (!inventory.GetSlot(indexInventory).IsEmpty())
            {
                Item itemSwaped = GameplayManager.GetInstance().GetItemFromID(inventory.GetSlot(indexInventory).ID);
                if (inventory.GetSlot(indexInventory).ID == currentEquipment[indexOutfit].ID && itemToSwap.maxStack > 1)
                {
                    currentEquipment[indexOutfit].amount = inventory.GetSlot(indexInventory).AddAmount(currentEquipment[indexOutfit].amount);
                    if (currentEquipment[indexOutfit].amount <= 0)
                    {
                        currentEquipment[indexOutfit].EmptySlot();
                    }
                    return true;
                }
                if (itemToSwap.GetItemType() != itemSwaped.GetItemType())
                {
                    return false;
                }
                else if(itemToSwap.GetItemType() == ItemType.Outfit)
                {
                    if (((Outfit)itemToSwap).type != ((Outfit)itemSwaped).type)
                    {
                        return false;
                    }
                }
            }
        }

        if (indexOutfit < weaponSlotsAmount) // Se tiró en un arma
        {
            if (itemToSwap.GetItemType() == ItemType.Arms)
            {
                Slot temp = inventory.GetSlot(indexInventory);
                inventory.SetSlot(indexInventory, currentEquipment[indexOutfit]);
                currentEquipment[indexOutfit] = temp;
                return true;
            }
        }
        else if (itemToSwap.GetItemType() == ItemType.Outfit) // Se tiró en un Outfit
        {
            OutfitSlotPosition slotsOutfit = (OutfitSlotPosition) (indexOutfit - weaponSlotsAmount);
            if (((Outfit)itemToSwap).type == slotsOutfit)
            {
                Slot temp = inventory.GetSlot(indexInventory);
                inventory.SetSlot(indexInventory, currentEquipment[indexOutfit]);
                currentEquipment[indexOutfit] = temp;
                return true;
            }
        }

        return false;
    }

    public Slot GetSlot(int index)
    {
        return currentEquipment[index];
    }

    public List<Slot> GetEquipmentList()
    {
        return currentEquipment;
    }

    public int GetID(int index)
    {
        return currentEquipment[index].ID;
    }

}