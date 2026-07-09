using UnityEngine;

public class Laser : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.parent.CompareTag("Player"))
        {
            if (!collision.transform.parent.GetComponent<PlayerManager>().ishidden)
            {
                Debug.Log("WOuld hv been killed if i wrote the logic");
                collision.transform.parent.position = Vector3.zero;
            }
        }
    }
}
