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
    [SerializeField] LayerMask enemyMask;
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
    private Rigidbody rb;

    public delegate void RefreshMesh();
    public static RefreshMesh OnRefreshMeshAsStatic;

    private bool movementAllowed = true;
    private bool m_IsGrounded;
    private float m_OrigGroundCheckDistance;
    private float m_GroundCheckDistance = 0.2f;
    private float m_GravityMultiplier = 1.5f;
    private float jumpSpeed = 15f;

    public delegate void OnDieDelegate();
    public OnDieDelegate onDie;
    public delegate void OnLivesChangedDelegate(int lives);
    public OnLivesChangedDelegate onLivesChanged;

    [SerializeField] int lives = 5;

    private void Awake()
    {
        equipment = GetComponent<Equipment>();
        inventory = GetComponent<Inventory>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        m_OrigGroundCheckDistance = m_GroundCheckDistance;

        OnRefreshMeshAsStatic += UpdateMesh;
    }

    private void Start()
    {
        UpdateMesh();    
    }

    void OnDestroy()
    {
       OnRefreshMeshAsStatic -= UpdateMesh;
    }

    public void AddDamage()
    {
        lives--;
        onLivesChanged?.Invoke(lives);

        if (lives == 0)
        {
            onDie?.Invoke();
        }
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

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
        {
            m_IsGrounded = true;
        }
        else
        {
            m_IsGrounded = false;
        }
    }

    void HandleAirborneMovement()
    {
        Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
        rb.AddForce(extraGravityForce);

        m_GroundCheckDistance = rb.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
    }

    void SetAttack()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        bool attacking = stateInfo.IsName("Attack");
        if(Input.GetKeyDown(KeyCode.Mouse0) && !attacking && !inventoryPanel.activeSelf)
        {
            anim.SetTrigger("Attack");
            Collider[] hitColliders = Physics.OverlapSphere((transform.position + new Vector3(0f, 2.5f, 0f)) + transform.forward * 1.5f, 1.5f, enemyMask);
            foreach(Collider hitColider in hitColliders)
            {
                hitColider.transform.GetComponent<ItemSpawn>().GenerateNewItem();
                Destroy(hitColider.gameObject);                
            }
        }
    }    

    void SetJump()
    {
        rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        m_IsGrounded = false;
        m_GroundCheckDistance = 0.1f;
    }

    void Update()
    {
        OpenInventory();
        SetAttack();

        if (!movementAllowed)
            return;

        if (IsMoving() && !anim.GetCurrentAnimatorStateInfo(0).IsName("PickUp") 
            && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") 
            && !anim.GetCurrentAnimatorStateInfo(0).IsName("MeteorShower"))
        {
            SetMovement(direction, GetDirection());
            direction = GetDirection();
            UpdateMoveAnimation();
        }
        else
        {
            anim.SetFloat("Speed", 0f);
        }
        CheckGroundStatus();
        if (m_IsGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SetJump();
            }
        }
        else
        {
            HandleAirborneMovement();
        }
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
            for (int i = 0; i < playerMesh.Length; i++)
            {
                if (i < 6)
                {
                    playerUIMesh[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = playerMesh[i].GetComponent<SkinnedMeshRenderer>().sharedMesh;
                }
                else
                {
                    playerUIMesh[i].GetComponent<MeshFilter>().mesh = playerMesh[i].GetComponent<MeshFilter>().mesh;
                }

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

                if (index == 6)
                {
                    playerMesh[index].transform.localPosition =
                        ((Arms)(GameplayManager.GetInstance().GetItemFromID(id))).spawnPositionL.pos;
                    playerMesh[index].transform.localEulerAngles =
                        ((Arms)(GameplayManager.GetInstance().GetItemFromID(id))).spawnPositionL.rot;
                }
                else if (index == 7)
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
        if(Contains(itemMask, other.gameObject.layer))
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                anim.SetTrigger("PickUp");

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