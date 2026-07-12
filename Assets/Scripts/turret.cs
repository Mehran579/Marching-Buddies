using UnityEngine;

public class turret : MonoBehaviour
{
    GameObject leftthand;
    public GameObject missile;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent.CompareTag("Player") && !other.transform.parent.GetComponent<PlayerManager>().ishidden)
        {
            Debug.Log("why");
            transform.localScale = new Vector3(-1, 1, 1);
            GameObject nmissile = Instantiate(missile,transform.position,Quaternion.identity);
            nmissile.GetComponent<Rigidbody2D>().AddForce((other.transform.position - transform.position).normalized * 14f, ForceMode2D.Impulse);
        }
    }
}
