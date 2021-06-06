using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiCheatModule : MonoBehaviour
{
    public UiInventory uiInventory;

    public void AddNewItem()
    {
        int randomID = GameplayManager.GetInstance().GetRandomItemID();
        int randomAmount = GameplayManager.GetInstance().GetRandomAmmountOfItem(randomID);
        if (uiInventory.inventory.AddNewItem(randomID, randomAmount))
            uiInventory.RefreshAllButtons?.Invoke();
    }
}