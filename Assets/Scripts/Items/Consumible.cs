using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumible", menuName = "Items/Consumible")]
public class Consumible : Item
{
    public override ItemType GetItemType() { return ItemType.Consumible; }
}
