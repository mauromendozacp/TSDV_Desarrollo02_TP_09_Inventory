using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
struct Slot
{
    public int _id;
    public int _amount;
    public Slot(int id, int amount)
    {
        _id = id;
        _amount = amount;
    }
}

public class Inventory : MonoBehaviour
{
    [SerializeField] List<Slot> CurrentItems;
    [SerializeField] int size = 10;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < size; i++)
        {
            int index = GameplayManager.GetInstance().GetRandomItemID();
            int amount = GameplayManager.GetInstance().GetRandomAmmountOfItem(index);
            Slot newSlot = new Slot(index, amount);
            CurrentItems.Add(newSlot);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
