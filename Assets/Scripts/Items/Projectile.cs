using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Items/Arms/Projectile")]
public class Projectile : Arms
{
    [Header("Projectile Specific")]
    public WeaponType weaponToUse;
}
