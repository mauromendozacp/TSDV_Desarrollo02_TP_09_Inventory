using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{

    [SerializeField] AllItems allItems;

    static private GameplayManager instance;

    static public GameplayManager GetInstance() { return instance; }

    private void Awake()
    {
        if(!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetRandomItemID()
    {
        return Random.Range(0, allItems.itemList.Count);
    }

    public int GetRandomAmmountOfItem(int id)
    {
        return Random.Range(1, allItems.itemList[id].maxStack);
    }

    public Item GetItemFromID(int id)
    {
        return allItems.itemList[id];
    }

}
