using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    enum SlotsOutfit
    {
        Helmet,
        Gloves,
        Boots,
        Shoulder,
        Armor,
        Size
    }

    [SerializeField] int weaponSlotsAmount = 4;
    [SerializeField] List<Slot> currentEquipment;
    Inventory inventory;

    private void Start()
    {
        inventory = GetComponent<Inventory>();
        
        for (int i = 0; i < weaponSlotsAmount; i++)
        {
            Slot newSlot = new Slot();
            currentEquipment.Add(newSlot);
        }

        for (int i = 0; i < (int)SlotsOutfit.Size; i++)
        {
            Slot newSlot = new Slot();
            currentEquipment.Add(newSlot);
        }
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
                    index = (int) SlotsOutfit.Helmet;
                    break;
                case OutfitSlotPosition.Gloves:
                    index = (int) SlotsOutfit.Gloves;
                    break;
                case OutfitSlotPosition.Boots:
                    index = (int) SlotsOutfit.Boots;
                    break;
                case OutfitSlotPosition.Shoulder:
                    index = (int) SlotsOutfit.Shoulder;
                    break;
                case OutfitSlotPosition.Armor:
                    index = (int) SlotsOutfit.Armor;
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
    public bool TrySwapCross(int index1, int index2, bool InvententoryToEquipment)
    {
        if (InvententoryToEquipment)
        {
            Item itemToSwap = GameplayManager.GetInstance().GetItemFromID(inventory.GetSlot(index1).ID);

            switch (itemToSwap.GetItemType())
            {
                case ItemType.Outfit:

                    if ((int)((Outfit) itemToSwap).type == index2)
                    {
                        Slot temp = inventory.GetSlot(index1);
                        inventory.SetSlot(index1, currentEquipment[index2]);    // NOO
                        currentEquipment[index2] = temp;

                        return true;
                    }

                    break;
                case ItemType.Consumible:
                    return false;

                case ItemType.Arms:

                   

                    break;
            }



            /*
             
            Item itemToSwap = GameplayManager.GetInstance().GetItemFromID(inventory.GetSlot(index1).ID);
            
            Item itemInSlot = GameplayManager.GetInstance().GetItemFromID(currentEquipment[index2].ID);
            if (itemToSwap.GetItemType() == ItemType.Outfit && itemInSlot.GetItemType() == ItemType.Outfit) 
            {
                if (((Outfit) itemToSwap).type == ((Outfit) itemInSlot).type)
                {
                    Slot temp = inventory.GetSlot(index1);
                    inventory.SetSlot(index1, currentEquipment[index2]);
                    currentEquipment[index2] = temp;
                    return true;
                }
            }
            else if (itemToSwap.GetItemType() == ItemType.Arms && itemInSlot.GetItemType() == ItemType.Arms)
            {
                Slot temp = inventory.GetSlot(index1);
                inventory.SetSlot(index1, currentEquipment[index2]);
                currentEquipment[index2] = temp;
                return true;
            }            
             
            */
        }
        return false;
    }
}