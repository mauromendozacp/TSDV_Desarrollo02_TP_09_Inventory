﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerMesh
{
    public Mesh armor;
    public Mesh gloves;
    public Mesh boots;
}

public class Player : Character
{
    [SerializeField] private GameObject[] playerMesh = new GameObject[7];
    [SerializeField] private GameObject hairMesh;
    [SerializeField] private PlayerMesh playerDefaultMesh;

    Equipment equipment;
    Inventory inventory;

    public delegate void RefreshMesh();
    public static RefreshMesh OnRefreshMeshAsStatic;

    public enum PlayerPart
    {
        Helmet,
        Shoulders,
        Armor,
        Gloves,
        Boots,
        Arms
    }

    private void Awake()
    {
       // equipment = GetComponent<Equipment>();
       // inventory = GetComponent<Inventory>();
    }

    private void Start()
    {
       // GameplayManager.GetInstance().SetPlayer(this);
       // OnRefreshMeshAsStatic += UpdateMesh;
    }

    void OnDestroy()
    {
       // OnRefreshMeshAsStatic -= UpdateMesh;
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

    public void UpdateMesh()
    {
       SetMesh(0, equipment.GetEquipmentList()[4].ID, PlayerPart.Helmet);
       SetMesh(1, equipment.GetEquipmentList()[5].ID, PlayerPart.Gloves);
       SetMesh(2, equipment.GetEquipmentList()[6].ID, PlayerPart.Boots);
       SetMesh(3, equipment.GetEquipmentList()[7].ID, PlayerPart.Shoulders);
       SetMesh(4, equipment.GetEquipmentList()[8].ID, PlayerPart.Armor);
       SetMesh(5, equipment.GetEquipmentList()[0].ID, PlayerPart.Arms);
       SetMesh(6, equipment.GetEquipmentList()[1].ID, PlayerPart.Arms);
    }

    public void SetMesh(int index, int id, PlayerPart part)
    {
        if (id != -1)
        {
            if (part == PlayerPart.Helmet)
            {
                playerMesh[index].GetComponent<SkinnedMeshRenderer>().sharedMesh =
                    GameplayManager.GetInstance().GetItemFromID(id).mesh;
                playerMesh[index].SetActive(true);
                hairMesh.SetActive(false);
            }
            if (part == PlayerPart.Arms)
            {
                playerMesh[index].GetComponent<MeshFilter>().mesh =
                    GameplayManager.GetInstance().GetItemFromID(id).mesh;

                if (index == 5)
                {
                    playerMesh[index].transform.localPosition =
                        ((Arms) (GameplayManager.GetInstance().GetItemFromID(id))).spawnPositionL.pos;
                    playerMesh[index].transform.localEulerAngles =
                        ((Arms)(GameplayManager.GetInstance().GetItemFromID(id))).spawnPositionL.rot;
                }
                else if(index == 6)
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
                    playerMesh[index].SetActive(false);
                    hairMesh.SetActive(true);

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

    bool IsMoving()
    {
        return Mathf.Abs(Input.GetAxis("Horizontal")) > 0f || Mathf.Abs(Input.GetAxis("Vertical")) > 0f;
    }

    void Update()
    {
        if (IsMoving())
        {
            SetMovement(direction, GetDirection());
            direction = GetDirection();
        }
    }
}
