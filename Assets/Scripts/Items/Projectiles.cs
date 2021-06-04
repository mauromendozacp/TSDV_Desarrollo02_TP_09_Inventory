using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Items/Arms/Projectile")]
public class Projectiles : Arms
{
    [Header("Projectile Specific")]
    public WeaponType weaponToUse;
}
