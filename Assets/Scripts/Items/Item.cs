using UnityEngine;

public enum ItemType { Arms, Outfit, Consumible };

public abstract class Item : ScriptableObject
{
    [Header("Item General")]
    public string itemName;
    public Sprite icon;
    public GameObject worldPrefab;
    public int maxStack;
    public int level;
    public float weight;
    public int price;
    public abstract ItemType GetItemType();
}
