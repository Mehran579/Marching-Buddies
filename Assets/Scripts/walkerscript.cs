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
    public Rigidbody2D rb;
    public bool chaseplayer;
    void Start()
    {
        //player = FindObjectOfType<PlayerManager>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
    }

    private void Update()
    {
        chaseplayer = DetectPlayer();
    }
    void FixedUpdate()
    {
        Patrol();
        //DetectPlayer();
    }

    void Patrol()
    {
        // Move
        //transform.position += Vector3.right * direction * speed * Time.deltaTime;
        // use rigidbody to move transform ignore physics

        //first will add the scenario when walker detects the player;
        if (chaseplayer)
        {
            rb.linearVelocity = new Vector2(Mathf.Sign(player.transform.position.x - rb.position.x) * speed, rb.linearVelocity.y);
            transform.localScale = new Vector3(Mathf.Sign(player.transform.position.x - rb.position.x), 1, 1);
        }
        // Turn at right limit
        else
        {
            //now the normal patrol of the walker;
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
            if (rb.position.x >= rightLimit)
        {
            direction = -1;
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // Turn at left limit
        if (rb.position.x <= leftLimit)
        {
            direction = 1;
            transform.localScale = new Vector3(1, 1, 1);
        }
        }
    }

    bool DetectPlayer()
    {
        if (player == null) return false;

        // Ignore player behind the walker
        if (direction == 1 && player.transform.position.x < transform.position.x)
            return false;

        if (direction == -1 && player.transform.position.x > transform.position.x)
            return false;

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance > visionDistance)
            return false;

        // Invisible = ignore player
        if (player.ishidden)
            return false;

        // Charge = destroy walker,,,,,,, no charge = destroy walker on contact;

        // All other powers = kill player, no all other power means now walker have to follow/attack the player kills happens through contact not through line of sight;
        return true;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (player.ishidden)
            {
                return;
            }else if (player.ischarged)
            {
                Destroy(gameObject);
            }else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
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
