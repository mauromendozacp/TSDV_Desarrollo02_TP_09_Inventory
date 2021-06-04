using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shield", menuName = "Items/Arms/Shield")]
public class Shields : Arms
{
    [Header("Shield Specific")]
    public int resistance;
    public int size;
    public int speedRating;
}
