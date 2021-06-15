using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] ItemList allItems;

    [SerializeField] Player player;
    [SerializeField] EnemiesManager enemiesManager;
    [SerializeField] UIGameplay uiGameplay;

    [SerializeField] GameObject itemPrefab;
    private const float itemArmScale = 0.018f;

    static private GameplayManager instance;

    static public GameplayManager GetInstance() { return instance; }

    string savePath = "SaveFile.json";

    public bool Paused { get; set; }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;

            LoadJson();

            Player.OnRefreshMeshAsStatic();

            player.onLivesChanged += uiGameplay.UpdateLives;
            enemiesManager.onNewEnemyCreated += SetEnemyAttack;
            enemiesManager.onEnemiesAmountChanged += uiGameplay.UpdateEnemiesAmount;

            enemiesManager.onAllEnemiesKilled += GameManager.Instance.FinishGame;
            player.onDie += GameManager.Instance.FinishGame;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetEnemyAttack(Enemy enemy)
    {
        enemy.onAttackPlayer += player.AddDamage;
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

    public void GenerateItemInWorldSpace(int itemID, int randomAmount, Vector3 SpawnPosition)
    {
        GameObject item = Instantiate(itemPrefab, SpawnPosition, Quaternion.identity);
        item.GetComponent<MeshFilter>().mesh = GetInstance().GetItemFromID(itemID).mesh;
        item.GetComponent<MeshCollider>().sharedMesh = GetInstance().GetItemFromID(itemID).mesh;
        item.GetComponent<ItemData>().itemID = itemID;
        item.GetComponent<ItemData>().itemAmount = randomAmount;
        item.GetComponent<MeshRenderer>().material = GetItemFromID(itemID).material;
        Instantiate(GetItemFromID(itemID).particle, item.transform);

        if (GetItemFromID(itemID).GetItemType() == ItemType.Arms)
        {
            item.transform.localScale *= itemArmScale;
        }
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
        string savedData;
        if (File.Exists(savePath))
        {
            FileStream fs;
            fs = File.Open(savePath, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            savedData = br.ReadString();
            fs.Close();
            br.Close();
        }
        else
        {
            return;
        }
        List<Slot> newList = new List<Slot>();
        for (int i = 0; i < savedData.Length; i++)
        {
            if (savedData[i] == '{')
            {
                string slotString = "";
                int aux = 0;
                while (savedData[i + aux] != '}')
                {
                    slotString += savedData[i + aux];
                    aux++;
                }
                slotString += '}';
                Slot newSlot = JsonUtility.FromJson<Slot>(slotString);
                newList.Add(newSlot);
            }
        }
        player.SetSaveSlots(newList);
    }

    void OnDestroy()
    {
        SaveJson();
    }
}