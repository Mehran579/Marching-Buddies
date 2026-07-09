using UnityEngine;
using UnityEngine.SceneManagement;

public class tempteleport : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        if (collision.CompareTag("Player")) 
        {
        }
    }
}
