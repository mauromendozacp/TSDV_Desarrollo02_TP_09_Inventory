using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Items/Weapon")]
public class Weapon : Item
{
    public enum WeaponType { Sword, Dagger, Bow, Spear, Trident, Crossbow }
    [Header("Weapon Specific")]
    public WeaponType type;
    public bool twoHanded;
    public int damage;
    public int speed;
    public override ItemType GetItemType() { return ItemType.Weapon; }
}
