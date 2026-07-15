using UnityEngine;
using UnityEngine.SceneManagement;

public class WalkerAI : MonoBehaviour
{
    [Header("Patrol")]
    public float leftLimit;
    public float rightLimit;
    public float speed = 2f;

    [Header("Detection")]
    public float visionDistance = 5f;

    private int direction = 1; // 1 = right, -1 = left
    private PlayerManager player;

    void Start()
    {
        player = FindObjectOfType<PlayerManager>();
    }

    void Update()
    {
        Patrol();
        DetectPlayer();
    }

    void Patrol()
    {
        // Move
        transform.position += Vector3.right * direction * speed * Time.deltaTime;

        // Turn at right limit
        if (transform.position.x >= rightLimit)
        {
            direction = -1;
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // Turn at left limit
        if (transform.position.x <= leftLimit)
        {
            direction = 1;
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void DetectPlayer()
    {
        if (player == null)
            return;

        // Ignore player behind the walker
        if (direction == 1 && player.transform.position.x < transform.position.x)
            return;

        if (direction == -1 && player.transform.position.x > transform.position.x)
            return;

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance > visionDistance)
            return;

        // Invisible = ignore player
        if (player.ishidden)
            return;

        // Charge = destroy walker
        if (player.ischarged)
        {
            Destroy(gameObject);
            return;
        }

        // All other powers = kill player
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if (direction == 1)
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * visionDistance);
        else
            Gizmos.DrawLine(transform.position, transform.position + Vector3.left * visionDistance);
    }
}
