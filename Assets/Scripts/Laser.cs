using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject panel;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.parent.CompareTag("Player"))
        {
            if (!collision.transform.parent.GetComponent<PlayerManager>().ishidden)
            {
                //Debug.Log("WOuld hv been killed if i wrote the logic");
                panel.SetActive(true);
                collision.transform.parent.position = Vector3.zero;
            }
        }
    }
}
