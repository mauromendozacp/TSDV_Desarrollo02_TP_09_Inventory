using UnityEngine;

public enum ItemType { Weapon, Outfit, Consumible };

public abstract class Item : ScriptableObject
{
    [Header("Item General")]
    public string name;
    public Sprite icon;
    public GameObject worldPrefab;
    public int level;
    public float weight;
    public int price;
    public abstract ItemType GetItemType();
}
