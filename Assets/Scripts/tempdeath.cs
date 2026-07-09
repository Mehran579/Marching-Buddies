using UnityEngine;
using UnityEngine.SceneManagement;

public class tempdeath : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.CompareTag("Player"))
        {
            Debug.Log("WOuld hv been killed if i wrote the logic");
            collision.transform.parent.position = Vector3.zero;
        }
    }
}
