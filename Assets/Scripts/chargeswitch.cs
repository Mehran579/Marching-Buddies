//using System.Xml.Linq;
using UnityEngine;

public class chargeswitch : MonoBehaviour
{
    public GameObject door;
    public GameObject opendoor;
    public GameObject glowingwires;
    public GameObject glowingwires2;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.parent.CompareTag("Player") && collision.transform.parent.GetComponent<PlayerManager>().ischarged)
        {
            door.SetActive(false);
            opendoor.SetActive(true);
            glowingwires.SetActive(true);
            glowingwires2.SetActive(true);
        }
    }
}
