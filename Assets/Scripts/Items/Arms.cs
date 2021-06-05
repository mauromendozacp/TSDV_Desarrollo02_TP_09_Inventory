using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arms : Item
{
    public enum WeaponType { Sword, Dagger, Bow, Spear, Trident, Crossbow }
    public override ItemType GetItemType() { return ItemType.Arms; }
}