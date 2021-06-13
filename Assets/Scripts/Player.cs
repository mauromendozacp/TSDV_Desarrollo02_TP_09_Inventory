using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class PlayerMesh
{
    public Mesh hair;
    public Mesh armor;
    public Mesh gloves;
    public Mesh boots;
}
public class Player : Character
{
    [SerializeField] private GameObject[] playerMesh = new GameObject[8];
    [SerializeField] private GameObject[] playerUIMesh = new GameObject[8];
    [SerializeField] private PlayerMesh playerDefaultMesh;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] LayerMask itemMask;
    [SerializeField] UiInventory uiInventory;

    public enum PlayerPart
    {
        Helmet,
        Shoulders,
        Armor,
        Gloves,
        Boots,
        Arms
    }

    Equipment equipment;
    Inventory inventory;
    Animator anim;

    public delegate void RefreshMesh();
    public static RefreshMesh OnRefreshMeshAsStatic;

    private bool movementAllowed = true;

    private void Awake()
    {
        equipment = GetComponent<Equipment>();
        inventory = GetComponent<Inventory>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
       GameplayManager.GetInstance().SetPlayer(this);
       OnRefreshMeshAsStatic += UpdateMesh;
    }

    void OnDestroy()
    {
       OnRefreshMeshAsStatic -= UpdateMesh;
    }

    public List<Slot> GetSaveSlots()
    {
        List<Slot> newList = new List<Slot>();

        for (int i = 0; i < equipment.GetEquipmentList().Count; i++)
        {
            newList.Add(equipment.GetEquipmentList()[i]);
        }

        for (int i = 0; i < inventory.GetInventoryList().Count; i++)
        {
            newList.Add(inventory.GetInventoryList()[i]);
        }
        return newList;
    }

    public void SetSaveSlots(List<Slot> newList)
    {
        int equipmentTotalSlots = equipment.GetEquipmentAmount();

        List<Slot> equipmentList = new List<Slot>();
        for (int i = 0; i < equipmentTotalSlots; i++)
        {
            equipmentList.Add(newList[i]);
        }
        equipment.SetNewEquipment(equipmentList);

        List<Slot> itemsList = new List<Slot>();
        for (int i = equipmentTotalSlots; i < newList.Count; i++)
        {
            itemsList.Add(newList[i]);
        }
        inventory.SetNewInventory(itemsList);
    }

    bool IsMoving()
    {
        return Mathf.Abs(Input.GetAxis("Horizontal")) > 0f || Mathf.Abs(Input.GetAxis("Vertical")) > 0f;
    }

    void UpdateMoveAnimation()
    {
        float speed = (Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical")));
        anim.SetFloat("Speed", speed);
    }

    void PickUp()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            anim.SetTrigger("PickUp");
        }
    }

    void OpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!inventoryPanel.activeSelf)
            {
                inventoryPanel.SetActive(true);
                UpdatePlayerUi();
                anim.SetFloat("Speed", 0.0f);
            }
            else
            {
                inventoryPanel.SetActive(false);
            }

            movementAllowed = !inventoryPanel.activeSelf;
        }
    }

    void Update()
    {
        OpenInventory();

        if (!movementAllowed)
            return;

        if (IsMoving() && !anim.GetCurrentAnimatorStateInfo(0).IsName("PickUp"))
        {
            SetMovement(direction, GetDirection());
            direction = GetDirection();
            UpdateMoveAnimation();
        }
        else
        {
            anim.SetFloat("Speed", 0f);
        }
        PickUp();
        
    }

    public void UpdateMesh()
    {
        SetMesh(0, equipment.GetEquipmentList()[4].ID, PlayerPart.Helmet);
        SetMesh(2, equipment.GetEquipmentList()[7].ID, PlayerPart.Shoulders);
        SetMesh(3, equipment.GetEquipmentList()[8].ID, PlayerPart.Armor);
        SetMesh(4, equipment.GetEquipmentList()[5].ID, PlayerPart.Gloves);
        SetMesh(5, equipment.GetEquipmentList()[6].ID, PlayerPart.Boots);
        SetMesh(6, equipment.GetEquipmentList()[0].ID, PlayerPart.Arms);
        SetMesh(7, equipment.GetEquipmentList()[1].ID, PlayerPart.Arms);

        UpdatePlayerUi();
    }

    void UpdatePlayerUi()
    {
        if (inventoryPanel.activeSelf)
        {
            for (int i = 0; i < playerMesh.Length - 2; i++)
            {
                playerUIMesh[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = playerMesh[i].GetComponent<SkinnedMeshRenderer>().sharedMesh;
                playerUIMesh[i].transform.localPosition = playerMesh[i].transform.localPosition;
                playerUIMesh[i].transform.localEulerAngles = playerMesh[i].transform.localEulerAngles;
            }
        }
    }

    public void SetMesh(int index, int id, PlayerPart part)
    {
        if (id != -1)
        {
            if (part == PlayerPart.Helmet)
            {
                playerMesh[index].GetComponent<SkinnedMeshRenderer>().sharedMesh =
                    GameplayManager.GetInstance().GetItemFromID(id).mesh;
                playerMesh[1].GetComponent<SkinnedMeshRenderer>().sharedMesh = new Mesh();
            }
            if (part == PlayerPart.Arms)
            {
                playerMesh[index].GetComponent<MeshFilter>().mesh =
                    GameplayManager.GetInstance().GetItemFromID(id).mesh;

                if (index == 5)
                {
                    playerMesh[index].transform.localPosition =
                        ((Arms)(GameplayManager.GetInstance().GetItemFromID(id))).spawnPositionL.pos;
                    playerMesh[index].transform.localEulerAngles =
                        ((Arms)(GameplayManager.GetInstance().GetItemFromID(id))).spawnPositionL.rot;
                }
                else if (index == 6)
                {
                    playerMesh[index].transform.localPosition =
                        ((Arms)(GameplayManager.GetInstance().GetItemFromID(id))).spawnPositionR.pos;
                    playerMesh[index].transform.localEulerAngles =
                        ((Arms)(GameplayManager.GetInstance().GetItemFromID(id))).spawnPositionR.rot;
                }
            }
            else
            {
                playerMesh[index].GetComponent<SkinnedMeshRenderer>().sharedMesh =
                    GameplayManager.GetInstance().GetItemFromID(id).mesh;
            }
        }
        else
        {
            switch (part)
            {
                case PlayerPart.Helmet:
                    playerMesh[index].GetComponent<SkinnedMeshRenderer>().sharedMesh = new Mesh();
                    playerMesh[1].GetComponent<SkinnedMeshRenderer>().sharedMesh = playerDefaultMesh.hair;

                    break;
                case PlayerPart.Shoulders:
                    playerMesh[index].GetComponent<SkinnedMeshRenderer>().sharedMesh = new Mesh();
                    break;

                case PlayerPart.Armor:
                    playerMesh[index].GetComponent<SkinnedMeshRenderer>().sharedMesh = playerDefaultMesh.armor;

                    break;
                case PlayerPart.Gloves:
                    playerMesh[index].GetComponent<SkinnedMeshRenderer>().sharedMesh = playerDefaultMesh.gloves;

                    break;
                case PlayerPart.Boots:
                    playerMesh[index].GetComponent<SkinnedMeshRenderer>().sharedMesh = playerDefaultMesh.boots;

                    break;
                case PlayerPart.Arms:
                    playerMesh[index].GetComponent<MeshFilter>().sharedMesh = new Mesh();

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(Contains(itemMask, other.gameObject.layer)){
            if(Input.GetKeyDown(KeyCode.E)){
                int _id = other.gameObject.GetComponent<ItemData>().itemID;
                int _amount = other.gameObject.GetComponent<ItemData>().itemAmount;
                inventory.AddNewItem(_id, _amount);
                uiInventory.RefreshAllButtons();
                Destroy(other.gameObject);
            }
        }
    }

    bool Contains(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
}