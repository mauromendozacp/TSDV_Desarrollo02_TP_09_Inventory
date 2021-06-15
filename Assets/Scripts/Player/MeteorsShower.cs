using UnityEngine;

public class MeteorsShower : MonoBehaviour
{
    [SerializeField] private GameObject meteorsParent;
    [SerializeField] private GameObject inventoryPanel;

    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        SetMeteorShower();
    }

    void SetMeteorShower()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        bool shower = stateInfo.IsName("MeteorShower");
        if (Input.GetKeyDown(KeyCode.Mouse1) && !shower && !inventoryPanel.activeSelf)
        {
            anim.SetTrigger("MeteorShower");
            SpawnMeteors();
        }
    }

    void SpawnMeteors()
    {
        GameObject parentMeteors = Instantiate(meteorsParent);
        parentMeteors.transform.name = "Magic Meteors";
        parentMeteors.transform.position = transform.position + new Vector3(0f, 1.5f, 0f) + transform.forward * 10;

        Destroy(parentMeteors, 2f);
    }
}
