using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArmsType { Weapon, Shield, Proyectile }

public abstract class Arms : Item
{
    public override ItemType GetItemType() { return ItemType.Arms; }

    public abstract ArmsType GetArmsType();

    public override string ToString() { return ""; }
}