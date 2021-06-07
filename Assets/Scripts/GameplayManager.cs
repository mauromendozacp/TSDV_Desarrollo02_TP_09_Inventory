using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{

    [SerializeField] ItemList allItems;

    Player player;

    static private GameplayManager instance;

    static public GameplayManager GetInstance() { return instance; }

    string savePath = "SaveFile.json";

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
        return Random.Range(0, allItems.List.Count);
    }

    public int GetRandomAmmountOfItem(int id)
    {
        return Random.Range(1, allItems.List[id].maxStack);
    }

    public Item GetItemFromID(int id)
    {
        return allItems.List[id];
    }

    public void SetPlayer(Player p)
    {
        player = p;
    }

    public void SaveJson()
    {
        List<Slot> playerItems = player.GetSaveSlots();
        string json = "";
        for (int i = 0; i < playerItems.Count; i++)
        {
            json += JsonUtility.ToJson(playerItems[i]);
        }

        FileStream fs;

        if (!File.Exists(savePath))
            fs = File.Create(savePath);
        else
            fs = File.Open(savePath, FileMode.Truncate);
   
        BinaryWriter bw = new BinaryWriter(fs);
        bw.Write(json);
        fs.Close();
        bw.Close();
    }

    public void LoadJson()
    {
        if (File.Exists(savePath))
        {
            FileStream fs;
            fs = File.Open(savePath, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            string savedData = br.ReadString();
            fs.Close();
            br.Close();
        }
        else
        {
            Debug.Log("file not found");
        }
    }

}