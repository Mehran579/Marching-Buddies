using UnityEngine;
using UnityEngine.SceneManagement;

public class WalkerVision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if it's the player
        if (!other.CompareTag("Player"))
            return;

        // Get PlayerManager
        PlayerManager player = other.GetComponent<PlayerManager>();

        if (player == null)
            return;

        // If invisible, walker ignores the player
        if (player.ishidden)
        {
            Debug.Log("Player is invisible.");
            return;
        }

        // Any other ability = detected
        Debug.Log("Player detected! Restarting level...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
