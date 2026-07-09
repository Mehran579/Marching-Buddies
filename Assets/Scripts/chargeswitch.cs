using System.Xml.Linq;
using UnityEngine;

public class chargeswitch : MonoBehaviour
{
    public GameObject door;
    public GameObject opendoor;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.parent.CompareTag("Player") && collision.transform.parent.GetComponent<PlayerManager>().ischarged)
        {
            door.SetActive(false);
            opendoor.SetActive(true);
        }
    }
}
