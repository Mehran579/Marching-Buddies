using UnityEngine;

public class tutotrigger : MonoBehaviour
{
    public GameObject tutp;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);
        Debug.Log(collision.transform.parent);
        
        if (collision!= null &&collision.transform.parent.CompareTag("Player"))
        {
            Invoke(nameof(settutp),5f);
        }
    }
    void settutp()
    {
        tutp.SetActive(true);
    }
}
