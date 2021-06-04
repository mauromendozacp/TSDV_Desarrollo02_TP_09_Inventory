using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    enum SlotsOutfit { Helmet, Gloves, Boots, Pants, Armor, Size }
    [SerializeField] int weaponSlotsAmount = 4;
    List<Slot> OutfitList;
    List<Slot> ArmsList;
    Inventory inventory;

    private void Start()
    {
        inventory = GetComponent<Inventory>();
        for (int i = 0; i < (int)SlotsOutfit.Size; i++)
        {
            Slot newSlot = new Slot();
            OutfitList.Add(newSlot);
        }
        for (int i = 0; i < weaponSlotsAmount; i++)
        {
            Slot newSlot = new Slot();
            ArmsList.Add(newSlot);
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
                    index = (int)SlotsOutfit.Helmet;
                    break;
                case OutfitSlotPosition.Gloves:
                    index = (int)SlotsOutfit.Gloves;
                    break;
                case OutfitSlotPosition.Boots:
                    index = (int)SlotsOutfit.Boots;
                    break;
                case OutfitSlotPosition.Pants:
                    index = (int)SlotsOutfit.Pants;
                    break;
                case OutfitSlotPosition.Armor:
                    index = (int)SlotsOutfit.Armor;
                    break;
            }
            Slot temp = OutfitList[index];
            OutfitList[index] = slotToChange;
            return temp; 
        }
        else // if(itemToSwap.GetItemType() == ItemType.weapon)
        {
            Slot returnSlot = ArmsList[0];
            ArmsList[0] = slotToChange;
            Slot temp = ArmsList[ArmsList.Count - 1];
            ArmsList[ArmsList.Count -1] = ArmsList[0];
            ArmsList[0] = temp;
            return returnSlot;
        }
    }

}
