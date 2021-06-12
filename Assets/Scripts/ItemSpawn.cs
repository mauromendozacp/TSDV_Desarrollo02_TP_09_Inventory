using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    GameObject enemy;
    [SerializeField] GameObject itemPrefab;

    void Start()
    {
        enemy = this.gameObject;
        GenerateNewItem();
    }

    //void OnDestroy()
    //{
    //    GenerateNewItem();
    //}

    public void GenerateNewItem()
    {
        int randomID = GameplayManager.GetInstance().GetRandomItemID();
        int randomAmount = GameplayManager.GetInstance().GetRandomAmmountOfItem(randomID);
        GameplayManager.GetInstance().GenerateItemInWorldSpace(randomID, randomAmount, itemPrefab, enemy.transform.position);
    }
}
