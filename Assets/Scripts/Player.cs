using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Equipment equipment;
    Inventory inventory;

    private void Awake()
    {
        equipment = GetComponent<Equipment>();
        inventory = GetComponent<Inventory>();
    }

    private void Start()
    {
        GameplayManager.GetInstance().SetPlayer(this);
    }

    public List<Slot> GetSaveSlots()
    {
        List<Slot> newList = new List<Slot>();

        for (int i = 0; i < equipment.GetEquipmentList().Count; i++)
        {
            newList.Add(equipment.GetEquipmentList()[i]);
        }

        for (int i = 0; i < inventory.GetInventoryList().Count; i++)
        {
            newList.Add(inventory.GetInventoryList()[i]);
        }
        return newList;
    }

}
