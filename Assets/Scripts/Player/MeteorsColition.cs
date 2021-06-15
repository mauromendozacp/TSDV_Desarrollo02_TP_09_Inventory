using UnityEngine;

public class MeteorsColition : MonoBehaviour
{
    [SerializeField]
    private float tumble;
    [SerializeField]
    private float initialForce;

    void Start() // Random Rotator
    {
        //GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;

        GetComponent<Rigidbody>().AddForce(-transform.up * initialForce, ForceMode.Force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemy")
        {
            other.transform.GetComponent<ItemSpawn>().GenerateNewItem();
            other.transform.GetComponent<Enemy>().onDie(other.transform.GetComponent<Enemy>());
            Destroy(other.gameObject);
        }
    }
}
