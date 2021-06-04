using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Items/Arms/Weapon")]
public class Weapon : Arms
{
    [Header("Weapon Specific")]
    public WeaponType type;
    public bool twoHanded;
    public int damage;
    public int speed;
    public override ItemType GetItemType() { return ItemType.Arms; }
}
