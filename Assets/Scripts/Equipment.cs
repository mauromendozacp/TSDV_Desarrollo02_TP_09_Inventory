using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    enum SlotsOutfit { Helmet, Gloves, Boots, Shoulder, Armor, Size }
    [SerializeField] int weaponSlotsAmount = 4;
    [SerializeField] List<Slot> currentOutfit;
    [SerializeField] List<Slot> currentArms;
    Inventory inventory;

    private void Start()
    {
        inventory = GetComponent<Inventory>();
        for (int i = 0; i < (int)SlotsOutfit.Size; i++)
        {
            Slot newSlot = new Slot();
            currentOutfit.Add(newSlot);
        }
        for (int i = 0; i < weaponSlotsAmount; i++)
        {
            Slot newSlot = new Slot();
            currentArms.Add(newSlot);
        }
    }

    public Slot SwapEquipment(Slot newItemSlot)
    {
        Item itemToSwap = GameplayManager.GetInstance().GetItemFromID(newItemSlot.ID);
        if (itemToSwap.GetItemType() == ItemType.Outfit)
        {
            int index = 0;
            switch (((Outfit)itemToSwap).type)
            {
                case OutfitSlotPosition.Helmet:
                    index = (int)SlotsOutfit.Helmet;
                    break;
                case OutfitSlotPosition.Gloves:
                    index = (int)SlotsOutfit.Gloves;
                    break;
                case OutfitSlotPosition.Boots:
                    index = (int)SlotsOutfit.Boots;
                    break;
                case OutfitSlotPosition.Shoulder:
                    index = (int)SlotsOutfit.Shoulder;
                    break;
                case OutfitSlotPosition.Armor:
                    index = (int)SlotsOutfit.Armor;
                    break;
            }
            Slot temp = currentOutfit[index];
            currentOutfit[index] = newItemSlot;
            return temp;
        }
        else // if(itemToSwap.GetItemType() == ItemType.Arms)
        {
            Slot returnSlot = currentArms[0];
            currentArms[0] = newItemSlot;
            Slot temp = currentArms[currentArms.Count - 1];
            currentArms[currentArms.Count - 1] = currentArms[0];
            currentArms[0] = temp;
            return returnSlot;
        }
    }

    public Slot SwapEquipment(Slot newItemSlot, int slotPos)
    {
        Item itemToSwap = GameplayManager.GetInstance().GetItemFromID(newItemSlot.ID);
        if (itemToSwap.GetItemType() == ItemType.Outfit)
        {
            Item itemInSlot = GameplayManager.GetInstance().GetItemFromID(currentOutfit[slotPos].ID);
            if (((Outfit)itemToSwap).type == ((Outfit)itemInSlot).type)
            {
                Slot temp = currentArms[slotPos];
                currentOutfit[slotPos] = newItemSlot;
                return temp;
            }
        }
        else if (itemToSwap.GetItemType() == ItemType.Arms)
        {
            Item itemInSlot = GameplayManager.GetInstance().GetItemFromID(currentArms[slotPos].ID);
            Slot temp = currentArms[slotPos];
            currentArms[slotPos] = newItemSlot;
            return temp;
        }
        return newItemSlot;
    }
}
