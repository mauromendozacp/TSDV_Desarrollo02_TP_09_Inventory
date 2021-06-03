using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OutfitSlotPosition { Helmet, Gloves, Boots, Pants, Armor };
[CreateAssetMenu(fileName = "Outfit", menuName = "Items/Outfit")]
public class Outfit : Item
{
    [Header("Armor Specific")]
    public OutfitSlotPosition type;
    public int defense;

    public override ItemType GetItemType() { return ItemType.Outfit; }
}
