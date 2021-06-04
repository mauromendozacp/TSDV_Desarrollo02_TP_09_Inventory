using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    enum SlotsEquipment { LeftWeapon, RightWeapon, Helmet, Gloves, Boots, Pants, Armor, Size }
    List<Slot> EquipmentList;
    Inventory inventory;

    private void Start()
    {
        inventory = GetComponent<Inventory>();
        for (int i = 0; i < (int)SlotsEquipment.Size; i++)
        {
            Slot newSlot = new Slot();
            EquipmentList.Add(newSlot);
        }
    }

    public Slot SwapEquipment(Slot slotToChange)
    {
        Item itemToSwap = GameplayManager.GetInstance().GetItemFromID(slotToChange.ID);
        if (itemToSwap.GetItemType() == ItemType.Outfit)
        {
            int index = 0;
            switch (((Outfit)itemToSwap).type)
            {
                case OutfitSlotPosition.Helmet:
                    index = (int)SlotsEquipment.Helmet;
                    break;
                case OutfitSlotPosition.Gloves:
                    index = (int)SlotsEquipment.Gloves;
                    break;
                case OutfitSlotPosition.Boots:
                    index = (int)SlotsEquipment.Boots;
                    break;
                case OutfitSlotPosition.Pants:
                    index = (int)SlotsEquipment.Pants;
                    break;
                case OutfitSlotPosition.Armor:
                    index = (int)SlotsEquipment.Armor;
                    break;
            }
            Slot temp = EquipmentList[index];
            EquipmentList[index] = slotToChange;
            return temp; 
        }
        else // if(itemToSwap.GetItemType() == ItemType.weapon)
        {
            if (((Weapon)itemToSwap).twoHanded)
            {
                Slot temp;
                if (EquipmentList[(int)SlotsEquipment.LeftWeapon].used && EquipmentList[(int)SlotsEquipment.RightWeapon].used)
                {
                    if (inventory.AddNewItem(EquipmentList[(int)SlotsEquipment.LeftWeapon].ID, 1))
                    {
                        EquipmentList[(int)SlotsEquipment.LeftWeapon] = slotToChange;
                        return EquipmentList[(int)SlotsEquipment.RightWeapon];
                    }
                    else
                    {
                        return slotToChange;
                    }
                }
                else if (EquipmentList[(int)SlotsEquipment.RightWeapon].used)
                {
                    temp = EquipmentList[(int)SlotsEquipment.RightWeapon];
                }
                else
                {
                    temp = EquipmentList[(int)SlotsEquipment.LeftWeapon];
                }
                EquipmentList[(int)SlotsEquipment.RightWeapon].EmptySlot();
                EquipmentList[(int)SlotsEquipment.LeftWeapon] = slotToChange;
                return temp;
            }
            else
            {
                Slot temp = EquipmentList[(int)SlotsEquipment.LeftWeapon];
                EquipmentList[(int)SlotsEquipment.LeftWeapon] = slotToChange;
                return temp;
            }
        }
    }

}
